using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.TopSellingProducts.Models;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.TopSellingProducts.Factories;
public interface ITopSellingProductFactory
{
    Task<TopSellingProductsModel> PrepareModelAsync(
    IEnumerable<Product> products,
    TopSellingProductsSettings settings);
}
