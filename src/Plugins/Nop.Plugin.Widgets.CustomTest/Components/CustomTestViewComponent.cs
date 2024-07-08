using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Widgets.CustomTest.Domain;
using Nop.Plugin.Widgets.CustomTest.Services;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Web.Framework.Components;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Widgets.CustomTest.Components;
public class CustomTestViewComponent : NopViewComponent
{
    private readonly ICustomerService _customerService;
    private readonly IProductService _productService;
    private readonly IProductViewTrackerService _productViewTrackerService;
    private readonly IWorkContext _workContext;

    public CustomTestViewComponent(ICustomerService customerService,
    IProductService productService,
    IProductViewTrackerService productViewTrackerService,
    IWorkContext workContext)
    {
        _customerService = customerService;
        _productService = productService;
        _productViewTrackerService = productViewTrackerService;
        _workContext = workContext;
    }
    //public IViewComponentResult Invoke(string widgetZone, object additionalData)
    //{
    //    return View("~/Plugins/Widgets.CustomTest/Views/CustomTest.cshtml");
    //}

    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        if (!(additionalData is ProductDetailsModel model))
            return Content("");
        //Read from the product service
        var productById = await _productService.GetProductByIdAsync(model.Id);
        //If the product exists we will log it
        if (productById != null)
        {
            var currentCustomer = await _workContext.GetCurrentCustomerAsync();
            //Setup the product to save
            var record = new ProductViewTrackerRecord
            {
                ProductId = model.Id,
                ProductName = productById.Name,
                CustomerId = currentCustomer.Id,
                IpAddress = currentCustomer.LastIpAddress,
                IsRegistered = true
            };
            //Map the values we're interested in to our new entity
            _productViewTrackerService.Log(record);
        }
        return Content("");
    }
}

