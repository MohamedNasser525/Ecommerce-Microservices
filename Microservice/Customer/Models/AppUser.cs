using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Versioning;
using AuthServer.Models;

namespace AuthServer.Models
{


    public class AppUser : IdentityUser
    {


        public byte[] Profileimg { get; set; } = { };

        public List<RefreshToken>? RefreshTokens { get; set; }


    }


}
  