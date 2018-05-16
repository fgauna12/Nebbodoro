using System.ComponentModel.DataAnnotations;

namespace Nebbodoro.API.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}