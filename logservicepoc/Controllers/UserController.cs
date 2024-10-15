using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using logservicepoc.Models;
using logservicepoc.Services;
using logservicepoc.DTO;

namespace logservicepoc.Controllers
{
    [ApiController]
    // [Route("users")]
    public class UserController : Controller{
        private readonly IUsersService _usersService;

        public UserController (IUsersService usersService){
            _usersService = usersService;
        }

        [HttpPost]
        [Route("user")]
        public async Task<IActionResult> CreateUser([FromBody] UserReq user){
            var result = await _usersService.CreateUser(user);
            return Ok(result);
        }

        [HttpPost]
        [Route("users")]
        public async Task<dynamic> GetUsers([FromBody] ListReq listReq){
            Console.Write("Hi im here");
            var result = await _usersService.GetAllUsers(listReq);
            return Ok(result);
        }

    }
}