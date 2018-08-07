using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SSH.Core.Infrastructure
{
    public static class HttpClientExtension
    {
        public static async Task<HttpResponseMessage> DeleteAsJsonAsync(this HttpClient httpClient, string requestUri, StringContent data)
        {
            return await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Delete, requestUri) { Content = data });
        }

        private static HttpContent Serialize(object data)
        {
            return new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
        }
    }
}
