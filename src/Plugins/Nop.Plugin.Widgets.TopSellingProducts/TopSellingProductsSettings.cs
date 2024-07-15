using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;

namespace Nop.Plugin.Widgets.TopSellingProducts;
public class TopSellingProductsSettings : ISettings
{
    public DateTime FromDate { get; set; } = DateTime.Now.AddDays(-30);

    public DateTime ToDate { get; set; } = DateTime.Now;

    public int TotalCount { get; set; } = 10;

}
