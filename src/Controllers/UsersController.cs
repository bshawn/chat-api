using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ChatApi.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private string connString = Environment.GetEnvironmentVariable("MONGODB_CONN_STR");

        // GET api/users
        [HttpGet]
        public IEnumerable<User> Get()
        {
            var client = new MongoClient(connString);
            var db = client.GetDatabase("chat-demo");
            var collection = db.GetCollection<User>("users");
            return collection.Find(new BsonDocument()).ToList();
        }

        // GET api/users/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/users
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/users/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/users/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
