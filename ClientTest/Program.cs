using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ClientTest
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }

    public class Program
    {
        static HttpClient client = new HttpClient();

        static async Task<Item> GetItemAsync(string path)
        {
            Item item = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                item = await response.Content.ReadAsAsync<Item>();
            }
            return item;
        }

        static async Task<Item[]> GetItemsAsync(string path)
        {
            Item[] items = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                items = await response.Content.ReadAsAsync<Item[]>();
            }
            return items;
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("http://localhost:58628/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Item item = await GetItemAsync("api/buyitem?id=2");

            Console.WriteLine(item.Name);

            Item[] items = await GetItemsAsync("api/getall");

            Console.WriteLine(items[0].Name);

            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            RunAsync().GetAwaiter().GetResult();
        }
    }
}
