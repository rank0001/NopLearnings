using DocumentFormat.OpenXml.Vml;
using Nop.Services.Payments;
using Stripe;

namespace Nop.Plugin.Payments.Stripe.Extensions;
public static class NopStripeExtension
{
    public static async Task<PaymentIntent> CompletePayment(this ProcessPaymentRequest processPaymentRequest, StripePaymentSettings stripePaymentSettings)
    {
        var paymentMethod = await GetPaymentMethodResponse(processPaymentRequest, stripePaymentSettings);
        var paymentIndent = await GetPaymentMethodResponse(paymentMethod, stripePaymentSettings, processPaymentRequest);
        return paymentIndent;
    }

    public static async Task<PaymentIntent> CreateCapture(this CapturePaymentRequest capturePaymentRequest, StripePaymentSettings stripePaymentSettings)
    {
        var service = new PaymentIntentService(stripePaymentSettings.GetStripeClient());
        var chargeId = capturePaymentRequest.Order.AuthorizationTransactionId;
        return await service.CaptureAsync(chargeId);
    }

    private static async Task<PaymentIntent> GetPaymentMethodResponse(PaymentMethod paymentMethod, StripePaymentSettings stripePaymentSettings, ProcessPaymentRequest processPaymentRequest)
    {
        var service = new PaymentIntentService(stripePaymentSettings.GetStripeClient());

        var options = new PaymentIntentCreateOptions
        {
            Amount = (long)(processPaymentRequest.OrderTotal * 100m),
            Currency = "usd",
            PaymentMethodTypes = new List<string>
            {
                "card"
            },
            PaymentMethod = paymentMethod.Id,
            ConfirmationMethod = "manual",
            CaptureMethod = stripePaymentSettings.PaymentType == Enums.PaymentType.Authorize ? "manual" : "automatic",
            Confirm = true
        };

        return await service.CreateAsync(options);
    }

    private static async Task<PaymentMethod> GetPaymentMethodResponse(ProcessPaymentRequest processPaymentRequest, StripePaymentSettings stripePaymentSettings)
    {
        var service = new PaymentMethodService(stripePaymentSettings.GetStripeClient());
        var options = new PaymentMethodCreateOptions
        {
            Type = "card",
            Card = new PaymentMethodCardOptions
            {
                Number = processPaymentRequest.CreditCardNumber,
                ExpMonth = processPaymentRequest.CreditCardExpireMonth,
                ExpYear = processPaymentRequest.CreditCardExpireYear,
                Cvc = processPaymentRequest.CreditCardCvv2,
            },
        };
        return await service.CreateAsync(options);
    }
}
