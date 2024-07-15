﻿using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.TrendingProducts.Models;
public record ConfigurationModel : BaseNopModel
{
    [UIHint("DateTime")]
    public DateTime FromDate { get; set; }

    [UIHint("DateTime")]
    public DateTime ToDate { get; set; }

    [UIHint("Number")]
    public int Count { get; set; }

    [UIHint("Number")]
    public int SlidesToShow { get; set; } = 4;

    [UIHint("Number")]
    public int SlidesToScroll { get; set; } = 1;

    public bool AutoPlay { get; set; }

    [UIHint("Number")]
    public int AutoPlaySpeed { get; set; } = 2000;

    public int ActiveStoreScopeConfiguration { get; set; }
}
