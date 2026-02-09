using DemoApp.Shared.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DemoApp.Shared.Components.Pages.BlazorMaps
{
    internal enum HRefBzMapsSource
    {
        AzureDocs,
        Examples,
    }

    internal static class Extensions
    {
        public static string ToBzMapsPageSource(this string name, string? subFolder = "") => HRefRepository.Demo.GitHubSrcItem("DemoApp.Shared", $"Components/Pages/BlazorMaps/{subFolder}{name}.razor");
        
        public static string ToBzMapsEventsPageSource(this string name) => HRefRepository.Demo.GitHubSrcItem("DemoApp.Shared", $"Components/Pages/BlazorMaps/Events/{name}.razor");

        public static string ToBzMapsCustomSource(this string name) => HRefRepository.Demo.GitHubSrcItem("DemoApp.Shared", $"Models/BlazorMaps/{name}");

        public static string BzMapsCodeUrl(this HRefBzMapsSource source, string path)
        {
            string? url = source switch
            {
                HRefBzMapsSource.AzureDocs => "https://learn.microsoft.com/en-us/azure/azure-maps",
                HRefBzMapsSource.Examples => "https://github.com/marqdouj/dotnet.azuremaps.blazor/blob/master/docs/examples/",
                _ => throw new NotImplementedException(),
            };
            return Path.Combine(url, path);
        }

        private static readonly JsonSerializerOptions jsonMinOptions = new()
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        internal static string ToJsonMin<T>(this T obj)
        {
            return JsonSerializer.Serialize(obj, jsonMinOptions);
        }
    }
}
