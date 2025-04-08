
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Services.VnPay;
using VNPAY.NET;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace StoreManageAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VnpayController : ControllerBase
    {
        private readonly IVnpayService _vnpayService;

        public VnpayController(IVnpayService vnpayService)
        {
            _vnpayService = vnpayService;
        }

        [HttpGet("CreatePaymentUrl")]
        public async Task<ActionResult<string>> CreatePaymentUrl(double moneyToPay, string description, int shop_id, int table_id)
        {
            try
            {
                var paymentUrl = await _vnpayService.CreatePaymentUrl(moneyToPay, description, shop_id, table_id, HttpContext);
                return Created(paymentUrl, paymentUrl);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpGet("ipn")]
        //public async Task<IActionResult> IpnAction()
        //{
        //    if (Request.QueryString.HasValue)
        //    {
        //        Console.WriteLine("IpnAction");
        //        try
        //        {
        //            var paymentResult = await _vnpayService.HandleIpn(Request.Query);
        //            if (paymentResult.IsSuccess)
        //            {
        //                Console.WriteLine("IpnAction success");

        //                // Thực hiện hành động nếu thanh toán thành công tại đây. Ví dụ: Cập nhật trạng thái đơn hàng trong cơ sở dữ liệu.
        //                return Ok("IpnAction success");
        //            }
        //            Console.WriteLine("IpnAction errror");

        //            // Thực hiện hành động nếu thanh toán thất bại tại đây. Ví dụ: Hủy đơn hàng.
        //            return BadRequest("Thanh toán thất bại");
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //    }

        //    return NotFound("Không tìm thấy thông tin thanh toán.");
        //}


        [HttpGet("Callback")]
        public async Task<ActionResult<ApiResponse>> CallbackAction()
        {
            try
            {
                var paymentResult = await _vnpayService.HandleCallback(Request.Query);
                if (!paymentResult.IsSuccess)
                {
                    return BadRequest(paymentResult);
                }

                return Ok(paymentResult);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
