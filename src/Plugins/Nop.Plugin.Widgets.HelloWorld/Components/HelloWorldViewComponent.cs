using Microsoft.AspNetCore.Mvc;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Widgets.HelloWorld.Components;
public class HelloWorldViewComponent : NopViewComponent
{
    public IViewComponentResult Invoke(string widgetZone, object additionalData)
    {
        return View("~/Plugins/Widgets.HelloWorld/Views/HelloWorld.cshtml");
    }
}
