using AuctionsApi_v2.Core.Interfaces;
using AuctionsApi_v2.Domain.DTO_s.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuctionsApi_v2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            _categoriesService = categoriesService;
        }

        [HttpGet("GetCategories")]

        public async Task<IActionResult> GetCategories()
        {
            var result = await _categoriesService.GetCategoriesAsync();

            if(!result.IsSuccces)
            {
                return NotFound(new {success=false,message=result.Message});
            }
            else
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
        }

        [HttpGet("GetOneCategory")]
        public async Task<IActionResult> GetOneCategory([FromQuery] int categoryId)
        {
            var result = await _categoriesService.GetOneCategory(categoryId);

            if (!result.IsSuccces)
            {
                return NotFound(new { success = false, message = result.Message });
            }
            else
            {
                return Ok(new { success = true, message = result.Message, data = result.Data});
            }

        }

        [HttpPost("AddCategory")]
        //[Authorize] //role
        public async Task<IActionResult> AddCategory(AddCategory_Req req)
        {
            var result = await _categoriesService.AddCategory(req);

            if (result.IsSuccces)
            {
                return Ok(new { success = true, message = result.Message, data = result.Data });
            }
            else
            {
                return BadRequest(new { success = false, message = result.Message });
            }
        }

        [HttpDelete("RemoveCategory")]
        //[Authorize]

        public async Task<IActionResult> RemoveCategory([FromQuery]int categoryId)
        {
            var result = await _categoriesService.RemoveCategory(categoryId);

            if (result.IsSuccces)
            {
                return Ok(new { result = true, message = result.Message, data = result.Data });
            }
            else
            {
                return BadRequest(new { result = false, message = result.Message });
            }
        }



    }
}
