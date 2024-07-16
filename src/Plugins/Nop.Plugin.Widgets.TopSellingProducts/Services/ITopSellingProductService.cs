using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widgets.TopSellingProducts.Services;
public interface ITopSellingProductService
{
    Task<IList<Product>> GetTopSellingProductsAsync(TopSellingProductsSettings topSoldsettings, int storeId);
}
