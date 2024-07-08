using Nop.Plugin.Widgets.CustomTest.Domain;

namespace Nop.Plugin.Widgets.CustomTest.Services;

public interface IProductViewTrackerService
{
    /// <summary>
    /// Logs the specified record.
    /// </summary>
    /// <param name="record">The record.</param>
    void Log(ProductViewTrackerRecord record);
}