namespace Marqdouj.DotNet.Demo.Components.Pages
{
    internal enum HRefCodeSource
    {
        General
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
                default:
                    throw new NotImplementedException();
            }

            return Path.Combine(url, path);
        }
     }
}
