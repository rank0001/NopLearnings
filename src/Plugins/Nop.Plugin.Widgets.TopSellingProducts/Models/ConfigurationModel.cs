using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.TopSellingProducts.Models;
public record ConfigurationModel : BaseNopModel
{
    [UIHint("DateTime")]
    public DateTime FromDate { get; set; }

    [UIHint("DateTime")]
    public DateTime ToDate { get; set; }

    [UIHint("Number")]
    public int TotalDisplay { get; set; }
}
