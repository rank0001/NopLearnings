using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.CustomTest.Models;
public partial record StudentCreateModel : BaseNopEntityModel
{
    public int Age { get; set; }
    public string Name { get; set; }
}
