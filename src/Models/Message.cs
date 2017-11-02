using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ChatApi.Models
{
    public class Message
    {
        public string Id { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        [Required]
        public string SenderId { get; set; }

        public User SenderDetails { get; set; }

        [Required]
        [MaxLength(255)]
        public string Text { get; set; }

        public string GifUri { get; set; }
    }
}