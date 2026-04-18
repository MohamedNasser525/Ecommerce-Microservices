using System.ComponentModel.DataAnnotations;

namespace AuthServer.Models
{
    public class RegisterModel
    {

        [RegularExpression(@"^[a-zA-Z0-9@,._\s]+$", ErrorMessage = "in Name Please enter characters like (a~z, A~Z, 0~9, @, _, ., ,, space) not more")]
        [StringLength(50)]
        public string Username { get; set; }

        [MaxLength(150), EmailAddress, RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }


        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-+_!@#$%^&*.,?]).+$", ErrorMessage = "Invalid input! The data must contain at least one uppercase letter, one lowercase letter, one special character, and can include letters, numbers, and symbols")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [DataType(DataType.Password)]
        [StringLength(256)]
        public string Password { get; set; }


        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[-+_!@#$%^&*.,?]).+$", ErrorMessage = "Invalid input! The data must contain at least one uppercase letter, one lowercase letter, one special character, and can include letters, numbers, and symbols")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        public string ConfirmPassword { get; set; }
    }
}