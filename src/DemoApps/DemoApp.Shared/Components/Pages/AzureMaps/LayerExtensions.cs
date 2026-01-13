using DemoApp.Shared.Models.AzureMaps;
using Marqdouj.DotNet.AzureMaps.Map.Common;
using Marqdouj.DotNet.AzureMaps.Map.Events;
using Marqdouj.DotNet.AzureMaps.Map.GeoJson;
using Marqdouj.DotNet.AzureMaps.Map.Interop;
using Marqdouj.DotNet.AzureMaps.Map.Layers;
using Marqdouj.DotNet.AzureMaps.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.UI.Models.Maps;
using Marqdouj.DotNet.Web.Components.Css;
using System.Collections.Generic;

namespace DemoApp.Shared.Components.Pages.AzureMaps
{
    internal static class LayerExtensions
    {
        public static List<IUIModelInputValue> GetInputs(this ILayerUIModel uiModel)
        {
            var inputs = uiModel.ToUIInputList();
            var mapType = uiModel.LayerDef.LayerType;

            switch (mapType)
            {
                case MapLayerType.Tile:
                    inputs.FirstOrDefault(e => e.Model.Name == nameof(TileLayerOptions.TileUrl))?.Model.ReadOnly = true;
                    break;
                default:
                    break;
            }

            return inputs;
        }

        public static async Task<List<MapLayerViewModel>> GetLayerViewModels(IMapDataService dataService)
        {
            var mapLayers = new List< MapLayerViewModel>();

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
                        zoomTo = (await dataService.GetLineLayerData())[0];
                        zoomLevel = 10;
                        break;
                    case MapLayerType.Polygon:
                        zoomTo = (await dataService.GetPolygonLayerData())[0][0];
                        break;
                    case MapLayerType.PolygonExtrusion:
                        zoomTo = (await dataService.GetPolygonExtLayerData())[0][0];
                        break;
                    case MapLayerType.Symbol:
                        zoomTo = (await dataService.GetSymbolLayerData())[0];
                        break;
                    case MapLayerType.Tile:
                        zoomTo = new Position(-122.426181, 47.608070);
                        zoomLevel = 10.75;
                        break;
                    default:
                        break;
                }
                var data = await dataService.GetBubbleLayerData();

                mapLayers.Add(new MapLayerViewModel(layerDef, zoomTo) { ZoomLevel = zoomLevel });
                ;
            }

            return mapLayers;
        }

        public static async Task<MapLayerDef> GetDefaultLayerDef(this MapLayerType layerType, IMapDataService dataService)
        {
            return layerType switch
            {
                MapLayerType.Bubble => new BubbleLayerDef(),
                MapLayerType.HeatMap => await GetDefaultHeatMapLayerDef(dataService),
                MapLayerType.Image => await GetDefaultImageLayerDef(dataService),
                MapLayerType.Line => new LineLayerDef()
                {
                    Before = "labels",
                    Options = new()
                    {
                        StrokeColor = HtmlColorName.Blue.ToString(),
                        StrokeWidth = 4,
                    }
                },
                MapLayerType.Polygon => new PolygonLayerDef()
                {
                    Options = new()
                    {
                        FillColor = HtmlColorName.Red.ToString(),
                        FillOpacity = 0.7,
                    }
                },
                MapLayerType.PolygonExtrusion => new PolygonExtLayerDef()
                {
                    Options = new()
                    {
                        FillColor = HtmlColorName.Red.ToString(),
                        FillOpacity = 0.7,
                        Height = 500,
                    }
                },
                MapLayerType.Symbol => new SymbolLayerDef() { Options = new() { IconOptions = new() { Image = IconImage.Pin_Red } } },
                MapLayerType.Tile => new TileLayerDef()
                {
                    Options = new()
                    {
                        Opacity = 0.8,
                        TileSize = 256,
                        MinSourceZoom = 7,
                        MaxSourceZoom = 17,
                        TileUrl = await dataService.GetTileLayerUrl()
                    },
                },
                _ => throw new ArgumentOutOfRangeException(nameof(layerType)),
            };
        }

        private static async Task<HeatMapLayerDef> GetDefaultHeatMapLayerDef(IMapDataService dataService)
        {
            var layerDef = new HeatMapLayerDef();
            layerDef.DataSource.Url = await dataService.GetHeatMapLayerUrl();
            return layerDef;
        }

        private static async Task<ImageLayerDef> GetDefaultImageLayerDef(IMapDataService dataService)
        {
            var layerDef = new ImageLayerDef();

            var data = await dataService.GetImageLayerData();
            layerDef.Options = new ImageLayerOptions
            {
                Url = data.Url,
                Coordinates = data.Coordinates,
            };

            return layerDef;
        }

        public static async Task<MapLayerDef> AddBasicMapLayer(this IAzureMapContainer mapInterop, IMapDataService dataService, MapLayerDef layerDef, IEnumerable<MapEventDef>? events = null, bool zoomTo = true)
        {
            return layerDef.LayerType switch
            {
                MapLayerType.Bubble => await AddBubbleLayer(mapInterop, dataService, (BubbleLayerDef)layerDef, zoomTo, events),
                MapLayerType.HeatMap => await AddHeatMapLayer(mapInterop, (HeatMapLayerDef)layerDef, zoomTo, events),
                MapLayerType.Image => await AddImageLayer(mapInterop, (ImageLayerDef)layerDef, zoomTo, events),
                MapLayerType.Line => await AddLineLayer(mapInterop, dataService, (LineLayerDef)layerDef, zoomTo, events),
                MapLayerType.Polygon => await AddPolygonLayer(mapInterop, dataService, (PolygonLayerDef)layerDef, zoomTo, events),
                MapLayerType.PolygonExtrusion => await AddPolygonExtLayer(mapInterop, dataService, (PolygonExtLayerDef)layerDef, zoomTo, events),
                MapLayerType.Symbol => await AddSymbolLayer(mapInterop, dataService, (SymbolLayerDef)layerDef, zoomTo, events),
                MapLayerType.Tile => await AddTileLayer(mapInterop, dataService, (TileLayerDef)layerDef, zoomTo, events),
                _ => throw new ArgumentOutOfRangeException(nameof(layerDef.LayerType)),
            };
        }

        private static async Task<MapLayerDef> AddBubbleLayer(IAzureMapContainer mapInterop, IMapDataService dataService, BubbleLayerDef layerDef, bool zoomTo = true, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetBubbleLayerData();
            MapFeatureDef featureDef = new(new MultiPoint(data))
            {
                Properties = new Properties
                    {
                        { "title", "my bubble layer" },
                        { "demo", true },
                    }
            };
            await mapInterop.Layers.AddMapFeature(featureDef, layerDef.DataSource.Id!);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0], 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddHeatMapLayer(IAzureMapContainer mapInterop, HeatMapLayerDef layerDef, bool zoomTo = true, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(new Position(-122.33, 47.6), 1);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddImageLayer(IAzureMapContainer mapInterop, ImageLayerDef layerDef, bool zoomTo = true, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(new Position(-74.172363, 40.735657), 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddLineLayer(IAzureMapContainer mapInterop, IMapDataService dataService, LineLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetLineLayerData();
            var feature = new MapFeatureDef(new LineString(data))
            {
                Properties = new Properties
                {
                    { "title", "my line" },
                    { "demo", true },
                }
            };

            await mapInterop.Layers.AddMapFeature(feature, layerDef.DataSource.Id!);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0], 10);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddPolygonLayer(IAzureMapContainer mapInterop, IMapDataService dataService, PolygonLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetPolygonLayerData();
            var feature = new MapFeatureDef(new Polygon(data))
            {
                Properties = new Properties
                {
                    { "title", "my Polygon layer" },
                    { "demo", true },
                },
                AsShape = true
            };

            await mapInterop.Layers.AddMapFeature(feature, layerDef.DataSource.Id!);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddPolygonExtLayer(IAzureMapContainer mapInterop, IMapDataService dataService, PolygonExtLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetPolygonExtLayerData();
            var feature = new MapFeatureDef(new Polygon(data))
            {
                Properties = new Properties
                {
                    { "title", "my PolygonExt layer" },
                    { "demo", true },
                },
                AsShape = true
            };

            await mapInterop.Layers.AddMapFeature(feature, layerDef.DataSource.Id!);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0][0], 11);

            var camera = await mapInterop.Configuration.GetCamera();
            camera.Pitch = 60;
            await mapInterop.Configuration.SetCamera(camera.ToCameraOptions());

            return layerDef;
        }

        private static async Task<MapLayerDef> AddSymbolLayer(IAzureMapContainer mapInterop, IMapDataService dataService, SymbolLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            var data = await dataService.GetSymbolLayerData();

            foreach (var position in data)
            {
                var feature = new MapFeatureDef(new Point(position))
                {
                    Properties = new Properties
                    {
                        { "title", "my symbol" },
                        { "description", "my symbol description" },
                        { "demo", true },
                    }
                };
                await mapInterop.Layers.AddMapFeature(feature, layerDef.DataSource.Id!);
            }

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(data[0], 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddTileLayer(IAzureMapContainer mapInterop, IMapDataService dataService, TileLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapInterop.Layers.CreateLayer(layerDef, events);

            if (zoomTo)
                await mapInterop.Configuration.ZoomTo(new Position(-122.426181, 47.608070), 10.75);

            return layerDef;
        }
    }
}
