using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.Services.Interfaces;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController
        (IRolesService role)
        : ControllerBase
    {
        private readonly IRolesService service = role;

        [HttpGet("get-list-role-shop")]
        public async Task<IActionResult> GetListRoleShop()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await service.GetListRoleShop();
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

    }
}
