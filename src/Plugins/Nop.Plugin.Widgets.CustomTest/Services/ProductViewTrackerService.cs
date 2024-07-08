using Nop.Data;
using Nop.Plugin.Widgets.CustomTest.Domain;

namespace Nop.Plugin.Widgets.CustomTest.Services;
public class ProductViewTrackerService:IProductViewTrackerService
{
    private readonly IRepository<ProductViewTrackerRecord> _productViewTrackerRecordRepository;
    public ProductViewTrackerService(IRepository<ProductViewTrackerRecord> productViewTrackerRecordRepository)
    {
        _productViewTrackerRecordRepository = productViewTrackerRecordRepository;
    }
    /// <summary>
    /// Logs the specified record.
    /// </summary>
    /// <param name="record">The record.</param>
    public virtual void Log(ProductViewTrackerRecord record)
    {
        if (record == null)
            throw new ArgumentNullException(nameof(record));
        _productViewTrackerRecordRepository.Insert(record);
    }
}
