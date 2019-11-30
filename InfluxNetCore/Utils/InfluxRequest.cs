using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace InfluxNetCore.Utils
{
    internal class InfluxRequest
    {
        private static readonly HttpClient _httpClient = new HttpClient();


        public static async Task<string> MakeGetAsync(string requestUrl, params string[] urlParams)
        {
            var url = BuildUrl(requestUrl, urlParams);
            var res = await _httpClient.GetAsync(url);

            res.EnsureSuccessStatusCode();
            return await res.Content.ReadAsStringAsync();
        }

        public static async Task<TResult> MakeGetAsync<TResult>(string requestUrl, params string[] urlParams)
        {
            var url = BuildUrl(requestUrl, urlParams);
            
            var res = await _httpClient.GetAsync(url);

            res.EnsureSuccessStatusCode();

            return await GetObjectFromRes<TResult>(res);
        }


        public static async Task<HttpResponseMessage> MakePostAsync(string requestUrl, string inputData, params string[] urlParams)
        {
            var url = BuildUrl(requestUrl, urlParams);

            var content = new StringContent(inputData);

            return await _httpClient.PostAsync(url, content);
        }

        public static async Task<TResult> MakePostAsync<TResult>(string requestUrl, string inputData, params string[] urlParams)
        {
            var res = await MakePostAsync(requestUrl, inputData, urlParams);

            return await GetObjectFromRes<TResult>(res);
        }


        #region Help utils

        private static async Task<TResult> GetObjectFromRes<TResult>(HttpResponseMessage resMsg)
        {
            var contentStream = await resMsg.Content.ReadAsStreamAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            var content = await JsonSerializer.DeserializeAsync<TResult>(contentStream, options);
            return content;
        }

        private static string BuildUrl(string path, params string[] urlParams)
        {
            if (InfluxClient.Connection.Username != null && InfluxClient.Connection.Password != null)
            {
                Array.Resize(ref urlParams, urlParams.Length + 2);
                urlParams[urlParams.Length-2] = "u=" + InfluxClient.Connection.Username;
                urlParams[urlParams.Length-1] = "p=" + InfluxClient.Connection.Password;
            }

            var apiUrl = GetApiUrl();
            var result = apiUrl + path;
            var parSep = "?";
            foreach(var param in urlParams)
            {
                var parts = param.Split('=');
                if (parts.Length != 2)
                    continue;

                result += parSep + parts[0]+"="+HttpUtility.UrlEncode(parts[1]);
                parSep = "&";
            }
            return result;
        }

        private static string GetApiUrl()
        {
            var host = InfluxClient.Connection.Host;
            var port = InfluxClient.Connection.Port.ToString();

            return host + ":" + port + "/";
        }

        #endregion
    }
}
