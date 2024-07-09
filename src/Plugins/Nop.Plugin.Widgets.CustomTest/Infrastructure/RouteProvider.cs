using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Framework;

namespace Nop.Plugin.Widgets.CustomTest.Infrastructure;

/// <summary>
/// Represents plugin route provider
/// </summary>
public class RouteProvider : IRouteProvider
{
    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {

        endpointRouteBuilder.MapControllerRoute(name: "Plugin.Widgets.CustomTest.Test",
            pattern: "CustomTest/Test",
            defaults: new { controller = "CustomTest", action = "Test" });
        endpointRouteBuilder.MapControllerRoute(name: "Plugin.Widgets.CustomTest.Test",
            pattern: "Admin/CustomTest/TestCrudMenu",
            defaults: new { controller = "CustomTest", action = "TestCrudMenu", area = AreaNames.ADMIN });
        //endpointRouteBuilder.MapControllerRoute(name: "Plugin.Widgets.CustomTest.Test",
        //    pattern: "Admin/CustomTest/GetAllStudentListAsync",
        //    defaults: new { controller = "CustomTest", action = "GetAllStudentList", area = AreaNames.ADMIN });
    }

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 0;
}