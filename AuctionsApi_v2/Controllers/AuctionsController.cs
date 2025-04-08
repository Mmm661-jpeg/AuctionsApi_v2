using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Data.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.HelperMethods.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionsApi_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuctionsController : ControllerBase
    {
        private readonly IAuctionsService _auctionsService;
        private readonly ITokenReader _tokenReader;

        public AuctionsController(IAuctionsService auctionsService,ITokenReader tokenReader)
        {
            _auctionsService = auctionsService;
            _tokenReader = tokenReader;
        }

        [HttpPost("CreateAuction")]
        [Authorize]
        public async Task<IActionResult> CreateAuction(CreateAuction_Req req)
        {
            var userId = _tokenReader.ReadIdFromToken(User);

            if (userId <= 0)
            {
                return Unauthorized(new { success = false, message = "Must be loggedin to make auction" });
            }

            req.UserId = userId;

            var result = await _auctionsService.CreateAuction(req);

            if (result.IsSuccces)
            {
                {
                    return Ok(new { success = true, message = result.Message, data = result.Data });
                }
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }

        [HttpPut("UpdateAuction")]
        [Authorize]
        public async Task<IActionResult> UpdateAuction(UpdateAuction_Req req)
        {
            var userId = _tokenReader.ReadIdFromToken(User);

            if (userId <= 0)
            {
                return Unauthorized(new { success = false, message = "Must be loggedin to update auction" });
            }

            req.UserId = userId;

            var result = await _auctionsService.UpdateAuction(req);

            if (result.IsSuccces)
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }

        }

        [HttpDelete("DeleteAuctions")]
        [Authorize]
        public async Task<IActionResult> DeleteAuctions([FromQuery] int auctionId)
        {
            var userId = _tokenReader.ReadIdFromToken(User);

            if (userId <= 0)
            {
                return Unauthorized(new { success = false, message = "Must be loggedin to update auction" });
            }

            var result = await _auctionsService.DeleteAuctions(auctionId, userId);

            if (result.IsSuccces)
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return BadRequest(new {success=false, message = result.Message});
            }


        }

        [HttpGet("GetAuctionsWithId")]
        public async Task<IActionResult> GetAuctionsWithId([FromQuery] int auctionId)
        {
            var result = await _auctionsService.GetAuctionsWithId(auctionId);

            if(!result.IsSuccces)
            {
                return NotFound(new { success = false, message = result.Message });
            }
            else
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
        }

        [HttpGet("GetAllAuctions")]
        public async Task<IActionResult> GetAllAuctions()
        {
            var result = await _auctionsService.GetAllAuctions();

            if (!result.IsSuccces)
            {
                return NotFound(new { success = false, message = result.Message });
            }
            else
            {
                return Ok(new { succcess = true, message = result.Message, data = result.Data });
            }
        }
    }
}
