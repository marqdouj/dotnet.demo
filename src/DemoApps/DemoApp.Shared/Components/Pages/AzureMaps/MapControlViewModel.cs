using Marqdouj.DotNet.AzureMaps.Map.Controls;

namespace DemoApp.Shared.Components.Pages.AzureMaps
{
    internal class MapControlViewModel(MapControl control)
    {
        public MapControl Control { get; set; } = control;
        public bool Loaded { get; set; }
        public bool IsNotLoaded => !Loaded;
    }
}
