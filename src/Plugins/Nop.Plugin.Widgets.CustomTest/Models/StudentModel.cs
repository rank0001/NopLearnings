
using Nop.Core.Domain.Blogs;
using Nop.Web.Areas.Admin.Models.Blogs;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomTest.Models;
public record StudentModel : BaseNopEntityModel
{
    public StudentModel()
    {
        StudentPosts = new StudentSearchModel();
    }
    [NopResourceDisplayName("Admin.Plugins.Crudtest.SearchTitle")]
    public string SearchTitle { get; set; }
    public int Age { get; set; }
    public string Name { get; set; }
    public StudentSearchModel StudentPosts { get; set; }
}
