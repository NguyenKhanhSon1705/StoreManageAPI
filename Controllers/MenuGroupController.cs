using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Dish;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuGroupController
    (
        IMenuGroupService service   
    )   
    : ControllerBase
    {
        private readonly IMenuGroupService service = service;
        [HttpPost("create-menu-group")]
        public async Task<IActionResult> CreateMenuGroup([FromForm] CreateMenuGroupV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateGroupMenuAsync(model);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("update-menu-group")]
        public async Task<IActionResult> UpdateMenuGroup([FromForm] CreateMenuGroupV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdateGroupMenuAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("get-all-menu-group")]
        public async Task<IActionResult> GetAllMenuGroup(int shopId, int pageIndex = 1, int limit = 10, string search = "")
        {
            var result = await service.GetAllGroupMenusAsync(shopId, pageIndex, limit, search);
            if (!result.IsSuccess)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpDelete("delete-menu-group")]
        public async Task<IActionResult> DeleteMenuGroup(int id)
        {
            var result = await service.DeleteGroupMenuAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-all-name-menu-group")]
        public async Task<IActionResult> GetAllNameMenuGroup(int shopId)
        {
            var result = await service.GetAllNameMenuGroup(shopId);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
