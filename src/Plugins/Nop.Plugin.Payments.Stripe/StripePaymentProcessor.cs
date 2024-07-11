using System.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Infrastructure;
using Nop.Plugin.Payments.Stripe.Components;
using Nop.Plugin.Payments.Stripe.Enums;
using Nop.Plugin.Payments.Stripe.Extensions;
using Nop.Plugin.Payments.Stripe.Models;
using Nop.Plugin.Payments.Stripe.Validators;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Orders;
using Nop.Services.Payments;
using Nop.Services.Plugins;

namespace Nop.Plugin.Payments.Stripe
{
    public class StripePaymentProcessor : BasePlugin, IPaymentMethod
    {
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;
        private readonly StripePaymentSettings _stripePaymentSettings;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        public StripePaymentProcessor(ISettingService settingService,
                                      ILocalizationService localizationService,
                                      IWebHelper webHelper,
                                      StripePaymentSettings stripePaymentSettings,
                                      IOrderTotalCalculationService orderTotalCalculationService
        )
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
            _stripePaymentSettings = stripePaymentSettings;
            _orderTotalCalculationService = orderTotalCalculationService;
        }

        public bool SupportCapture => true;

        public bool SupportPartiallyRefund => true;

        public bool SupportRefund => true;

        public bool SupportVoid => true;

        public RecurringPaymentType RecurringPaymentType => RecurringPaymentType.NotSupported;

        public PaymentMethodType PaymentMethodType => PaymentMethodType.Standard;

        public bool SkipPaymentInfo => false;

        public Task<CancelRecurringPaymentResult> CancelRecurringPaymentAsync(CancelRecurringPaymentRequest cancelPaymentRequest)
        {
            return Task.FromResult(new CancelRecurringPaymentResult { Errors = new[] { "Recurring payment not supported" } });
        }

        public Task<bool> CanRePostProcessPaymentAsync(Order order)
        {
            if (order == null)
                throw new ArgumentNullException(nameof(order));

            //let's ensure that at least 5 seconds passed after order is placed
            //P.S. there's no any particular reason for that. we just do it
            if ((DateTime.UtcNow - order.CreatedOnUtc).TotalSeconds < 5)
                return Task.FromResult(false);

            return Task.FromResult(true);
        }

        public async Task<CapturePaymentResult> CaptureAsync(CapturePaymentRequest capturePaymentRequest)
        {
            if (capturePaymentRequest == null)
                throw new ArgumentNullException(nameof(CapturePaymentRequest));

            var capture = await capturePaymentRequest.CreateCapture(_stripePaymentSettings);

            if (capture.Status == "succeeded")
            {
                return new CapturePaymentResult
                {
                    NewPaymentStatus = PaymentStatus.Paid,
                    CaptureTransactionId = capture.Id,
                };
            }

            return new CapturePaymentResult
            {
                Errors = new List<string>(new[] { $"An error occured attempting to capture charge {capture.Id}." }),
                NewPaymentStatus = PaymentStatus.Authorized,
                CaptureTransactionId = capture.Id
            };
        }

        public async Task<decimal> GetAdditionalHandlingFeeAsync(IList<ShoppingCartItem> cart)
        {
            return await _orderTotalCalculationService.CalculatePaymentAdditionalFeeAsync(cart,
                _stripePaymentSettings.AdditionalFee, _stripePaymentSettings.AdditionalFeePercentage);
        }

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentStripe/Configure";
        }

        public Task<ProcessPaymentRequest> GetPaymentInfoAsync(IFormCollection form)
        {
            return Task.FromResult(new ProcessPaymentRequest
            {
                CreditCardName = form["CardholderName"],
                CreditCardNumber = form["CardNumber"],
                CreditCardExpireMonth = int.Parse(form["ExpireMonth"]),
                CreditCardExpireYear = int.Parse(form["ExpireYear"]),
                CreditCardCvv2 = form["CardCode"]
            });
        }

        public async Task<string> GetPaymentMethodDescriptionAsync()
        {
            return _stripePaymentSettings.Title;
        }

        public Type GetPublicViewComponent()
        {
            return typeof(PaymentStripeViewComponent);
        }

        public Task<bool> HidePaymentMethodAsync(IList<ShoppingCartItem> cart)
        {
            //var dataExistenceCheck = string.IsNullOrEmpty(_stripePaymentSettings.GetPublishableKey())
            //                        || string.IsNullOrEmpty(_stripePaymentSettings.GetSecretKey());

            //return Task.FromResult(dataExistenceCheck);
            return Task.FromResult(false);
        }

        public override async Task InstallAsync()
        {
            //settings
            await _settingService.SaveSettingAsync(new StripePaymentSettings
            {
                UseSandbox = true,
                Title = "Credit Card Stripe",
                AdditionalFee = 0,
                PaymentType = PaymentType.Authorize
            });

            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {
                ["Plugins.Payments.Stripe.Fields.AdditionalFee"] = "Additional fee",
                ["Plugins.Payments.Stripe.Fields.AdditionalFee.Hint"] = "Enter additional fee to charge your customers.",
                ["Plugins.Payments.Stripe.Fields.AdditionalFeePercentage"] = "Additional fee. Use percentage",
                ["Plugins.Payments.Stripe.Fields.AdditionalFeePercentage.Hint"] = "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.",
                ["Plugins.Payments.Stripe.Fields.UseSandbox"] = "Use Sandbox",
                ["Plugins.Payments.Stripe.Fields.UseSandbox.Hint"] = "Check to enable Sandbox (testing environment).",
                ["Plugins.Payments.Stripe.Fields.Title"] = "Title",
                ["Plugins.Payments.Stripe.Fields.Title.Hint"] = "Specify your title.",
                ["Plugins.Payments.Stripe.Fields.TestPublishableKey"] = "Test Publishable key",
                ["Plugins.Payments.Stripe.Fields.TestPublishableKey.Hint"] = "Specify your test publishable key.",
                ["Plugins.Payments.Stripe.Fields.TestSecretKey"] = "Test Secret key",
                ["Plugins.Payments.Stripe.Fields.TestSecretKey.Hint"] = "Specify your test secret key.",
                ["Plugins.Payments.Stripe.Fields.LivePublishableKey"] = "Live Publishable key",
                ["Plugins.Payments.Stripe.Fields.LivePublishableKey.Hint"] = "Specify your live publishable key.",
                ["Plugins.Payments.Stripe.Fields.LiveSecretKey"] = "Live Secret key",
                ["Plugins.Payments.Stripe.Fields.LiveSecretKey.Hint"] = "Specify your live Secret key.",
                ["Plugins.Payments.Stripe.Fields.PaymentType"] = "Payment type",
                ["Plugins.Payments.Stripe.Fields.PaymentType.Hint"] = "Specify your payment type.",

                ["Plugins.Payments.Stripe.Instructions"] = @"
                    <p>
                        For plugin configuration follow these steps:<br/>
                        <br/>
	                    <b>If you're using this gateway ensure that your primary store currency is supported by PayPal.</b>
	                    <br />
	                    <br />To use PDT, you must activate PDT and Auto Return in your PayPal account profile. You must also acquire a PDT identity token, which is used in all PDT communication you send to PayPal. Follow these steps to configure your account for PDT:<br />
	                    <br />1. Log in to your PayPal account (click <a href=""https://www.paypal.com/us/webapps/mpp/referral/paypal-business-account2?partner_id=9JJPJNNPQ7PZ8"" target=""_blank"">here</a> to create your account).
	                    <br />2. Click on the Profile button.
	                    <br />3. Click on the <b>Account Settings</b> link.
	                    <br />4. Select the <b>Website payments</b> item on left panel.
	                    <br />5. Find <b>Website Preferences</b> and click on the <b>Update</b> link.
	                    <br />6. Under <b>Auto Return</b> for <b>Website payments preferences</b>, select the <b>On</b> radio button.
	                    <br />7. For the <b>Return URL</b>, enter and save the URL on your site that will receive the transaction ID posted by PayPal after a customer payment (<em>{0}</em>).
                        <br />8. Under <b>Payment Data Transfer</b>, select the <b>On</b> radio button and get your <b>Identity token</b>.
	                    <br />9. Enter <b>Identity token</b> in the field below on the plugin configuration page.
                        <br />10. Click <b>Save</b> button on this page.
	                    <br />
                    </p>",

            });

            await base.InstallAsync();
        }

        public Task PostProcessPaymentAsync(PostProcessPaymentRequest postProcessPaymentRequest)
        {
            return Task.CompletedTask;
        }

        public async Task<ProcessPaymentResult> ProcessPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            var result = new ProcessPaymentResult()
            {
                NewPaymentStatus = _stripePaymentSettings.PaymentType
                                   == PaymentType.Authorize ? PaymentStatus.Authorized : PaymentStatus.Paid,

            };

            var currentStore = await EngineContext.Current.Resolve<IStoreContext>().GetCurrentStoreAsync();
            var paymentResponse = await processPaymentRequest.CompletePayment(_stripePaymentSettings);

            var transactionResult = $"Transaction was processed by using stripe, status is {paymentResponse.Status}";

            if (_stripePaymentSettings.PaymentType == PaymentType.Capture)
            {
                result.CaptureTransactionId = paymentResponse.Id;
                result.CaptureTransactionResult = transactionResult;
            }
            else if (_stripePaymentSettings.PaymentType == PaymentType.Authorize)
            {
                result.AuthorizationTransactionId = paymentResponse.Id;
                result.AuthorizationTransactionResult = transactionResult;
            }

            return result;
        }

        public Task<ProcessPaymentResult> ProcessRecurringPaymentAsync(ProcessPaymentRequest processPaymentRequest)
        {
            return Task.FromResult(new ProcessPaymentResult { Errors = new[] { "Recurring payment not supported" } });
        }

        public Task<RefundPaymentResult> RefundAsync(RefundPaymentRequest refundPaymentRequest)
        {
            return Task.FromResult(new RefundPaymentResult { Errors = new[] { "Refund method not supported" } });
        }

        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<StripePaymentSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Payments.Stripe");

            await base.UninstallAsync();
        }

        public Task<IList<string>> ValidatePaymentFormAsync(IFormCollection form)
        {
            var warnings = new List<string>();

            //validate
            var validator = new PaymentInfoValidator(_localizationService);
            var model = new PaymentInfoModel
            {
                CardholderName = form["CardholderName"],
                CardNumber = form["CardNumber"],
                CardCode = form["CardCode"],
                ExpireMonth = form["ExpireMonth"],
                ExpireYear = form["ExpireYear"]
            };
            var validationResult = validator.Validate(model);
            if (!validationResult.IsValid)
                warnings.AddRange(validationResult.Errors.Select(error => error.ErrorMessage));

            return Task.FromResult<IList<string>>(warnings);
        }

        public Task<VoidPaymentResult> VoidAsync(VoidPaymentRequest voidPaymentRequest)
        {
            return Task.FromResult(new VoidPaymentResult { Errors = new[] { "Void method not supported" } });
        }
    }
}
