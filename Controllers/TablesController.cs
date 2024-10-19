using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Shopes;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablesController
        (
        ITablesService service
        )
        : ControllerBase
    {
        private readonly ITablesService service = service;

        [HttpGet("get-table-area")]
        public async Task<IActionResult> GetTableArea(int shopId)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.GetListTableAsync(shopId);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("get-table-by-id")]
        public async Task<IActionResult> GetTableById(int id)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.GetTableByIdAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("create-tables")]
        public async Task<IActionResult> CreateTables([FromBody] CreateTablesV model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateTableAsync(model);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut("update-tables")]
        public async Task<IActionResult> UpdateTables([FromBody]CreateTablesV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdateTableAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpDelete("delete-tables")]
        public async Task<IActionResult> DeleteTables(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.DeleteTableAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }



    }
}
