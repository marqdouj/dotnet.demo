namespace DemoApp.Shared.Models
{
    public enum HRefRepository
    {
        AzureMaps,
        AzureMapsUI, 
        Components,
        ComponentsFluentUI,
        Demo,
        General,
        MailDev,
        MailKit,
    }

    public static class LinkExtensions
    {
        private const string github = "https://github.com/marqdouj/";
        private const string githubContent = "https://raw.githubusercontent.com/marqdouj/";
        public const string Avatar = "https://avatars.githubusercontent.com/u/16510416";
        public const string LearnBlazor = "https://learn.microsoft.com/en-us/aspnet/core/blazor";
        public const string FluentBlazor = "https://www.fluentui-blazor.net/";
        public const string Markdown = "https://cdnjs.cloudflare.com/ajax/libs/github-markdown-css/5.8.1/github-markdown.min.css";
        public const string AtlasMinCss = "https://atlas.microsoft.com/sdk/javascript/mapcontrol/3/atlas.min.css";
        public const string AtlasMinJs = "https://atlas.microsoft.com/sdk/javascript/mapcontrol/3/atlas.min.js";
        public const string MarqdoujAzureMapsJs = "_content/Marqdouj.DotNet.AzureMaps/marqdouj-azuremaps.js";

        public static string Copyright => $"© Douglas Marquardt {DateTime.Today.Year}";

        public static string GitHub(this HRefRepository repository) => Path.Combine(github, repository.Name());

        private static string GitHubContent(this HRefRepository repository) => Path.Combine(githubContent, repository.Name());

        private static string GitHubBlobSrc(this HRefRepository repository) => Path.Combine(repository.GitHub(), "blob/master/src");

        private static string GitHubTreeSrc(this HRefRepository repository) => Path.Combine(repository.GitHub(), "tree/master/src");

        private static string GitHubRepositoryFolder(this HRefRepository repository)
        {
            var folder = repository switch
            {
                HRefRepository.Demo => "DemoApps",
                _ => "MainLib",
            };

            return folder;
        }

        public static string GitHubSrcFolder(this HRefRepository repository, string? projectName = null, string path = "")
        {
            var url = repository.GitHubTreeSrc();
            var folder = repository.GitHubRepositoryFolder();

            return Path.Combine(url, folder, projectName ?? repository.Name(true), path);
        }

        public static string GitHubSrcItem(this HRefRepository repository, string? projectName = null, string path = "")
        {
            var url = repository.GitHubBlobSrc();
            var folder = repository.GitHubRepositoryFolder();

            return Path.Combine(url, folder, projectName ?? repository.Name(true), path);
        }

        public static string ReadMe(this HRefRepository repository)
            => Path.Combine(repository.GitHubContent(), "master", "README.md");

        public static string ReadMe(this HRefRepository? repository)
            => repository != null ? repository.Value.ReadMe() : "";

        public static string Name(this HRefRepository repository, bool addPrefix = false)
        {
            var prefix = addPrefix ? "Marqdouj." : "";

            return repository switch
            {
                HRefRepository.AzureMaps => $"{prefix}DotNet.AzureMaps",
                HRefRepository.AzureMapsUI => $"{prefix}DotNet.AzureMaps.UI",
                HRefRepository.Components => $"{prefix}DotNet.Web.Components",
                HRefRepository.ComponentsFluentUI => $"{prefix}DotNet.Web.Components.FluentUI",
                HRefRepository.Demo => $"{prefix}DotNet.Demo",
                HRefRepository.General => $"{prefix}DotNet.General",
                HRefRepository.MailDev => "DotNet.Aspire.MailDev",
                HRefRepository.MailKit => "DotNet.Aspire.MailKit",
                _ => throw new ArgumentOutOfRangeException(nameof(repository)),
            };
        }
    }
}
