using DemoApp.Shared.Models;
using Marqdouj.DotNet.AzureMaps.Map.Layers;

namespace DemoApp.Shared.Components.Pages.AzureMaps
{
    internal enum HRefSource
    {
        AzureDocs,
        Examples,
    }

    internal static class MapExtensions
    {
        public static string? ToHRefAddLayerExample(this MapLayerDef? layerDef) => layerDef?.LayerType.ToHRefAddLayerExample();

        public static string ToHRefAddLayerExample(this MapLayerType layerType)
        {
            var hRef = HRefSource.Examples.CodeUrl($"add{layerType}Layer.md");
            return hRef;
        }

        public static string? ToHRefAzureDocs(this MapLayerDef? layerDef) => layerDef?.LayerType.ToHRefAzureDocs();

        public static string ToHRefAzureDocs(this MapLayerType layerType)
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

            var hRef = HRefSource.AzureDocs.CodeUrl(codePath);
            return hRef;
        }

        public static string ToMapPageSource(this string name, string? subFolder ="") => HRefRepository.Demo.GitHubSrcItem("DemoApp.Shared", $"Components/Pages/AzureMaps/{subFolder}{name}.razor");
        public static string ToMapEventsPageSource(this string name) => HRefRepository.Demo.GitHubSrcItem("DemoApp.Shared", $"Components/Pages/AzureMaps/Events/{name}.razor");
        public static string ToMapCustomSource(this string name) => HRefRepository.Demo.GitHubSrcItem("DemoApp.CustomMaps", name);

        public static string CodeUrl(this HRefSource source, string path)
        {
            string? url = source switch
            {
                HRefSource.AzureDocs => "https://learn.microsoft.com/en-us/azure/azure-maps",
                HRefSource.Examples => "https://github.com/marqdouj/dotnet.azuremaps/blob/master/docs/examples/",
                _ => throw new NotImplementedException(),
            };
            return Path.Combine(url, path);
        }

    }
}
