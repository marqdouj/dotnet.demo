using DemoApp.Shared.Models.BlazorMaps;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Configuration;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Controls;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;

namespace DemoApp.Shared.Components.Pages.BlazorMaps
{
    internal static class MapHelpers
    {
        public static async Task<List<LayerDefViewModel>> GetLayerViewModels(IBzMapsDataService dataService)
        {
            var mapLayers = new List<LayerDefViewModel>();

            foreach (var layerType in Enum.GetValues<MapLayerType>().Where(e => e != MapLayerType.Unknown))
            {
                var layerDef = await layerType.GetDefaultLayerDef(dataService);
                Position? zoomTo = null;
                double zoomLevel = 11;

                switch (layerType)
                {
                    case MapLayerType.Bubble:
                        zoomTo = (await dataService.GetBubbleLayerData())[0];
                        break;
                    case MapLayerType.HeatMap:
                        zoomTo = new Position(-122.33, 47.6);
                        zoomLevel = 1;
                        break;
                    case MapLayerType.Image:
                        zoomTo = new Position(-74.172363, 40.735657);
                        break;
                    case MapLayerType.Line:
                        zoomTo = (await dataService.GetLineLayerData())[8];
                        zoomLevel = 11;
                        break;
                    case MapLayerType.Polygon:
                        zoomTo = (await dataService.GetPolygonLayerData())[0][0];
                        break;
                    case MapLayerType.PolygonExtrusion:
                        zoomTo = (await dataService.GetPolygonExtLayerData())[0][0];
                        break;
                    case MapLayerType.Symbol:
                        zoomTo = (await dataService.GetSymbolLayerData())[8];
                        break;
                    case MapLayerType.Tile:
                        zoomTo = new Position(-122.426181, 47.608070);
                        zoomLevel = 10.75;
                        break;
                    default:
                        break;
                }
                var data = await dataService.GetBubbleLayerData();

                mapLayers.Add(new LayerDefViewModel(layerDef, zoomTo) { ZoomLevel = zoomLevel });
            }

            return mapLayers;
        }

        public static MapOptions GetDefaultCreateMapOptions(Position? center = null, double zoomLevel = 10.5)
        {
            // Initialize map options with a specific camera position
            return new MapOptions
            {
                Camera = new CameraOptions
                {
                    Center = center ?? new Position(-122.33, 47.6), // (Seattle, WA)
                    Zoom = zoomLevel,
                },
                Style = new(),
            };
        }

        public static MapOptionsSet GetDefaultSetMapOptions(Position? center = null, double zoomLevel = 10.5)
        {
            // Initialize map options with a specific camera position and default other settings
            return new MapOptionsSet
            {
                Camera = new CameraOptionsSet
                {
                    Camera = new CameraOptions
                    {
                        Center = center ?? new Position(-122.33, 47.6), // (Seattle, WA)
                        Zoom = zoomLevel,
                        Pitch = 0,
                    },
                },
                Service = new(),
                Style = new(),
                UserInteraction = new()
            };
        }

        public static List<MapControl> GetDefaultControls()
        {
            var controls = new List<MapControl>()
            {
                 GetDefaultControl(MapControlType.Fullscreen),
                 GetDefaultControl(MapControlType.Zoom),
                 GetDefaultControl(MapControlType.Pitch),
                 GetDefaultControl(MapControlType.Compass),
                 GetDefaultControl(MapControlType.Style),
                 GetDefaultControl(MapControlType.Traffic),
                 GetDefaultControl(MapControlType.TrafficLegend),
                 GetDefaultControl(MapControlType.Scale)
            };

            //Set the ZOrder based on position in the list
            var zOrder = 0;
            foreach (var control in controls)
            {
                control.SortOrder = zOrder;
                zOrder++;
            }

            return controls;
        }

        public static MapControl GetDefaultControl(this MapControlType controlType)
        {
            return controlType switch
            {
                MapControlType.Compass => new CompassControl(MapControlPosition.Top_Right),
                MapControlType.Fullscreen => new FullscreenControl(MapControlPosition.Top_Right),
                MapControlType.Pitch => new PitchControl(MapControlPosition.Top_Right),
                MapControlType.Scale => new ScaleControl(MapControlPosition.Bottom_Right),
                MapControlType.Style => new StyleControl(MapControlPosition.Top_Right) { Options = GetStyleControlOptions() },
                MapControlType.Traffic => new TrafficControl(MapControlPosition.Top_Right),
                MapControlType.TrafficLegend => new TrafficLegendControl(MapControlPosition.Bottom_Left),
                MapControlType.Zoom => new ZoomControl(MapControlPosition.Top_Right),
                _ => throw new ArgumentOutOfRangeException(nameof(controlType)),
            };
        }

        private static StyleControlOptions GetStyleControlOptions()
        {
            List<MapStyle> styles = [.. Enum.GetValues<MapStyle>()];
            styles.Remove(MapStyle.Blank);
            styles.Remove(MapStyle.Blank_Accessible);
            return new StyleControlOptions { MapStyles = styles };
        }
    }
}
