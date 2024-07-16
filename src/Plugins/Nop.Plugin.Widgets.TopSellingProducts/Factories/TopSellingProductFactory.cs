using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.TopSellingProducts.Models;
using Nop.Web.Factories;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.TopSellingProducts.Factories;
public class TopSellingProductFactory(IProductModelFactory productModelFactory) : ITopSellingProductFactory
{
    private readonly IProductModelFactory _productModelFactory = productModelFactory;

    public async Task<TopSellingProductsModel> PrepareModelAsync(
    IEnumerable<Product> products,
    TopSellingProductsSettings settings)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(products));

        var productOverviewModels = await _productModelFactory.PrepareProductOverviewModelsAsync(products);

        return new TopSellingProductsModel
        {
            Products = productOverviewModels.ToList()
        };
    }
}
