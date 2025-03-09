using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Stores;
using Swashbuckle.AspNetCore.Annotations;
using StoreManageAPI.Config.Roles;
using StoreManageAPI.Mddleware;
using System.ComponentModel.DataAnnotations;
namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Quản lý thông tin liên quan đến cửa hàng")]
    public class ShopController
        (
        IShopSerVice shopSerVice,
        CloudinaryMiddle cloud
        )
        : ControllerBase
    {
        private readonly IShopSerVice shopSerVice = shopSerVice;
        private readonly CloudinaryMiddle cloud = cloud;
        [HttpPost("create-shop")]
        [Authorize(Roles = AppRoles.Owner)]
        [SwaggerOperation("Tạo cửa hàng" , "Tạo, đặt tên cho cửa hàng của bạn")]
        public async Task<IActionResult> CreateShop([FromForm] ShopV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await shopSerVice.CreateShopAsync(model);
            if(!result.IsSuccess)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }
        [HttpDelete("delete-shop")]
        [Authorize(Roles = AppRoles.Owner)]
        public async Task<IActionResult> DeleteShop([Required] int id ,[Required] string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await shopSerVice.DeleteShopAsync(id , password);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("update-shop")]
        [Authorize(Roles = AppRoles.Owner)]
        public async Task<IActionResult> UpdateShop([FromForm] ShopV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await shopSerVice.UpdateShopAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("active-shop")]
        [Authorize(Roles = AppRoles.Owner + "," + AppRolesShop.Manager)]
        public async Task<IActionResult> ActiveShop([Required] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await shopSerVice.IsActiveShopAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("get-list-shop")]
        [Authorize]
        [SwaggerOperation("Danh sách cửa hàng của user", "Danh sách cửa hàng của user")]
        public async Task<IActionResult> GetListShop()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await shopSerVice.GetListShopAsync();
            if (!result.IsSuccess)
            {
                return BadRequest(ModelState);
            }
            return Ok(result);
        }

        [HttpGet("get-shop-by-id")]
        [Authorize]
        [SwaggerOperation("Lấy thông tin cửa hàng bằng ID", "Lấy thông tin cửa hàng bằng ID")]
        public async Task<IActionResult> GetShopById([Required] int idShop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await shopSerVice.GetShopByIdAsync(idShop);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        
        [HttpPost("test-uploadimg")]
        public  IActionResult TestUploadImage(int shop_id , int table_id)
        {

            
            return Ok();
        }
    }
}
