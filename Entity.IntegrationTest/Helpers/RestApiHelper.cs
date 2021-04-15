using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Entity.IntegrationTest.Helpers
{
    public static class RestApiHelper
    {
        public static HttpClient GetHttpClient(string url = "", string token = "")
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new System.Uri(string.IsNullOrEmpty(url) ? ConfigurationHelper.ServerUrl : url);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            httpClient.Timeout = TimeSpan.FromSeconds(30);
            return httpClient;
        }
    }
}
