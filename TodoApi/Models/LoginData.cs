using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class LoginData
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}