using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CMCS.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        public string UserCode { get; set; } // login as User Name
        public string Username { get; set; } // User's real name
        public string Password { get; set; }

        public string Role { get; set; } // HR, Manager, Coordinator, Lecturer, User

        public string Email { get; set; }
        public string ContactNumber { get; set; }

        public ICollection<Claim> Claims { get; set; }
    }
}
