using MudBlazor;

namespace EmirCoaching.Admin.Layout;

// Sitenin renkleri ve fontları (EmirCoaching.Web/wwwroot/style.css → :root)
public static class AdminTheme
{
    public const string Navy = "#000636";
    public const string Accent = "#2b7eff";

    private static readonly string[] Inter = ["Inter", "sans-serif"];
    private static readonly string[] Geologica = ["Geologica", "sans-serif"];

    public static readonly MudTheme Theme = new()
    {
        PaletteLight = new PaletteLight
        {
            Primary = Accent,
            Secondary = Navy,
            AppbarBackground = "#ffffff",
            AppbarText = Navy,
            DrawerBackground = Navy,
            DrawerText = "rgba(255,255,255,0.8)",
            DrawerIcon = "rgba(255,255,255,0.8)",
            Background = "#f5f6fa",
            TextPrimary = Navy
        },
        Typography = new Typography
        {
            Default = new DefaultTypography { FontFamily = Inter },
            H4 = new H4Typography { FontFamily = Geologica, FontWeight = "600" },
            H5 = new H5Typography { FontFamily = Geologica, FontWeight = "600" },
            H6 = new H6Typography { FontFamily = Geologica, FontWeight = "600" },
            Button = new ButtonTypography { FontFamily = Inter, FontWeight = "500", TextTransform = "none" }
        },
        LayoutProperties = new LayoutProperties { DrawerWidthLeft = "260px" }
    };
}
