using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Layers;

namespace DemoApp.Shared.Components.Pages.AzureMaps
{
    internal class MapLayerViewModel(MapLayerDef layerDef, Position? zoomTo)
    {
        public MapLayerDef LayerDef { get; } = layerDef;
        public Position? ZoomTo { get; } = zoomTo;
        public double ZoomLevel { get; set; } = 11;
        public bool IsLoaded { get; set; }
        public bool IsNotLoaded => IsLoaded == false;
    }
}
