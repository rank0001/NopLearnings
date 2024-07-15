using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.TopSellingProducts.Components;
public class TopSellingProductsViewComponent: NopViewComponent
{
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        return View("~/Plugins/Widgets.TopSellingProducts/Views/PublicInfo.cshtml");
    }
}
