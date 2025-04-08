using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.DTO.Order;
using StoreManageAPI.DTO.Payment;
using StoreManageAPI.Services.Interfaces;
using StoreManageAPI.ViewModels.Ordertables;

namespace StoreManageAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class OrderTableDishController
        (
        ITableDishService service
        )
        : ControllerBase
    {
        private readonly ITableDishService service = service;

        [HttpGet("get-info-checkout")]
        public async Task<IActionResult> GetCheckoutInfo(int table_id , int shop_id)
        {
            var result = await service.GetInfoCheckoutAsync( table_id, shop_id);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("checkout-table")]
        public async Task<IActionResult> PaymentTable([FromBody] PaymentDTO model)
        {
            if (model == null)
            {
                return BadRequest();
            }
            var result = await service.PaymentAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("open-table-dish")]
        public async Task<IActionResult> OpenTableDish([FromBody]CreateTableDishV model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await service.OpenTableAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
        [HttpGet("get-dish-table")]
        public async Task<IActionResult> GetInfoDishCurrentTable(int tableId)
        {
            var result = await service.GetInfoDishCurrentTable(tableId);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("choose-table-dish")]
        public async Task<IActionResult> ChooseTableDish(ChooseTableId model)
        {
            var result = await service.ChangeTableDish(model.table_id_old, model.table_id_new);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("update-dish-table")]
        public async Task<IActionResult> UpdateDishTable(CreateTableDishV model)
        {
            var result = await service.UpdateTableDishAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPost("aborted-table")]
        public async Task<IActionResult> AbortedTable(AbortedTableDTO model)
        {
            var result = await service.AbortedDishOnTableAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
