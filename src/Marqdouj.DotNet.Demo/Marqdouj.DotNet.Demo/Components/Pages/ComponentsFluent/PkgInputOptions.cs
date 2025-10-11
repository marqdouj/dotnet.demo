using Marqdouj.DotNet.Web.Components.Css;
using Marqdouj.DotNet.Web.Components.UI;

namespace Marqdouj.DotNet.Demo.Components.Pages.ComponentsFluent
{
    public enum PkgInputAlignment
    {
        TopLeft, TopRight, Left, Right, Top, BottomLeft, BottomRight,
    }

    public enum PkgInputPosition
    {
        Top, Left, Bottom, Right,
    }

    public class PkgInputOptions
    {
        public PkgInputAlignment? Alignment { get; set; }
        public string? Color { get; set; } = HtmlColorName.DarkBlue.ToString();
        public bool Hidden { get; set; }
        public string? Description { get; set; }
        public double? Opacity { get; set; } = 1;
        public PkgInputPosition Position { get; set; }
        public double Degrees { get; set; } 
        public bool? Visible { get; set; }
    }

    public class PkgInputOptionsUI : UIModel<PkgInputOptions>
    {
        public PkgInputOptionsUI()
        {
            Opacity.SetBindMinMax(0, 1);
            Degrees.SetBindMinMax(0, 360);

            foreach (var item in Items)
            {
                var optional = item.IsNullableValueType ? " (Optional)" : "";
                item.Description = $"This is the description for the '{item.NameDisplay}'{optional}.";
            }
        }

        public IUIModelValue Alignment => GetItem(nameof(PkgInputOptions.Alignment))!;
        public IUIModelValue Color => GetItem(nameof(PkgInputOptions.Color))!;
        public IUIModelValue Hidden => GetItem(nameof(PkgInputOptions.Hidden))!;
        public IUIModelValue Description => GetItem(nameof(PkgInputOptions.Description))!;
        public IUIModelValue Opacity => GetItem(nameof(PkgInputOptions.Opacity))!;
        public IUIModelValue Position => GetItem(nameof(PkgInputOptions.Position))!;
        public IUIModelValue Degrees => GetItem(nameof(PkgInputOptions.Degrees))!;
        public IUIModelValue Visible => GetItem(nameof(PkgInputOptions.Visible))!;
    }
}
