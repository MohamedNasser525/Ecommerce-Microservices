using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using AuthServer.DTOS;
using AuthServer.Models;
using System.Drawing;

using Microsoft.AspNetCore.Authorization;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel.DataAnnotations;
using AuthServer.DTOS;
using System.IdentityModel.Tokens.Jwt;
using AuthServer.Helper;
//using Swashbuckle.AspNetCore.Annotations;
using Customer.Models;
//using AuthServer.Helper;
namespace AuthServer.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize]

   
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private new List<string> _allowExtentions = new List<string> { ".jpg", ".png" };
        private long _allowSize = 1048576 * 3; // 3m
        private readonly IDCheck _idCheck;
        public UserController(UserManager<AppUser> userManager, ApplicationDbContext context, IDCheck idCheck)
        {
            _context = context;
            _userManager = userManager;
            _idCheck = idCheck;

        }

        [HttpGet]
        [Route("/user/{id}")]
        //[SwaggerOperation(Summary = "Getting some info about the user")]

        public async Task<IActionResult> myinfo(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound($"id not correct : {id}");



            if (!_idCheck.IsUserIdValid(id))
            {
                return Unauthorized( "Unauthorized access." );
            }
          


            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.Profileimg,
            });
        }

        public class picture
        {
            public IFormFile? poster { get; set; }

        }
        [HttpPost]
        [Route("/profilepicture/{id}")]
        //[SwaggerOperation(Summary = "Change profile picture")]

        public async Task<IActionResult> profilepicture(string id, [FromForm] picture bot)
        {
            if (!_idCheck.IsUserIdValid(id))
            {
                return Unauthorized("Unauthorized access.");
            }
            IFormFile file = bot.poster;

            if (!ModelState.IsValid)
            {
                return BadRequest("somethig be wrong");
            }
            if (bot.poster == null)
                return BadRequest("Picture is required !!");

            if (!_allowExtentions.Contains(Path.GetExtension(bot.poster.FileName).ToLower()))
                return BadRequest("only allow Extentions be .jpg or .png !!");

            if (bot.poster.Length > _allowSize)
                return BadRequest("Max size of Picture 3m !!");

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound($"id not correct : {id}");


            using var datastream = new MemoryStream();
            await bot.poster.CopyToAsync(datastream);
            user.Profileimg = datastream.ToArray();

            await _userManager.UpdateAsync(user);
            _context.SaveChanges();
            return Ok(user.Profileimg);

        }

        

        [Route("/update/{id}")]
        [HttpPatch]
        //[SwaggerOperation(Summary = "Updating name , mail")]

        public async Task<IActionResult> Modified(string id, [FromForm] newuser model)
        {
            if (!_idCheck.IsUserIdValid(id))
            {
                return Unauthorized("Unauthorized access.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { return NotFound("ID user not valid"); }

            if (model.Email != null) {
                var usermail = await _userManager.FindByEmailAsync(model.Email);
                if (usermail != null && usermail.Id != id)
                {
                    //ModelState.AddModelError("Email", "Email is alredy exists");
                    return BadRequest("Email is alredy exists");
                }
                user.Email = model.Email;

            }

        

            if (model.Username != null)
            {
                user.UserName = model.Username;

            }
            //var result = await _userManager.CreateAsync(user, model.Password);
            //   var result = await _userManager.ChangePasswordAsync(user, user.PasswordHash,model.Password);


            await _userManager.UpdateAsync(user);
            _context.SaveChanges();

            /*  if (!result.Succeeded)
              {
                  var errors = string.Empty;

                  foreach (var error in result.Errors)
                      errors += $"{error.Description},";

                  return BadRequest(errors);
              }*/
            return Ok(new {
                id = user.Id,
                name = user.UserName,
                Email = user.Email,
            });

        }

       

        [HttpPost]
        [Route("/resetpass/{id}")]
        public async Task<IActionResult> Modifiedpass(string id, [FromForm] mypass model)
        {
            if (!_idCheck.IsUserIdValid(id))
            {
                return Unauthorized("Unauthorized access.");
            }
            if (!ModelState.IsValid)
            {
                return BadRequest("something wrong");
            }
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) { return NotFound("ID user not valid"); }

            if (model.newPassword != model.ConfirmPassword)
                return BadRequest("The newPassword and ConfirmPassword do not match.");

            var result = await _userManager.ChangePasswordAsync(user,model.oldPassword, model.newPassword);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return BadRequest(errors);
            }
            
            return Ok("Password changed");
        }

    }
}
