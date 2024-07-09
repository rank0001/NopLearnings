
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomTest.Models;
public record StudentModel : BaseNopEntityModel
{
    [NopResourceDisplayName("Admin.Plugins.Crudtest.SearchTitle")]
    public string? SearchTitle { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
}
