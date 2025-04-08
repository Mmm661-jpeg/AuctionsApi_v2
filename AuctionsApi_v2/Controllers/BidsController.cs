using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using AuctionsApi_v2.Domain.Models;
using AuctionsApi_v2.HelperMethods.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionsApi_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidsController : ControllerBase
    {
        private readonly IBidsService _bidsService;
        private readonly ITokenReader _tokenReader;

        public BidsController(IBidsService bidsService,ITokenReader tokenReader)
        {
            _bidsService = bidsService;
            _tokenReader = tokenReader;
        }

        [HttpPost("MakeBid")]
        [Authorize]

        public async Task<IActionResult> MakeBid(MakeBid_Req req)
        {
            var userId = _tokenReader.ReadIdFromToken(User);

            if (userId <= 0)
            {
                return Unauthorized(new { success = false, message = "Must be loggedin to make bid" });
            }

            req.UserId = userId;

            var result = await _bidsService.MakeBid(req);

            if(result.IsSuccces)
            {
                return Ok(new {success=true,message=result.Message,data = result.Data});
            }
            else
            {
                return BadRequest(new {success=false,message=result.Message});
            }
        }

        [HttpDelete("RemoveBid")]
        [Authorize]
        public async Task<IActionResult> RemoveBid([FromQuery]int bidId)
        {
            var userId = _tokenReader.ReadIdFromToken(User);

            if (userId <= 0)
            {
                return Unauthorized(new { success = false, message = "Must be loggedin to remove bid" });
            }

            var result = await _bidsService.RemoveBid(userId, bidId);

            if (result.IsSuccces)
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }


        }

        [HttpGet("ViewAllBidsWithID")]

        public async Task<IActionResult> ViewAllBidsWithID([FromQuery] int userId)
        {
            var result = await _bidsService.ViewAllBidsWithID(userId);

            if (!result.IsSuccces)
            {
                return NotFound(new { success = false, message = result.Message});
            }
            else
            {
                return Ok(new { success = true, message = result.Message,data=result.Data });
            }
        }

        [HttpGet("ViewBidsOnAuction")]

        public async Task<IActionResult> ViewBidsOnAuction([FromQuery] int auctionId)
        {
            var result = await _bidsService.ViewBidsOnAuction(auctionId);

            if (!result.IsSuccces)
            {
                return NotFound(new { success = false, message = result.Message });
            }
            else
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
        }
    }
}
