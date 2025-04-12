using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Shopes;
using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreasController
        (
            IAreasService service
        )

        : ControllerBase
    {
        private readonly IAreasService service = service;

        [HttpGet("get-list-areas")]
        public async Task<IActionResult> GetListAreas([Required] int idShop)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await service.GetListAreaAsync(idShop);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("get-area-by-id")]
        public async Task<IActionResult> GetAreaById([Required] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.GetAreaByIdAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("create-area")]
        public async Task<IActionResult> CreateArea([FromBody] AreasV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.CreateAreaAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpPut("update-area")]
        public async Task<IActionResult> UpdateArea([FromBody] AreasV model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.UpdateAreaAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpDelete("delete-area")]
        public async Task<IActionResult> DeleteArea([Required] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var result = await service.DeleteAreaAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
