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
    public class MessagesController : Controller
    {
        // GET api/messages
        [HttpGet]
        public IActionResult Get(int limit)
        {
            var collection = CollectionManager.GetMessageCollection();
            var messages = collection.Find(m => true)
                .SortByDescending(m => m.Timestamp)
                .Limit(limit)
                .ToList();

            var usrCollection = CollectionManager.GetUserCollection();
            var users = usrCollection.Find(u => true)
                .ToList();

            var fullMsgs =
                from m in messages
                join u in users on m.SenderId equals u.Id
                select new Message
                {
                    Id = m.Id,
                    Text = m.Text,
                    Timestamp = m.Timestamp,
                    SenderDetails = u,
                    SenderId = m.SenderId
                };

            return Json(fullMsgs);
        }

        // GET api/messages/59f3d3c225eca06c14be4694
        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            if (!BsonValidator.IsValidObjectId(id))
                return NotFound("Message not found");

            var collection = CollectionManager.GetMessageCollection();
            var message = collection.Find(m => m.Id == id).FirstOrDefault();

            if (message == null)
                return NotFound("Message not found");

            var uCollection = CollectionManager.GetUserCollection();
            message.SenderDetails = uCollection.Find(u => u.Id == message.SenderId).FirstOrDefault();

            return Json(message);
        }

        [HttpGet]
        [Route("~/api/users/{userId}/sentmessages")]
        public IActionResult GetAllFromUser(string userId, int limit)
        {
            if (!BsonValidator.IsValidObjectId(userId))
                return NotFound("User not found");

            var uCollection = CollectionManager.GetUserCollection();
            var user = uCollection.Find(u => u.Id == userId).FirstOrDefault();
            if (user == null)
                return NotFound("User not found");

            var collection = CollectionManager.GetMessageCollection();
            var messages = collection.Find(m => m.SenderId == userId)
                .SortByDescending(m => m.Timestamp)
                .Limit(limit)
                .ToList()
                .Select(m => new Message
                {
                    Id = m.Id,
                    Text = m.Text,
                    Timestamp = m.Timestamp,
                    SenderDetails = user,
                    SenderId = m.SenderId
                });

            return Json(messages);
        }

        // POST api/messages
        [HttpPost]
        public IActionResult Post([FromBody]Message message)
        {
            message.Id = ObjectId.GenerateNewId().ToString();
            message.Timestamp = DateTime.Now;

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var collection = CollectionManager.GetMessageCollection();
            collection.InsertOne(message);
            return Get(message.Id);
        }

        // DELETE api/messages/59f3d3c225eca06c14be4694
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            if (!BsonValidator.IsValidObjectId(id))
                return NotFound("Message not found");

            var collection = CollectionManager.GetMessageCollection();

            var exists = collection.Find(m => m.Id == id).FirstOrDefault();
            if (exists == null)
                return NotFound("Message not found");

            collection.DeleteOne(m => m.Id == id);
            return Ok();
        }
    }
}
