using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using glidedrose.Models;
using glidedrose.Common;
using System.Net.Http.Formatting;
using Newtonsoft.Json;

namespace glidedrose.Controllers
{
    [RoutePrefix("api")]
    public class ItemController : ApiController
    {
        Item[] items = new Item[]
        {
            new Item { Id = 1, Name = "Zoom Vaporfly 4% Flyknit ", Description = "Lightweight road racing flat", Price = 250 },
            new Item { Id = 2, Name = "Zoom Fly Flyknit", Description = "Highly cushioned and super responsive lightweight running shoe", Price = 160 },
            new Item { Id = 3, Name = "Zoom Streak 6", Description = "Race-day weapon for fast half and full marathons", Price = 110 },
            new Item { Id = 4, Name = "Zoom Pegasus 35 Turbo", Description = "Lightweight running shoe with most responsive cushioning to date", Price = 180 }
        };

        [HttpGet]
        [Route("getall")]
        public IEnumerable<Item> GetAllItems()
        {
            return items;
        }

        [Authorize]
        [HttpPost]
        [Route("buyitem")]
        public IHttpActionResult BuyItem([FromBody] IDModel frm)
        {
            //var myJson = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

            var item = items.FirstOrDefault((i) => i.Id.ToString() == frm.id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        [Route("login")]
        public IHttpActionResult Authenticate()
        {
            string token = TokenUtils.CreateToken("glidedrose");
            //return the token
            return Ok<string>(token);
        }

        public class IDModel
        {
            public string id { get; set; }
        }

    }
}
