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
        // GET api/users
        [HttpGet]
        public IActionResult Get()
        {
            var collection = CollectionManager.GetUserCollection();
            var users = collection.Find(u => true).ToList();
            return Json(users);
        }

        // GET api/users/59f3d3c225eca06c14be4694
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!BsonValidator.IsValidObjectId(id))
                return NotFound("User not found");

            var collection = CollectionManager.GetUserCollection();
            var user = collection.Find(u => u.Id == id).FirstOrDefault();

            if (user == null)
                return NotFound("User not found");

            return Json(user);
        }

        // POST api/users
        [HttpPost]
        public IActionResult Post([FromBody]User user)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collection = CollectionManager.GetUserCollection();

            var exists = collection.Find(u => u.UserName == user.UserName).FirstOrDefault();
            if (exists != null)
                return StatusCode(409, $"A user with the user name {user.UserName} already exists");

            user.Id = ObjectId.GenerateNewId().ToString();
            collection.InsertOne(user);
            return Get(user.Id);
        }

        // PUT api/users/59f3d3c225eca06c14be4694
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody]User user)
        {
            if (!BsonValidator.IsValidObjectId(id))
                return NotFound("User not found");

            user.Id = id;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collection = CollectionManager.GetUserCollection();

            var idExists = collection.Find(u => u.Id == id).FirstOrDefault();
            if (idExists == null)
                return NotFound("User not found");

            var unExists = collection.Find(u => u.UserName == user.UserName && u.Id != id).FirstOrDefault();
            if (unExists != null)
                return StatusCode(409, $"A user with the user name {user.UserName} already exists");

            collection.ReplaceOne(u => u.Id == id, user);
            return Get(user.Id);
        }

        // DELETE api/users/59f3d3c225eca06c14be4694
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (!BsonValidator.IsValidObjectId(id))
                return NotFound("User not found");

            var uCollection = CollectionManager.GetUserCollection();

            var exists = uCollection.Find(u => u.Id == id).FirstOrDefault();
            if (exists == null)
                return NotFound("User not found");

            // Delete messages this user has sent.
            var mCollection = CollectionManager.GetMessageCollection();
            mCollection.DeleteMany(m => m.SenderId == id);


            uCollection.DeleteOne(u => u.Id == id);
            return Ok();
        }
    }
}
