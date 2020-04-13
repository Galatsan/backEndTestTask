using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using backEndTestTask.Exceptions;
using backEndTestTask.Interfaces;
using Newtonsoft.Json;

namespace backEndTestTask.Services
{
    public class Client : IClient
    {
        private const string MEDIA_TYPE = "application/json";

        public async Task<TRes> GetDataJsonAsync<TRes>(string url, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync(url);
            return await ParseContent<TRes>(response);
        }

        public async Task<TRes> PostDataAsJsonAsync<TRes>(string url, object data)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(url, new StringContent(GetStringValue(data), Encoding.Unicode, MEDIA_TYPE));
            return await ParseContent<TRes>(response);
        }

        private static string GetStringValue(object data) => JsonConvert.SerializeObject(data, Formatting.Indented);

        private async Task<T> ParseContent<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                try
                {
                    return JsonConvert.DeserializeObject<T>(content);
                }
                catch
                {
                    return default;
                }
            }
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedStatusCodeException();
            }
            throw new Exception("Client server error");
        }
    }
}
