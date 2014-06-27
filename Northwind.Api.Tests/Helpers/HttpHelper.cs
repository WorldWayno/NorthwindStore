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
        public static async Task<TokenResponse> PostFormData(HttpClient client, string url, Dictionary<string, string> formData)
        {
            var response = await client.PostAsync(string.Empty, new FormUrlEncodedContent(formData)).ConfigureAwait(false);

            var token = new TokenResponse(response.StatusCode,response.ReasonPhrase);
 
            if (response.IsSuccessStatusCode)
            {  
                token.Content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            return token;
        }

    }
}
