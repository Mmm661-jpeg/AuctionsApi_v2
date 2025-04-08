using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.HelperMethods.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionsApi_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly ITokenReader _tokenReader;

        public UsersController(IUsersService usersService,ITokenReader tokenReader)
        {
            _usersService = usersService;
            _tokenReader = tokenReader;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login_Req req)
        {
            var result = await _usersService.Login(req);

            if(result.IsSuccces)
            {
                return Ok(new {success = true, message = result.Message, token = result.Data});
            }
            else
            {
                return Unauthorized(new {success=false,message=result.Message});
            }
        }

        [HttpPost("Register")]

        public async Task<IActionResult> Register([FromBody] Register_Req req)
        {
            var result = await (_usersService.Register(req));

            if(result.IsSuccces)
            {
                return Ok(new { success = true, message = result.Message,data= result.Data});
            }
            else
            {
                return BadRequest(new {success=false,message=result.Message});
            }
        }

        [HttpPut("UpdateUser")]
        [Authorize]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUser_Req req)
        {
            var userId = _tokenReader.ReadIdFromToken(User);

            if(userId <= 0)
            {
                return Unauthorized(new { success = false, message = "Must be loggedin to delete user" });
            }

            req.UserId = userId; //overwrite id

            var result = await _usersService.Update(req);

            if (result.IsSuccces)
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }

        [HttpDelete("DeleteUser")]
        [Authorize]

        public async Task<IActionResult> DeleteUser()
        {
            var userId = _tokenReader.ReadIdFromToken(User);

            if(userId <= 0)
            {
                return Unauthorized(new { success = false, message = "Must be loggedin to delete user" });

            }

            var result = await _usersService.Delete(userId);

            if (result.IsSuccces)
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return BadRequest(new {succes= false, message = result.Message});
            }
        }

        [HttpGet("GetOneUserWithId")]

        public async Task<IActionResult> GetOneUser([FromQuery]int id)
        {
            var result = await _usersService.GetUsersWithId(id);

            if(!result.IsSuccces)
            {
                return BadRequest(new { success = false, message = result.Message });
            }
            else
            {
                return Ok(new { success = true, message =result.Message, data = result.Data });
            }
        }
    }
}
