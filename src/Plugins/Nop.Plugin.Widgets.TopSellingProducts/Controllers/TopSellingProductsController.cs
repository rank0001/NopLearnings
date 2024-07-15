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

namespace Nop.Plugin.Widgets.TopSellingProducts.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]

public class TopSellingProductsController(IStoreContext storeContext) : BasePluginController
{
    private readonly IStoreContext _storeContext = storeContext;
    public IActionResult Configure()
    {
        var model = new ConfigurationModel();
        return View("~/Plugins/Widgets.TopSellingProducts/Views/Configure.cshtml", model);
    }

}

