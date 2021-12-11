using ClassLibrary1.Common;
using ClassLibrary1.Domain.Entities;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ClassLibrary1.Services
{
    /// <summary>
    /// Helper class to call a protected API and process its result
    /// </summary>
    public class PrivateEndpointCallHelper
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="httpClient">HttpClient used to call the protected API</param>
        public PrivateEndpointCallHelper(HttpClient httpClient)
        {
            HttpClient = httpClient;
        }

        protected HttpClient HttpClient { get; private set; }

        /// <summary>
        /// Calls the protected web API and processes the result
        /// </summary>
        /// <param name="webApiUrl">URL of the web API to call (supposed to return Json)</param>
        /// <param name="accessToken">Access token used as a bearer security token to call the web API</param>
        /// <param name="processResult">Callback used to process the result of the call to the web API</param>
        public async Task<Packet> GetResultAsync(Guid id, string webApiUrl, string accessToken)
        {
            Tag.Where("GetResultAsync");

            Packet result = new();

            if (!string.IsNullOrEmpty(accessToken))
            {
                var defaultRequestHeaders = HttpClient.DefaultRequestHeaders;

                if (defaultRequestHeaders.Accept == null || !defaultRequestHeaders.Accept.Any(m => m.MediaType == "application/json"))
                {
                    HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                }

                defaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                Tag.Why("PreGetAsync");

                HttpResponseMessage response = await HttpClient.GetAsync(webApiUrl);

                Tag.Why("PostGetAsync");

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    result = JsonSerializer.Deserialize<Packet>(json);

                    Tag.What($"result={result}");
                }
                else
                {
                    string content = await response.Content.ReadAsStringAsync();
                }
            }

            return result;
        }
    }
}
