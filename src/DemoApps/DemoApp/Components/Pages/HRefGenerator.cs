namespace DemoApp.Components.Pages
{
    internal enum HRefCodeSource
    {
        General,
        Components,
        ComponentsFluent,
    }

    internal static class HRefGenerator
    {
        public static string CodeUrl(this HRefCodeSource source, string path)
        {
            string? url;

            switch (source)
            {
                case HRefCodeSource.General:
                    url = "https://github.com/marqdouj/dotnet.general/blob/master/src/Marqdouj.DotNet.General/Marqdouj.DotNet.General";
                    break;
                case HRefCodeSource.Components:
                    url = "https://github.com/marqdouj/dotnet.web.components/blob/master/src/Marqdouj.DotNet.Web.Components/Marqdouj.DotNet.Web.Components";
                    break;
                case HRefCodeSource.ComponentsFluent:
                    url = "https://github.com/marqdouj/dotnet.web.components.fluentui/tree/master/src/FluentUI/Marqdouj.DotNet.Web.Components.FluentUI";
                    break;
                default:
                    throw new NotImplementedException();
            }

            return Path.Combine(url, path);
        }
    }
}
