using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManageAPI.DTO.Reports;
using StoreManageAPI.Services.Interfaces;

namespace StoreManageAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController
        (
        IReportAbortedService serviceAbort,
        IReportBillService serviceBill,
        IStatisticalService serviceStatistical
        )
        : ControllerBase
    {
        private readonly IReportAbortedService serviceAbort = serviceAbort;
        private readonly IReportBillService serviceBill = serviceBill;
        private readonly IStatisticalService serviceStatistical = serviceStatistical;

        [HttpGet("report-aborted")]
        public async Task<IActionResult> GetReportAborted([FromQuery] ReportFilterDTO model)
        {
            var result = await serviceAbort.GetAllReportAbortedAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("report-aborted-detail")]
        public async Task<IActionResult> ShowReportAbortedDetail([FromQuery] int id)
        {
            var result = await serviceAbort.ShowAborteDetailAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("report-bill")]
        public async Task<IActionResult> GetReportBill([FromQuery] ReportFilterDTO model)
        {
            var result = await serviceBill.GetAllReportBillAsync(model);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("report-bill-detail")]
        public async Task<IActionResult> ShowReportBillDetail(int id)
        {
            var result = await serviceBill.ShowBillDetailAsync(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpGet("report-revenue")]
        public async Task<IActionResult> GetReportRevenue([FromQuery] FilterRevenueReportDTO model)
        {
            var result = await serviceStatistical.RevenueReportAsync(model);
            if(!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}
