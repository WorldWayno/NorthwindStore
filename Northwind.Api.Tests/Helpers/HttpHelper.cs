using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Northwind.Api.Tests.Helpers
{
    public class HttpHelper
    {
        public static async Task<TokenResponse> GetTokenAsync(HttpClient client, string username, string password)
        {

            var formData = new Dictionary<string, string>
            {
                {"username", username},
                {"password", password},
                {"grant_type", "password"}
            };

            var response = await client.PostAsJsonAsync("token", new FormUrlEncodedContent(formData));


            if (response.IsSuccessStatusCode)
            {  
               var result = response.Content.ReadAsStringAsync().Result;
            }

            return null;
        }

    }
}
