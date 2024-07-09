using Microsoft.AspNetCore.Routing;
using Nop.Plugin.Widgets.CustomTest.Components;
using Nop.Plugin.Widgets.CustomTest.Domain;
using Nop.Services.Cms;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.CustomTest;

public class CustomTestPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
{
    public bool HideInWidgetList => false;

    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(CustomTestViewComponent);
    }

    //public Task<IList<string>> GetWidgetZonesAsync()
    //{
    //    return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBeforeNews });
    //}

    public Task ManageSiteMapAsync(SiteMapNode rootNode)
    {
        var menuItem = new SiteMapNode()
        {
            SystemName = "CustomTest",
            Title = "Crud Test",
            ControllerName = "CustomTest",
            ActionName = "TestCrudMenu",
            IconClass = "far fa-dot-circle",
            Visible = true,
            RouteValues = new RouteValueDictionary() { { "area", AreaNames.ADMIN } },
        };

        var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
        //if (pluginNode != null)
        //    pluginNode.ChildNodes.Add(menuItem);
        //else
        //    rootNode.ChildNodes.Add(menuItem);

        pluginNode.ChildNodes.Add(menuItem);   

        return Task.CompletedTask;
    }

    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.ProductDetailsTop });
    }

    public override async Task InstallAsync()
    {
        await base.InstallAsync();
    }
    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }
}
