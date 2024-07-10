using FluentValidation;
using Nop.Plugin.Payments.Stripe.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Payments.Stripe.Validators;
public partial class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
{
    public ConfigurationValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Title).NotEmpty()
                             .WithMessage("Title is required!");

        RuleFor(x => x.TestPublishableKey).NotEmpty()
                                          .WithMessage("Test Publishable Key is required!")
                                          .When(y => y.UseSandbox);

        RuleFor(x => x.TestSecretKey).NotEmpty()
                                     .WithMessage("Test Secret Key is required!")
                                     .When(y => y.UseSandbox);

        RuleFor(x => x.LivePublishableKey).NotEmpty()
                                  .WithMessage("Live Publishable Key is required!")
                                  .When(y => !y.UseSandbox);

        RuleFor(x => x.LiveSecretKey).NotEmpty()
                                     .WithMessage("Live Secret Key is required!")
                                     .When(y => !y.UseSandbox);

    }
}

