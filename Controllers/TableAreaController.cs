using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Services.Interfaces;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TableAreaController 
        (
        ITableAreaService service
        ) : ControllerBase
    {
        private readonly ITableAreaService service = service;

        [HttpGet("get-tables-by-area")]
        public async Task<IActionResult> GetTableByArea(int areaId)
        {
            var result =  await service.GetTableByArea(areaId);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
                return Ok(result);
        }
    }
}
