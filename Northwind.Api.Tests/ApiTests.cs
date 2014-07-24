using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Northwind.Api.Models;
using Northwind.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Northwind.Api.Tests
{
    public class OrdersApiTests : TestBase
    {
        [Test]
        public void Http_Get_Orders()
        {
            var uri = "http://localhost/NortwindApi/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.SetBasicAuthentication("wlewalski@comcast.net", "Wayne!");

                HttpResponseMessage response = client.GetAsync("api/orders").Result;

                Assert.IsTrue(response.IsSuccessStatusCode);

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.IsTrue(!String.IsNullOrEmpty(content));

                var orders = JsonConvert.DeserializeObject<IEnumerable<Order>>(content);

                Assert.IsTrue(orders.Any());
            }
        }

        [Test]
        public void Http_Post_Order()
        {
            var uri = "http://localhost/NortwindApi/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.SetBasicAuthentication("wlewalski@comcast.net", "Wayne!");

                var model = new OrderModel()
                {
                    CustomerID = "VINET",
                    ShipCity = "Boston",
                    IsShipped = true
                };

                HttpResponseMessage response = client.PostAsJsonAsync("api/orders", model).Result;

                Assert.IsTrue(response.IsSuccessStatusCode);

                var content = response.Content.ReadAsStringAsync().Result;

                Assert.IsTrue(!String.IsNullOrEmpty(content));

                var order = JsonConvert.DeserializeObject<Order>(content);

                Assert.IsNotNull(order);
            }
        }

        [Test]
        public void Http_Post_Token()
        {
            var uri = "http://localhost/NortwindApi/";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(uri);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var token = client.GetTokenAsync("wlewalski@comcast.net", "Wayne!");

                var formValues = new Dictionary<string, string>
                {
                    {"username", "wlewalski@comcast.net"},
                    {"password", "Wayne!"},
                    {"grant_type", "password"}
                };

                var response = client.PostFormData("token", formValues).Result;

                //Assert.IsTrue(response.IsSuccessStatusCode);

                //var content = response.Content.ReadAsStringAsync().Result;

                //Assert.IsTrue(!String.IsNullOrEmpty(content));

                //var token = JsonConvert.DeserializeObject<TokenResponse>(content);

                Assert.IsNotNull(token);
            }
        }

        [Test]
        public void Post_Json_to_Users()
        {

            var json = JObject.FromObject(
                new
                {
                    swaggerVersion = "1.2",
                    apiVersion = "1.0",
                    apis = new object[]
                    {
                        new { path = "/deletes" },
                        new { path = "/gets" },
                        new { path = "/posts" },
                        new { path = "/puts" },
                    }
                });

            Assert.IsNotNull(json);
        }
    }
}