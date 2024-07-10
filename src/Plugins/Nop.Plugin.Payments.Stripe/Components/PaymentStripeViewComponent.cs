using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Payments.Stripe.Models;
using Nop.Web.Framework.Components;

namespace Nop.Plugin.Payments.Stripe.Components;
public class PaymentStripeViewComponent : NopViewComponent
{
    public IViewComponentResult Invoke(string widgetZone, object additionalData)
    {
        var model = new PaymentInfoModel();

        //years
        for (var i = 0; i < 15; i++)
        {
            var year = (DateTime.Now.Year + i).ToString();
            model.ExpireYears.Add(new SelectListItem { Text = year, Value = year, });
        }

        //months
        for (var i = 1; i <= 12; i++)
        {
            model.ExpireMonths.Add(new SelectListItem { Text = i.ToString("D2"), Value = i.ToString(), });
        }
        return View("~/Plugins/Payments.Stripe/Views/PaymentInfo.cshtml", model);
    }
}
