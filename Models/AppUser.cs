using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PicNic.Models
{
    public class AppUser : IdentityUser
    {

    }

    public class UserLogin
    {
        [Required, EmailAddress]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CreateUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}