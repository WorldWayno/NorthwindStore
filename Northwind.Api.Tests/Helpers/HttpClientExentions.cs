using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Northwind.Api.Tests.Helpers;

namespace Northwind.Api.Tests
{
    public static class HttpClientExtensions
    {
        public static void SetBasicAuthentication(this HttpClient client, string username, string password)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(username + ":" + password);
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public static void SetToken(this HttpClient client, string scheme, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        }

        public static void SetBearerToken(this HttpClient client, string token)
        {
            client.SetToken("Bearer", token);
        }

        public static async Task<HttpResponseMessage> PostFormData(this HttpClient client, string url, Dictionary<string, string> formData)
        {
            return await client.PostAsync(url, new FormUrlEncodedContent(formData));
        }
    }
}