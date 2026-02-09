//using Microsoft.JSInterop;
// For Azure Maps Anonymous authentication
//using Microsoft.Identity.Client;

using Marqdouj.DotNet.AzureMaps.Blazor.Models;
using Marqdouj.DotNet.AzureMaps.Blazor.Models.Settings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DemoApp.Shared.Models.BlazorMaps
{
    public static class BzMapsBlazorSetup
    {
        private static MapConfiguration? mapConfiguration;
        //private static string clientSecret = "";
        //private static readonly string authorityFormat = "https://login.microsoftonline.com/{0}/oauth2/v2.0";
        //private static readonly string graphScope = "https://atlas.microsoft.com/.default";
        //private static string? sasToken; //Used only for demo purposes; do not do this in production.

        public static IServiceCollection ConfigureMarqdoujAzMapsBlazor(this IServiceCollection services, IConfiguration configuration)
        {
            //User Secrets for local development; Azure Key Vault for Production?:
            //"AzureMaps": {
            //    "AuthenticationType": "SubscriptionKey",
            //    "AadAppId": "",
            //    "AadTenant": "",
            //    "ClientId": "",
            //    "ClientSecret": "",
            //    "SubscriptionKey": "[YOUR KEY]"
            //  }

            mapConfiguration = services.AddMarqdoujAzureMapsBlazor(config =>
            {
                ConfigureForSubscriptionKey(configuration, config);
                //ConfigureForSasToken(configuration, config);
                //ConfigureForAad(configuration, config);
                //ConfigureForAnonymous(configuration, config);
            });

            return services;
        }

        private static void ConfigureForSubscriptionKey(IConfiguration configuration, MapConfiguration config)
        {
            config.Authentication.AuthType = AuthenticationType.subscriptionKey;
            config.Authentication.SubscriptionKey = configuration["AzureMaps:SubscriptionKey"];
        }

        //private static void ConfigureForSasToken(IConfiguration configuration, AzMapsConfiguration config)
        //{
        //    config.Authentication.AuthType = AuthenticationType.sas;

        //    //If provided, do do not need to configure GetSasToken callback in App.Razor
        //    //config.SasTokenUrl = "[YOUR SAS TOKEN URL]";

        //    //For demo only, do not do this in production.
        //    sasToken = configuration["AzureMaps:SasToken"];
        //}

        /// <summary>
        /// Only used for SasToken authentication.
        /// Requires token callback be configured in App.razor.
        /// </summary>
        /// <returns></returns>
        //[JSInvokable]
        //public static async Task<string?> GetSasToken()
        //{
        //    //TODO: Implement logic to generate SasToken.
        //    // For the purpose of testing, I manually generate SasToken (via Azure Maps Account/Shared Access Signature)
        //    // and add it to my User Secrets.
        //    return sasToken;
        //}

        //private static void ConfigureForAad(IConfiguration configuration, AzMapsConfiguration config)
        //{
        //    config.Authentication.AuthType = AuthenticationType.aad;
        //    config.Authentication.AadAppId = configuration["AzureMaps:AadAppId"];
        //    config.Authentication.AadTenant = configuration["AzureMaps:AadTenant"];
        //    config.Authentication.ClientId = configuration["AzureMaps:ClientId"];
        //}

        //private static void ConfigureForAnonymous(IConfiguration configuration, AzMapsConfiguration config)
        //{
        //    //NOTE: See GetAccessToken().
        //    config.Authentication.AuthType = AuthenticationType.anonymous;
        //    config.Authentication.AadAppId = configuration["AzureMaps:AadAppId"];
        //    config.Authentication.AadTenant = configuration["AzureMaps:AadTenant"];
        //    config.Authentication.ClientId = configuration["AzureMaps:ClientId"];
        //    clientSecret = configuration["AzureMaps:ClientSecret"] ?? "";
        //}

        /// <summary>
        /// Only used for Anonymous authentication.
        /// Requires token callback be configured in App.razor.
        /// </summary>
        /// <returns></returns>
        //[JSInvokable]
        //public static async Task<string> GetAccessToken()
        //{
        //    IConfidentialClientApplication daemonClient;
        //    daemonClient = ConfidentialClientApplicationBuilder.Create(mapConfiguration!.Authentication.AadAppId)
        //        .WithAuthority(string.Format(authorityFormat, mapConfiguration.Authentication.AadTenant))
        //        .WithClientSecret(clientSecret)
        //        .Build();
        //    AuthenticationResult authResult =
        //    await daemonClient.AcquireTokenForClient([graphScope]).ExecuteAsync();
        //    return authResult.AccessToken;
        //}
    }
}
