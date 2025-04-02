using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;

namespace TaskCollabration.Models
{
    public class AuthModel
    {
        [Key]
        [Required(ErrorMessage = "Please Enter a Email")]

        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter a Password")]

        public string Password { get; set; }
        public bool RememberMe { get; set; }

        public string? FirstName { get; set; }
        public string? Image { get; set; }
    }
}
