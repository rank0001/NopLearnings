using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.TopSellingProducts.Factories;
using Nop.Plugin.Widgets.TopSellingProducts.Services;
using Nop.Services.Configuration;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.TopSellingProducts.Components;
public class TopSellingProductsViewComponent(ISettingService settingService,
                                             ITopSellingProductService topSellingProductService,
                                             ITopSellingProductFactory topSellingProductFactory) : NopViewComponent
{
    private readonly ISettingService _settingService = settingService;
    private readonly ITopSellingProductService _topSellingProductService = topSellingProductService;
    private readonly ITopSellingProductFactory _topSellingProductFactory = topSellingProductFactory;    
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        var setting = await _settingService.LoadSettingAsync<TopSellingProductsSettings>();

        var products = await _topSellingProductService.GetTopSellingProductsAsync(setting, 1);

        var model = await _topSellingProductFactory.PrepareModelAsync(products, setting);

        return View("~/Plugins/Widgets.TopSellingProducts/Views/PublicInfo.cshtml", model);
    }
}
