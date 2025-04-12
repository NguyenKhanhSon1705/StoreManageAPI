using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Config.Roles;
using StoreManageAPI.DatabaseMigrations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Quản lý cơ sở dữ liệu dành cho nhà phát triển (dev)")]
  
    public class DatabaseMigrationsController
        (
        IDatabaseMigrationsService dbservice
        ) : ControllerBase
    {
        private readonly IDatabaseMigrationsService dbservice = dbservice;
        
        
        [HttpGet("get-all-table-Database")]
        [Authorize(Roles = AppRoles.Developer + "," + AppRoles.Administrator )]
        [SwaggerOperation("Danh sách các bảng trong database" , "Danh sách các bảng trong database")]
        public async Task<IActionResult> GetAllTableInDatabase()
        {
            var result = await dbservice.ListTableInDataBaseAsync();
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("check-connect-database")]
        [Authorize(Roles = AppRoles.Administrator)]
        [SwaggerOperation("Kiểm tra kết nối", "Kiểm tra kết nối với cơ sở dữ liệu")]
        public async Task<IActionResult> CheckConnectDatabase()
        {
            var result = await dbservice.CheckConnectDatabaseAsync();
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("Initialization-database")]
        [Authorize(Roles = AppRoles.Developer + "," + AppRoles.Administrator)]

        [SwaggerOperation("Kiểm tra và khởi tạo cơ sở dữ liệu" , "Kiểm tra và khởi tạo cơ sở dữ liệu")]
        public async Task<IActionResult> InitializationDatabase()
        {
            var result = await dbservice.CreateDatabaseAsync();
            if(! result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("delete-database")]
        [Authorize(Roles = AppRoles.Administrator)]
        [SwaggerOperation("Xóa cơ sở dữ liệu" , "Xóa cơ sở dữ liệu chỉ dành cho dev có khóa bí mật")]
        public async Task<IActionResult> DeleteDatabase([FromBody][Required]string secretKey)
        {
            var result = await dbservice.DeleteDatabaseAsync(secretKey);
            if(! result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("generate-table-database")]
        [Authorize(Roles = AppRoles.Developer + "," + AppRoles.Administrator)]
        [SwaggerOperation("Tạo tự động các bảng vào csdl" , "Tạo tự động các bảng vào csdl")]
        public async Task<IActionResult> GenerateTableDatabase()
        {
            var result = await dbservice.MigrationsAddDatabaseAsync();
            if(! result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPost("seed-data-default")]
        [SwaggerOperation("Tạo dữ liệu cố định cho cơ sở dữ liệu" , "Tạo dữ liệu cố định cho cơ sở dữ liệu")]
        public async Task<IActionResult> SeedDataDefault()
        {
            var result = await dbservice.SeedDataDefaultAsync();
            if(! result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }


    }
}
