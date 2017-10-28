using System.ComponentModel.DataAnnotations;

namespace ChatApi.Models
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [MaxLength(255)]
        public string FullName { get; set; }
    }
}