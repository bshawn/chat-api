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
        public IActionResult Get()
        {
            var collection = GetUserCollection();
            var users = collection.Find(u => true).ToList();
            return Json(users);
        }

        // GET api/users/59f3d3c225eca06c14be4694
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var collection = GetUserCollection();
            var user = collection.Find(u => u.Id == id).FirstOrDefault();

            if (user == null)
                return NotFound();

            return Json(user);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collection = GetUserCollection();
            user.Id = ObjectId.GenerateNewId().ToString();
            collection.InsertOne(user);
            return Get(user.Id);
        }

        // PUT api/users/59f3d3c225eca06c14be4694
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collection = GetUserCollection();
            user.Id = id;
            collection.ReplaceOne(u => u.Id == id, user);
            return Get(user.Id);
        }

        // DELETE api/users/59f3d3c225eca06c14be4694
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var collection = GetUserCollection();
            collection.DeleteOne(u => u.Id == id);
            return Ok();
        }

        private IMongoCollection<User> GetUserCollection()
        {
            var client = new MongoClient(connString);
            var db = client.GetDatabase("chat-demo");
            var collection = db.GetCollection<User>("users");
            return collection;
        }
    }
}
