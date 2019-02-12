using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;
using glidedrose.Models;
using glidedrose.Common;
using Newtonsoft.Json;
using NUnit.Framework;
namespace glidedrose.Tests
{

    [TestFixture]
    public class ApiIntegrationTest
    {
        private HttpServer _server;
        private string _url = "http://localhost/";
        
        Item[] items = new Item[]
        {
            new Item { Id = 1, Name = "Zoom Vaporfly 4% Flyknit ", Description = "Lightweight road racing flat", Price = 250 },
            new Item { Id = 2, Name = "Zoom Fly Flyknit", Description = "Highly cushioned and super responsive lightweight running shoe", Price = 160 },
            new Item { Id = 3, Name = "Zoom Streak 6", Description = "Race-day weapon for fast half and full marathons", Price = 110 },
            new Item { Id = 4, Name = "Zoom Pegasus 35 Turbo", Description = "Lightweight running shoe with most responsive cushioning to date", Price = 180 }
        };

        [OneTimeSetUp]
        public void Init()
        {
            try
            {
                var config = new HttpConfiguration();

                config.MapHttpAttributeRoutes();

                config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
                config.MessageHandlers.Add(new TokenValidationHandler());

                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );

                _server = new HttpServer(config);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [Test]
        public void Test_GetAllItems()
        {
            var client = new HttpClient(_server);
            var request = createRequest("api/getall", "application/json", null, HttpMethod.Get);
            var expectedJson = JsonConvert.SerializeObject(items);

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                Assert.NotNull(response.Content);
                Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
                Assert.AreEqual(expectedJson, response.Content.ReadAsStringAsync().Result);
            }

            request.Dispose();
        }

        [Test]
        public void Test_BuyAnItemOrMore()
        {
            string username = "glidedrose";

            var client = new HttpClient(_server);
            AuthenticationHeaderValue auth = new AuthenticationHeaderValue("Bearer", TokenUtils.CreateToken(username));
            var request = createRequest("api/buyitem", "{\"id\":\"1\"}", auth, HttpMethod.Post);
            var expectedJson = JsonConvert.SerializeObject(items[0]);

            using (HttpResponseMessage response = client.SendAsync(request).Result)
            {
                if (response.Content != null)
                {
                    Assert.AreEqual("application/json", response.Content.Headers.ContentType.MediaType);
                    Assert.AreEqual(expectedJson, response.Content.ReadAsStringAsync().Result);
                }
                else
                    Assert.AreEqual(500, (int)response.StatusCode);
            }

            request.Dispose();
        }

        private HttpRequestMessage createRequest(string url, string content, AuthenticationHeaderValue auth, HttpMethod method)
        {
            var request = new HttpRequestMessage();

            request.RequestUri = new Uri(_url + url);
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            if (content != null && content != string.Empty)
            {
                HttpContent httpContent = new StringContent(content, Encoding.UTF8);
                request.Content = httpContent;
            }
            if (auth != null)
                request.Headers.Authorization = auth;

            request.Method = method;

            return request;
        }

        public void Dispose()
        {
            if (_server != null)
            {
                _server.Dispose();
            }
        }

        public class IDModel
        {
            public string id { get; set; }
        }

    }
}
