using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.Widgets.TopSellingProducts.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.TopSellingProducts.Validators;
public partial class ConfigurationModelValidator : BaseNopValidator<ConfigurationModel>
{
    public ConfigurationModelValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.FromDate)
            .Must((model, fromDate) => BeAValidDateRange(model, fromDate))
            .WithMessage("FromDate must be less than or equal to ToDate.");

        RuleFor(x => x.ToDate)
            .Must((model, toDate) => BeAValidDateRange(model, toDate))
            .WithMessage("ToDate must be greater than or equal to FromDate.");
    }

    private bool BeAValidDateRange(ConfigurationModel model, DateTime? date)
    {
        if (model.FromDate.HasValue && model.ToDate.HasValue)
        {
            return model.FromDate.Value <= model.ToDate.Value;
        }

        return true;
    }
}
