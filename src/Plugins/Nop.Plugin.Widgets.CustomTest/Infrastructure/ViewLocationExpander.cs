using Microsoft.AspNetCore.Mvc.Razor;

namespace Nop.Plugin.Widgets.CustomTest.Infrastructure;

/// <summary>
/// Specifies the contracts for a view location expander that is used by Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine instances to determine search paths for a view.
/// </summary>
public class ViewLocationExpander : IViewLocationExpander
{
    private const string THEME_KEY = "CustomTheme";
    /// <summary>
    /// Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine the values that would be consumed by this instance
    /// of <see cref="T:Microsoft.AspNetCore.Mvc.Razor.IViewLocationExpander" />. The calculated values are used to determine if the view location
    /// has changed since the last time it was located.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" /> for the current view location
    /// expansion operation.</param>
    public void PopulateValues(ViewLocationExpanderContext context)
    {
    }

    /// <summary>
    /// Invoked by a <see cref="T:Microsoft.AspNetCore.Mvc.Razor.RazorViewEngine" /> to determine potential locations for a view.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Razor.ViewLocationExpanderContext" /> for the current view location
    /// expansion operation.</param>
    /// <param name="viewLocations">The sequence of view locations to expand.</param>
    /// <returns>A list of expanded view locations.</returns>
    public IEnumerable<string> ExpandViewLocations(ViewLocationExpanderContext context,
        IEnumerable<string> viewLocations)
    {
        if (context.AreaName == "Admin")
        {
            viewLocations = new[] {
                    $"/Plugins/Widgets.CustomTest/Areas/Admin/Views/Shared/{{0}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Areas/Admin/Views/{{1}}/{{0}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Areas/Admin/Views/{{0}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Views/Shared/Components/{{0}}/{{1}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Views/Shared/EditorTemplates/{{0}}.cshtml",
                }.Concat(viewLocations);
        }
        else
        {
            viewLocations = new[] {
                    $"/Plugins/Widgets.CustomTest/Views/Shared/{{0}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Views/Shared/Components/{{0}}/{{1}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Views/Shared/EditorTemplates/{{0}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Views/{{1}}/{{0}}.cshtml",
                    $"/Plugins/Widgets.CustomTest/Views/{{0}}.cshtml",
                }.Concat(viewLocations);

            if (context.Values.TryGetValue(THEME_KEY, out var theme))
                viewLocations = new[] {
                        $"/Plugins/Widgets.CustomTest/Themes/{theme}/Views/Shared/{{0}}.cshtml",
                        $"/Plugins/Widgets.CustomTest/Themes/{theme}/Views/{{1}}/{{0}}.cshtml"
                    }.Concat(viewLocations);

        }
        return viewLocations;
    }
}