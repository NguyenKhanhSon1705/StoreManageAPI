using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Org.BouncyCastle.Asn1.Ocsp;
using StoreManageAPI.Context;
using StoreManageAPI.DTO.Payment;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.Services.Interfaces;
using VNPAY.NET;
using VNPAY.NET.Enums;
using VNPAY.NET.Models;
using VNPAY.NET.Utilities;

namespace StoreManageAPI.Services.VnPay
{
    public interface IVnpayService
    {
        Task<string> CreatePaymentUrl(double moneyToPay, string description, int shop_id, int table_id, HttpContext httpContext);
        Task<ApiResponse> HandleCallback(IQueryCollection query);
        //Task<ApiResponse> HandleIpn(IQueryCollection query);
    }

    public class VnpayService : IVnpayService
    {
        private readonly IVnpay _vnpay;
        private readonly ITableDishService _service;
        private readonly ILogger<VnpayService> _logger;
        private readonly DataStore context;
        public VnpayService(IVnpay vnPayService, IConfiguration configuration , ITableDishService service, ILogger<VnpayService> logger , DataStore context)
        {
            _vnpay = vnPayService;
            _service = service;
            _logger = logger;
            this.context = context;

            // Initialize VNPAY configuration
            _vnpay.Initialize(
                configuration["Vnpay:TmnCode"] ?? "",
                configuration["Vnpay:HashSecret"] ?? "",
                configuration["Vnpay:BaseUrl"] ?? "",
                configuration["Vnpay:CallbackUrl"] ?? ""
            );
        }

        public static string FormatNumberWithLeadingZeros(int number, int totalLength)
        {
            // Sử dụng PadLeft để thêm các số 0 vào đầu
            return number.ToString().PadLeft(totalLength, '0');
        }
        public async Task<string> CreatePaymentUrl(double moneyToPay, string description, int shop_id, int table_id, HttpContext httpContext)
        {
            if (moneyToPay <= 0)
            {
                throw new ArgumentException("Số tiền phải lớn hơn 0.");
            }

            try
            {
                var checkShop = await context.Shop.AnyAsync(an => an.Id == shop_id);
                if (!checkShop)
                {
                    return "Thông tin cửa hàng không hợp lệ";
                }
                var checkTable = await context.Tables.FirstOrDefaultAsync(fr => fr.Id == table_id);
                if (checkTable == null)
                {
                    return "Thông tin bàn không hợp lệ";
                }
                if (!checkTable.IsActive)
                {
                    return "Bàn này đã được thanh toán hoặc chưa được mở";
                }



                var ipAddress = NetworkHelper.GetIpAddress(httpContext);

                string ids = FormatNumberWithLeadingZeros(shop_id, 5);
                string idt = FormatNumberWithLeadingZeros(table_id, 5);
                Random random = new Random();

                long creatIdPayment = long.Parse($"1{ids}{idt}{FormatNumberWithLeadingZeros(random.Next(1, 10000), 5)}");

                
                var request = new PaymentRequest
                {
                    PaymentId = creatIdPayment,
                    Money = moneyToPay,
                    Description = description,
                    IpAddress = ipAddress
                };

                return _vnpay.GetPaymentUrl(request);

            }catch (Exception ex)
            {
                _logger.LogError($"There are something error in ShopService/CreateStoreName: {ex.Message} at {DateTime.UtcNow}");

                return $"There are something error in ShopService/CreateStoreName: {ex.Message} at {DateTime.UtcNow}";
            }
        }

        //public async Task<ApiResponse> HandleIpn(IQueryCollection query)
        //{
        //    try
        //    {
        //        Console.WriteLine("HandleIpn");

        //        var paymentResult = _vnpay.GetPaymentResult(query);
        //        if (paymentResult.IsSuccess)
        //        {
        //            var paymentId = paymentResult.PaymentId;
        //            Console.WriteLine(paymentId.ToString().Substring(1, 5));
        //            Console.WriteLine(paymentId.ToString().Substring(6, 5));
        //            Console.WriteLine(paymentId.ToString().Substring(11, 5));
        //            Console.WriteLine("HandleIpn");

        //            return new ApiResponse
        //            {
        //                Message = "Thành công",
        //                StatusCode = 200,
        //                IsSuccess = true,
        //            };
        //        }
        //        return new ApiResponse
        //        {
        //            Message = "Thành công",
        //            StatusCode = 400,

        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ApiResponse
        //        {
        //            Message = ex.Message,
        //            StatusCode = 400,
        //        };
        //    }
           
        //}

        public async Task<ApiResponse> HandleCallback(IQueryCollection query)
        {
           
            var paymentResult = _vnpay.GetPaymentResult(query);

            if (paymentResult.IsSuccess)
            {
            var paymentId = paymentResult.PaymentId;
            string shop_id = paymentId.ToString().Substring(1, 5);
            string table_id = paymentId.ToString().Substring(6, 5);


            var checkShop = await context.Shop.AnyAsync(an => an.Id == int.Parse(shop_id));
            if (!checkShop)
            {
                return new ApiResponse
                {
                    Message = "Thông tin cửa hàng không hợp lệ"
                };
            }
            var checkTable = await context.Tables.FirstOrDefaultAsync(fr => fr.Id == int.Parse(table_id));
            if (checkTable == null)
            {
                return new ApiResponse
                {
                    Message = "Thông tin bàn không hợp lệ hoặc đã được thanh toán"
                };
            }
            if (!checkTable.IsActive)
            {
                return new ApiResponse
                {
                    Message = "Thông tin bàn không hợp lệ hoặc đã được thanh toán"
                };
            }

            var model = new PaymentDTO
            {
                shop_id = int.Parse(shop_id),
                table_Id = int.Parse(table_id),
                payment_method = 1
            };


            var transaction = new TransactionsDTO
            {
                transaction_id = paymentId,
                payment_date = paymentResult.Timestamp,
                bank_code = paymentResult.BankingInfor.BankCode,
                payment_method = paymentResult.PaymentMethod,
                status_name = paymentResult.PaymentResponse.Description,
                message = paymentResult.TransactionStatus.Description,
                decription = paymentResult.Description,
            };

            await _service.PaymentAsync(model , transaction);
                return new ApiResponse
                {
                    Message = $"{paymentResult.PaymentResponse.Description} - {paymentResult.TransactionStatus.Description}",
                    StatusCode = 200,
                    Data = paymentResult,
                    IsSuccess = true
                };
            }
            return new ApiResponse
            {
                Message = $"{paymentResult.PaymentResponse.Description} - {paymentResult.TransactionStatus.Description}",
                StatusCode = 400,
                Data = paymentResult,
                IsSuccess = false
            };
        }
    }
}
