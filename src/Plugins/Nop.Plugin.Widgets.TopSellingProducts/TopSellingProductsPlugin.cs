using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing;
using Nop.Plugin.Widgets.TopSellingProducts.Components;
using Nop.Services.Cms;
using Nop.Services.Orders;
using Nop.Services.Plugins;
using Nop.Web.Framework;
using Nop.Web.Framework.Infrastructure;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Widgets.TopSellingProducts
{
    public class TopSellingProductsPlugin : BasePlugin, IWidgetPlugin, IAdminMenuPlugin
    {
        public bool HideInWidgetList => false;

        public Type GetWidgetViewComponent(string widgetZone)
        {
            return typeof(TopSellingProductsViewComponent);
        }

        public Task<IList<string>> GetWidgetZonesAsync()
        {
            return Task.FromResult<IList<string>>(new List<string> { PublicWidgetZones.HomepageBeforeNews });
        }

        public override async Task InstallAsync()
        {
            await base.InstallAsync();
        }

        public override async Task UninstallAsync()
        {
            await base.UninstallAsync();
        }

        public Task ManageSiteMapAsync(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                SystemName = "TopSellingProducts",
                Title = "Configure",
                ControllerName = "TopSellingProducts",
                ActionName = "Configure",
                IconClass = "far fa-dot-circle",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", AreaNames.ADMIN } },
            };

            var listofNodes = rootNode.ChildNodes.Select(x => x.SystemName).ToList();

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");

            if (pluginNode != null)
            {
                pluginNode.Title = "Top Selling Products";
                pluginNode.ChildNodes.Add(menuItem);
            }
            else
                rootNode.ChildNodes.Add(menuItem);


            return Task.CompletedTask;
        }
    }
}
