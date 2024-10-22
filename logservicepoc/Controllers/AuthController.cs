using logservicepoc.DTO;
using logservicepoc.Services;
using Microsoft.AspNetCore.Mvc;

namespace logservicepoc.Controllers{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {

        private readonly IUsersService _usersService;
        private readonly IAuthservice _authservice;

        public AuthController (IUsersService usersService, IAuthservice authservice){
            _usersService = usersService;
            _authservice = authservice;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginReq loginReq)
        {
            var user = await _authservice.UserLogin(loginReq);
            if (user == null)
                return Unauthorized();

            return Ok(user);
        }
    }
}