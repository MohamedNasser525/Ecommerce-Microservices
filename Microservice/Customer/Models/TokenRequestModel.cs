using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public class TokenRequestModel
    {
        [Display(Name = "Email")]
        [MaxLength(150), EmailAddress, RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }


        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-+_!@#$%^&*.,?]).+$", ErrorMessage = "Invalid input Password! The data must contain at least one uppercase letter, one lowercase letter, one special character, and can include letters, numbers, and symbols")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}