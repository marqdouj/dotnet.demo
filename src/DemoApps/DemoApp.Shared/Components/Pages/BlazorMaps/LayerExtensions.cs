using DemoApp.Shared.Models.BlazorMaps;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Common;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Events;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.GeoJson;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Interop;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Layers;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Input;
using Marqdouj.DotNet.AzureMaps.Blazor.UI.Models.Layers;
using Marqdouj.DotNet.Web.Components.Css;

namespace DemoApp.Shared.Components.Pages.BlazorMaps
{
    internal static class LayerExtensions
    {
        public static string? ToHRefBzMapsAddLayerExample(this MapLayerDef? layerDef) => layerDef?.LayerType.ToHRefBzMapsAddLayerExample();

        public static string ToHRefBzMapsAddLayerExample(this MapLayerType layerType)
        {
            var hRef = HRefBzMapsSource.Examples.BzMapsCodeUrl($"add{layerType}Layer.md");
            return hRef;
        }

        public static string? ToHRefBzMapsAzureDocs(this MapLayerDef? layerDef) => layerDef?.LayerType.ToHRefBzMapsAzureDocs();

        public static string ToHRefBzMapsAzureDocs(this MapLayerType layerType)
        {
            string codePath = layerType switch
            {
                MapLayerType.Bubble => "map-add-bubble-layer",
                MapLayerType.HeatMap => "map-add-heat-map-layer",
                MapLayerType.Image => "map-add-image-layer",
                MapLayerType.Line => "map-add-line-layer",
                MapLayerType.Polygon => "map-add-shape",
                MapLayerType.PolygonExtrusion => "map-extruded-polygon",
                MapLayerType.Symbol => "map-add-pin",
                MapLayerType.Tile => "map-add-tile-layer",
                _ => throw new NotImplementedException(),
            };

            var hRef = HRefBzMapsSource.AzureDocs.BzMapsCodeUrl(codePath);
            return hRef;
        }

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

        public static List<MapFeatureDef> GetDefaultSymbolLayerFeatures(this List<Position> data)
        {
            var results = new List<MapFeatureDef<Point>>();
            var counter = 0;

            foreach (var position in data)
            {
                counter++;

                var feature = new MapFeatureDef<Point>(new Point(position))
                {
                    Properties = new Properties
                    {
                        { "title", $"my symbol #{counter}" },
                        { "description", $"my symbol #{counter} description" },
                        { "demo", true },
                    },
                    Id = $"demoSymbol-{counter}"
                };

                results.Add(feature);
            }

            return [.. results.Cast<MapFeatureDef>()];
        }

        public static async Task<List<MapFeatureDef>> GetDefaultSymbolLayerFeatures(this IBzMapsDataService dataService)
        {
            var results = new List<MapFeatureDef>();
            var data = await dataService.GetSymbolLayerData();
            var counter = 0;

            foreach (var position in data)
            {
                counter++;

                var feature = new MapFeatureDef(new Point(position))
                {
                    Properties = new Properties
                    {
                        { "title", $"my symbol #{counter}" },
                        { "description", $"my symbol #{counter} description" },
                        { "demo", true },
                    },
                    Id = $"demoSymbol-{counter}"
                };

                results.Add(feature);
            }

            return results;
        }

        public static MapLayerDef GetDefaultLayerDef(this MapLayerType layerType)
        {
            return layerType switch
            {
                MapLayerType.Bubble => new BubbleLayerDef(),
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
                _ => throw new ArgumentOutOfRangeException(nameof(layerType)),
            };
        }

        public static async Task<MapLayerDef> GetDefaultLayerDef(this MapLayerType layerType, IBzMapsDataService dataService)
        {
            return layerType switch
            {
                MapLayerType.HeatMap => await GetDefaultHeatMapLayerDef(dataService),
                MapLayerType.Image => await GetDefaultImageLayerDef(dataService),
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
                _ => layerType.GetDefaultLayerDef(),
            };
        }

        private static async Task<HeatMapLayerDef> GetDefaultHeatMapLayerDef(IBzMapsDataService dataService)
        {
            var layerDef = new HeatMapLayerDef();
            layerDef.DataSource.Url = await dataService.GetHeatMapLayerUrl();
            return layerDef;
        }

        private static async Task<ImageLayerDef> GetDefaultImageLayerDef(IBzMapsDataService dataService)
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

        public static async Task<MapLayerDef> AddBasicMapLayer(this IAzMapInterop mapsInterop, IBzMapsDataService dataService, MapLayerType layerType, IEnumerable<MapEventDef>? events = null, bool zoomTo = true)
        {
            return await mapsInterop.AddBasicMapLayer(dataService, await layerType.GetDefaultLayerDef(dataService), events, zoomTo);
        }

        public static async Task<MapLayerDef> AddBasicMapLayer(this MapLayerDef layerDef, IAzMapInterop mapsInterop, IBzMapsDataService dataService, IEnumerable<MapEventDef>? events = null, bool zoomTo = true)
        {
            return await mapsInterop.AddBasicMapLayer(dataService, layerDef, events, zoomTo);
        }

        public static async Task<MapLayerDef> AddBasicMapLayer(this IAzMapInterop mapsInterop, IBzMapsDataService dataService, MapLayerDef layerDef, IEnumerable<MapEventDef>? events = null, bool zoomTo = true)
        {
            return layerDef.LayerType switch
            {
                MapLayerType.Bubble => await AddBubbleLayer(mapsInterop, dataService, (BubbleLayerDef)layerDef, zoomTo, events),
                MapLayerType.HeatMap => await AddHeatMapLayer(mapsInterop, (HeatMapLayerDef)layerDef, zoomTo, events),
                MapLayerType.Image => await AddImageLayer(mapsInterop, (ImageLayerDef)layerDef, zoomTo, events),
                MapLayerType.Line => await AddLineLayer(mapsInterop, dataService, (LineLayerDef)layerDef, zoomTo, events),
                MapLayerType.Polygon => await AddPolygonLayer(mapsInterop, dataService, (PolygonLayerDef)layerDef, zoomTo, events),
                MapLayerType.PolygonExtrusion => await AddPolygonExtLayer(mapsInterop, dataService, (PolygonExtLayerDef)layerDef, zoomTo, events),
                MapLayerType.Symbol => await AddSymbolLayer(mapsInterop, dataService, (SymbolLayerDef)layerDef, zoomTo, events),
                MapLayerType.Tile => await AddTileLayer(mapsInterop, dataService, (TileLayerDef)layerDef, zoomTo, events),
                _ => throw new ArgumentOutOfRangeException(nameof(layerDef.LayerType)),
            };
        }

        private static async Task<MapLayerDef> AddBubbleLayer(IAzMapInterop mapsInterop, IBzMapsDataService dataService, BubbleLayerDef layerDef, bool zoomTo = true, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add(layerDef, events);

            var data = await dataService.GetBubbleLayerData();
            MapFeatureDef featureDef = new(new MultiPoint(data))
            {
                Properties = new Properties
                    {
                        { "title", "my bubble layer" },
                        { "demo", true },
                    }
            };
            await mapsInterop.Features.Add([featureDef], layerDef.DataSource.Id!);

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(data[0], 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddHeatMapLayer(IAzMapInterop mapsInterop, HeatMapLayerDef layerDef, bool zoomTo = true, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add([layerDef], events);

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(new Position(-122.33, 47.6), 1);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddImageLayer(IAzMapInterop mapsInterop, ImageLayerDef layerDef, bool zoomTo = true, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add([layerDef], events);

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(new Position(-74.172363, 40.735657), 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddLineLayer(IAzMapInterop mapsInterop, IBzMapsDataService dataService, LineLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add([layerDef], events);

            var data = await dataService.GetLineLayerData();
            var feature = data.GetLineLayerFeatureDef();

            await mapsInterop.Features.Add(feature, layerDef.DataSource.Id!);

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(data[8], 11);

            return layerDef;
        }

        public static MapFeatureDef<LineString> GetLineLayerFeatureDef(this List<Position> data)
        {
            return new MapFeatureDef<LineString>(new LineString(data))
            {
                Properties = new Properties
                {
                    { "title", "my line" },
                    { "demo", true },
                }
            };
        }

        private static async Task<MapLayerDef> AddPolygonLayer(IAzMapInterop mapsInterop, IBzMapsDataService dataService, PolygonLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add([layerDef], events);

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

            await mapsInterop.Features.Add(feature, layerDef.DataSource.Id!);

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(data[0][0], 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddPolygonExtLayer(IAzMapInterop mapsInterop, IBzMapsDataService dataService, PolygonExtLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add([layerDef], events);

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

            await mapsInterop.Features.Add(feature, layerDef.DataSource.Id!);

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(data[0][0], 11);

            var camera = await mapsInterop.Configurations.GetCamera();
            camera.Pitch = 60;
            await mapsInterop.Configurations.SetCamera(camera.ToCameraOptions());

            return layerDef;
        }

        private static async Task<MapLayerDef> AddSymbolLayer(IAzMapInterop mapsInterop, IBzMapsDataService dataService, SymbolLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add([layerDef], events);

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
                await mapsInterop.Features.Add(feature, layerDef.DataSource.Id!);
            }

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(data[8], 11);

            return layerDef;
        }

        private static async Task<MapLayerDef> AddTileLayer(IAzMapInterop mapsInterop, IBzMapsDataService dataService, TileLayerDef layerDef, bool zoomTo, IEnumerable<MapEventDef>? events = null)
        {
            await mapsInterop.Layers.Add([layerDef], events);

            if (zoomTo)
                await mapsInterop.Configurations.ZoomTo(new Position(-122.426181, 47.608070), 10.75);

            return layerDef;
        }

    }
}

