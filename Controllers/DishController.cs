using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mysqlx.Crud;
using StoreManageAPI.Config;
using StoreManageAPI.DTO.Dish;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Dish;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DishController
        (
        IDishService service
        )
        : ControllerBase
    {
        private readonly IDishService service = service;

        [HttpPost("create-dish")]
        public async Task<IActionResult> CreateDish([FromBody] CreateDishV model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateDishAsync(model);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("add-price-dish")]
        public async Task<IActionResult> AddPriceDish([FromBody] AddPriceDishDTO model)
        {
            var result = await service.AddPriceDishAsync(model);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
        [HttpGet("get-all-dish")]
        //[Authorize(Roles = Config.Roles.AppRoles.Owner)]
        public async Task<IActionResult> GetAllDish(int shopId, int pageIndex = 1, int pageSize = 10, string search = "")
        {
            var result = await service.GetAllDishAsync(shopId, pageIndex, pageSize , search);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
                return Ok(result);
        }

        [HttpGet("get-dish-menugroup")]
        public async Task<IActionResult> GetAllDishByGroupMenu(string? search, int pageIndex = 1, int pageSize = 10, int? menuGroupId = null, int? shopId = null)
        {
            if(shopId == null)
            {
                return BadRequest();
            }
            var result = await service.GetAllDishByGroupMenu(search, pageIndex, pageSize, menuGroupId, shopId);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("update-dish")]
        public async Task<IActionResult> UpdateDish([FromBody]CreateDishV model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdateDishAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("delete-dish")]
        public async Task<IActionResult> DeleteDish(int Id)
        {
            var result = await service.DeleteDishAsync(Id);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
                return Ok(result);
        }

        [HttpDelete("delete-price-dish")]
        public async Task<IActionResult> DeletePriceDish(int id)
        {
            var result = await service.DeletePriceDishAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
