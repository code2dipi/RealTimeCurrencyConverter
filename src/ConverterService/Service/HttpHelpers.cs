using System.Net;
using Newtonsoft.Json;

namespace ConverterService.Service
{
    public class HttpHelpers : IHttpHelpers
    {
        private readonly HttpClient _client;

        public HttpHelpers(HttpClient client)
        {
            this._client = client;
        }

        public async Task<TReturn?> Get<TReturn>(string url)
            where TReturn : class
        {
            using var requestMessage = new HttpRequestMessage();
            requestMessage.Method = HttpMethod.Get;
            requestMessage.RequestUri = GetUri(url);

            using var responseMessage = await _client.SendAsync(requestMessage, CancellationToken.None).ConfigureAwait(false);

            if (responseMessage.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ArgumentNullException($"Not Found");
            }

            var response = await responseMessage.Content.ReadAsStringAsync();
            var deserializeValue = JsonConvert.DeserializeObject<TReturn>(response);

            if (deserializeValue is null)
            {
                throw new ArgumentNullException($"No data found");
            }

            return deserializeValue;
        }

        private Uri GetUri(string relativeUri)
        {
            var baseUri = _client?.BaseAddress?.ToString().TrimEnd('/');
            relativeUri = relativeUri.TrimEnd('/');
            return new Uri($"{baseUri}/{relativeUri}");
        }
    }
}
