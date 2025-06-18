using System.ComponentModel.DataAnnotations;

namespace Shopping_Cart.Models
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
