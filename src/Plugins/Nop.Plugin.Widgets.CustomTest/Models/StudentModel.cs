
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.CustomTest.Models;
public record StudentModel : BaseNopModel
{
    public int Id { get; set; } 
    public int Age { get; set; }
    public string Name { get; set; }
}
