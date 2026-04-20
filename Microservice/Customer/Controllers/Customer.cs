using Microsoft.AspNetCore.Mvc;
namespace Customer.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    // Renamed to CustomerController to follow .NET conventions
    public class CustomerController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("it work Customer Service");
        }
    }
}
