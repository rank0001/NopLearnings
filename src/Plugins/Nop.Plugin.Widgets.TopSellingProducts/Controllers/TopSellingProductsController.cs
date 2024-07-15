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

namespace Nop.Plugin.Widgets.TopSellingProducts.Controllers;

[Area(AreaNames.ADMIN)]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]

public class TopSellingProductsController : BasePluginController
{
    public IActionResult Configure()
    {
        return View("~/Plugins/Widgets.NivoSlider/Views/Configure.cshtml");
    }

}

