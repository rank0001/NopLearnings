using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Orders;
using Nop.Core;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.Payments.Stripe.Models;
using Nop.Plugin.Payments.Stripe.Enums;
using Nop.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Payments.Stripe.Controllers;
public class PaymentStripeController: BasePaymentController
{
    private readonly IPermissionService _permissionService;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ISettingService _settingService;
    private readonly IStoreContext _storeContext;
    private readonly IWebHelper _webHelper;
    
    public PaymentStripeController(IPermissionService permissionService,
                                   ILocalizationService localizationService,
                                   INotificationService notificationService,
                                   ISettingService settingService,
                                   IStoreContext storeContext,
                                   IWebHelper webHelper)
    {
        _permissionService = permissionService;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _settingService = settingService;
        _storeContext = storeContext;
        _webHelper = webHelper;
    }

    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<IActionResult> Configure()
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
            return AccessDeniedView();

        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var stripePaymentSettings = await _settingService.LoadSettingAsync<StripePaymentSettings>(storeScope);

        var model = new ConfigurationModel
        {
            UseSandbox = stripePaymentSettings.UseSandbox,
            Title = stripePaymentSettings.Title,
            TestPublishableKey = stripePaymentSettings.TestPublishableKey,
            TestSecretKey = stripePaymentSettings.TestSecretKey,
            LivePublishableKey = stripePaymentSettings.LivePublishableKey,
            LiveSecretKey = stripePaymentSettings.LiveSecretKey,
            AdditionalFee = stripePaymentSettings.AdditionalFee,
            AdditionalFeePercentage = stripePaymentSettings.AdditionalFeePercentage,
            ActiveStoreScopeConfiguration = storeScope,
            PaymentTypeId = (int) stripePaymentSettings.PaymentType
        };

        if (storeScope > 0)
        {
            model.UseSandbox_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.UseSandbox, storeScope);
            model.Title_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.Title, storeScope);
            model.TestPublishableKey_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.TestPublishableKey, storeScope);
            model.TestSecretKey_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.TestSecretKey, storeScope);
            model.LivePublishableKey_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.LivePublishableKey, storeScope);
            model.LiveSecretKey_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.LiveSecretKey, storeScope);
            model.AdditionalFee_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.AdditionalFee, storeScope);
            model.AdditionalFeePercentage_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.AdditionalFeePercentage, storeScope);
            model.PaymentTypeId_OverrideForStore = await _settingService.SettingExistsAsync(stripePaymentSettings, x => x.PaymentType, storeScope);
        }

        model.PaymentTypes = await (await PaymentType.Authorize.ToSelectListAsync(false))
                            .Select(item => new SelectListItem(item.Text, item.Value))
                            .ToListAsync();

        return View("~/Plugins/Payments.Stripe/Views/Configure.cshtml", model);
    }

    [HttpPost]
    [AuthorizeAdmin]
    [Area(AreaNames.ADMIN)]
    [AutoValidateAntiforgeryToken]
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
            return AccessDeniedView();

        if (!ModelState.IsValid)
            return await Configure();

        //load settings for a chosen store scope
        var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
        var stripePaymentSettings = await _settingService.LoadSettingAsync<StripePaymentSettings>(storeScope);

        //save settings
        stripePaymentSettings.UseSandbox = model.UseSandbox;
        stripePaymentSettings.Title = model.Title;
        stripePaymentSettings.TestPublishableKey = model.TestPublishableKey;
        stripePaymentSettings.TestSecretKey = model.TestSecretKey;
        stripePaymentSettings.LivePublishableKey = model.LivePublishableKey;
        stripePaymentSettings.LiveSecretKey = model.LiveSecretKey;
        stripePaymentSettings.AdditionalFee = model.AdditionalFee;
        stripePaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;
        stripePaymentSettings.PaymentType = (PaymentType)model.PaymentTypeId;

        /* We do not clear cache after each setting update.
         * This behavior can increase performance because cached settings will not be cleared 
         * and loaded from database after each update */
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.UseSandbox, model.UseSandbox_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.Title, model.Title_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.TestPublishableKey, model.TestPublishableKey_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.TestSecretKey, model.TestSecretKey_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.LivePublishableKey, model.LivePublishableKey_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.LiveSecretKey, model.LiveSecretKey_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, storeScope, false);
        await _settingService.SaveSettingOverridablePerStoreAsync(stripePaymentSettings, x => x.PaymentType, model.PaymentTypeId_OverrideForStore, storeScope, false);
        //now clear settings cache
        await _settingService.ClearCacheAsync();

        _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

        return await Configure();
    }

}
