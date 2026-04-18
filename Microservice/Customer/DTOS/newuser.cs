using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace AuthServer.DTOS
{
    public class newuser
    {
        [RegularExpression(@"^[a-zA-Z0-9@,._\s]+$", ErrorMessage = "in Name Please enter characters like (a~z, A~Z, 0~9, @, _, ., ,, space) not more")]
        [StringLength(50)]
        public string ?Username { get; set; }

        [MaxLength(150), EmailAddress, RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }


    }
     
}
