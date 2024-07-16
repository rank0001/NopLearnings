using DocumentFormat.OpenXml.InkML;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Configuration;
using Nop.Core.Domain.Orders;
using Nop.Data;

namespace Nop.Plugin.Widgets.TopSellingProducts.Services;
public class TopSellingProductService(IRepository<Product> productRepository,
                                      IRepository<OrderItem> orderItemRepository,
                                      IRepository<Order> orderRepository) : ITopSellingProductService
{
    private readonly IRepository<Product> _productRepository = productRepository;
    private readonly IRepository<OrderItem> _orderItemRepository = orderItemRepository;
    private readonly IRepository<Order> _orderRepository = orderRepository;

    public async Task<IList<Product>> GetTopSellingProductsAsync(TopSellingProductsSettings topSoldsettings, int storeId)
    {
        var orderQuery = _orderRepository.Table;

        if (topSoldsettings.FromDate != null)
        {
            orderQuery = orderQuery.Where(o => o.CreatedOnUtc >= topSoldsettings.FromDate);
        }

        if (topSoldsettings.ToDate != null)
        {
            orderQuery = orderQuery.Where(o => o.CreatedOnUtc <= topSoldsettings.ToDate);
        }

        var query = from o in orderQuery
                    join oi in _orderItemRepository.Table on o.Id equals oi.OrderId
                    join p in _productRepository.Table on oi.ProductId equals p.Id
                    group new { oi.ProductId } by oi.ProductId into g
                    let total = g.Count()
                    orderby total descending
                    select new
                    {
                        ProductId = g.Key,
                        Total = total
                    };

        var result = from a in query
                     join p in _productRepository.Table on a.ProductId equals p.Id
                     orderby a.Total descending
                     select p;

        return await result.Take(topSoldsettings.TotalDisplay).ToListAsync();
    }
}

