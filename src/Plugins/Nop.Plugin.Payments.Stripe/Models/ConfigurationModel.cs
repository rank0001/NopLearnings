
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Payments.Stripe.Models;
public record ConfigurationModel : BaseNopModel
{
    public int ActiveStoreScopeConfiguration { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.UseSandbox")]
    public bool UseSandbox { get; set; }
    public bool UseSandbox_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.Title")]
    public string Title { get; set; }
    public bool Title_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.TestPublishableKey")]
    public string TestPublishableKey { get; set; }
    public bool TestPublishableKey_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.TestSecretKey")]
    public string TestSecretKey { get; set; }
    public bool TestSecretKey_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.LivePublishableKey")]
    public string LivePublishableKey { get; set; }
    public bool LivePublishableKey_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.LiveSecretKey")]
    public string LiveSecretKey { get; set; }
    public bool LiveSecretKey_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.AdditionalFee")]
    public decimal AdditionalFee { get; set; }
    public bool AdditionalFee_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.AdditionalFeePercentage")]
    public bool AdditionalFeePercentage { get; set; }
    public bool AdditionalFeePercentage_OverrideForStore { get; set; }

    [NopResourceDisplayName("Plugins.Payments.Stripe.Fields.PaymentType")]
    public int PaymentTypeId { get; set; }
    public bool PaymentTypeId_OverrideForStore { get; set; }
    public IList<SelectListItem> PaymentTypes { get; set; } 
}
