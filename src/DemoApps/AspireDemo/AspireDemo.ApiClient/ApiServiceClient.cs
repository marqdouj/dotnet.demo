namespace AspireDemo.ApiClient
{
    public interface IApiServiceClient
    {
        INewsletterClient Newsletter { get; }
    }

    public class ApiServiceClient(HttpClient httpClient) : IApiServiceClient
    {
        public INewsletterClient Newsletter { get; } = new NewsletterClient(httpClient);
    }
}
