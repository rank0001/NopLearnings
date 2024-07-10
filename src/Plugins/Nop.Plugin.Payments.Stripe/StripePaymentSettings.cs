using Nop.Core.Configuration;
using Nop.Plugin.Payments.Stripe.Enums;

namespace Nop.Plugin.Payments.Stripe;

public class StripePaymentSettings : ISettings
{
    public bool UseSandbox { get; set; }
    public string Title { get; set; }
    public string TestPublishableKey { get; set; }
    public string TestSecretKey { get; set; }
    public string LivePublishableKey { get; set; }
    public string LiveSecretKey { get; set; }
    public decimal AdditionalFee { get; set; }
    public bool AdditionalFeePercentage { get; set; }
    public PaymentType PaymentType { get; set; }
    
    public string GetSecretKey()
    {
        return UseSandbox ? TestSecretKey : LiveSecretKey;
    }
    public string GetPublishableKey()
    {
        return UseSandbox ? TestPublishableKey : LivePublishableKey;
    }
}
