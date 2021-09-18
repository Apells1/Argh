using System.ComponentModel.DataAnnotations;

namespace PicNic.Models
{
    public class AspNetUserWithPassword
    {
        // public AspNetUsers AspNetUsers { get; set; }
        [UIHint("password"), Required]
        public string Password { get; set; }
    }
    public class LoginModel
    {
        [Required, UIHint("email")]
        public string Email { get; set; }

        [Required, UIHint("password")]
        public string Password { get; set; }
    }
}