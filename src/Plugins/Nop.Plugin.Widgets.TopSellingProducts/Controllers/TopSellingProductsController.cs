using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using DocumentFormat.OpenXml.EMMA;
using Nop.Core;
using Nop.Plugin.Widgets.TopSellingProducts.Models;
using Nop.Services.Configuration;
using Nop.Services.Messages;
using Nop.Services.Localization;

namespace Nop.Plugin.Widgets.TopSellingProducts.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]

public class TopSellingProductsController(IStoreContext storeContext,
                                          ISettingService settingService,
                                          INotificationService notificationService,
                                          ILocalizationService localizationService) : BasePluginController
{
    private readonly IStoreContext _storeContext = storeContext;
    private readonly ISettingService _settingService = settingService;
    private readonly INotificationService _notificationService = notificationService;
    private readonly ILocalizationService _localizationService = localizationService;

    public async Task<IActionResult> Configure()
    {
        var topSellingProductsSettings = await _settingService.LoadSettingAsync<TopSellingProductsSettings>();

        var model = new ConfigurationModel
        {
            FromDate = topSellingProductsSettings.FromDate ,
            ToDate = topSellingProductsSettings.ToDate,
            TotalDisplay = topSellingProductsSettings.TotalDisplay
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Configure(ConfigurationModel model)
    {
        if (ModelState.IsValid)
        {
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var topSellingProductsSettings = await _settingService.LoadSettingAsync<TopSellingProductsSettings>();

            topSellingProductsSettings.FromDate = model.FromDate ;
            topSellingProductsSettings.ToDate = model.ToDate;
            topSellingProductsSettings.TotalDisplay = model.TotalDisplay;

            await _settingService.SaveSettingAsync(topSellingProductsSettings, storeScope);

            _notificationService.SuccessNotification(
                await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return RedirectToAction("Configure", "TopSellingProducts");
        }

        return View(model);
    }

}

