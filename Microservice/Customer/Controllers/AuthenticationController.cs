using AuthServer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
//using Swashbuckle.AspNetCore.Annotations;
using AuthServer.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using AuthServer.Services;
using AuthServer.DTO;
namespace AuthServer.Controllers
{
    [Route("v1/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly UserManager<AppUser> _userManager;
        public AuthenticationController(UserManager<AppUser> userManager, IAuthService authService)
        {
            _userManager = userManager;
            _authService = authService;
        }
        //[HttpGet("myIP")]
        //public IActionResult GetClientIp()
        //{
        //    var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        //    return Ok(ipAddress);
        //}

        [HttpPost("register")]
        //[SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<AuthModel>))]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            //SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);
            //var user = await _userManager.FindByEmailAsync(model.Email);
            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = result.Email }, Request.Scheme);
            //await _mailingService.SendMailAsync(model.Email, "Confirm your email",
            //            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

            //return Ok("Confirm your Email address then Login");
            return Ok(result);
        }

        [HttpPost("login")]
        //[SwaggerResponse(StatusCodes.Status200OK ,Type = typeof(IEnumerable<AuthModel>))]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
    
        public async Task<IActionResult> GetTokenAsync([FromForm] TokenRequestModel model)
        {

            var result = await _authService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            if(!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

       

        [HttpGet("refreshToken")]
        //[SwaggerResponse(StatusCodes.Status200OK)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _authService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }

        [HttpPost("logout")]
        //[SwaggerResponse(StatusCodes.Status200OK)]
        //[SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RevokeToken([FromForm] RevokeToken model)
        {
            var token = model.Token ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest("Token is required!");

            var result = await _authService.RevokeTokenAsync(token);

            if(!result)
                return BadRequest("Token is invalid!");

            return Ok("Token had revoke");
        }

        [HttpPost("DecryptJWT")]
        //[SwaggerOperation(Summary = "for Decrypt token")]
        //[SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<AuthModel>))]
        public async Task<IActionResult> Decrypt([FromForm] byte[] encrypttext)
        {

            return Ok(Encoding.UTF8.GetString(AuthServer.Helper.RC4.RC4Decrypt(encrypttext)));
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                 HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        [HttpGet("ConfirmEmail")]
        //[SwaggerOperation(Summary = "call by ResendCodeConfirmEmail API to confirm email not for U")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {
                    return Ok("Email Verified Successfully");
                }
            }
            return BadRequest("invaild user");
        }

        [HttpPost("ResendCodeConfirmEmail")]
        public async Task<IActionResult> ResendCodeConfirmEmail([FromForm] string mail)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Authentication", new { token, email = mail }, Request.Scheme);

            //await _mailingService.SendMailAsync(mail, "Confirm your email",
            //            $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

            return Ok("resend Succeeded");
        }

        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword([FromForm] Login l)
        {
            var user = await _userManager.FindByEmailAsync(l.Email);
            if (user == null)
            {
                return BadRequest("invaild user");
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ResetPassword), "Authentication", new { mail = l.Email, newpassword = l.newPassword, token }, Request.Scheme);

            //await _mailingService.SendMailAsync(l.Email, "Confirm ResetPassword",
            //                       $"To Reset your Password <a href='{confirmationLink}'>clicking here</a>.");

            return Ok("Done");
        }

        [HttpGet("ResetPassword")]
        //[SwaggerOperation(Summary = "call by ForgetPassword API not for U")]
        public async Task<IActionResult> ResetPassword(string mail, string newpassword, string token)
        {
            var user = await _userManager.FindByEmailAsync(mail);
            if (user == null)
            {
                return BadRequest("invaild user");
            }
            var result = await _userManager.ResetPasswordAsync(user, token, newpassword);

            if (!result.Succeeded)
            {
                var errors = string.Empty;

                foreach (var error in result.Errors)
                    errors += $"{error.Description},";

                return BadRequest(errors);
            }
            return Ok("ResetPassword Succeeded");

        }
    }
}
