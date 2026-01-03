using StoreManageAPI.DTO.Order;
using StoreManageAPI.DTO.Payment;
using StoreManageAPI.ModelReturnData;
using StoreManageAPI.ViewModels.Ordertables;

namespace StoreManageAPI.Services.Interfaces
{
    public interface ITableDishService
    {
        public Task<ApiResponse> GetInfoCheckoutAsync(int table_id, int shop_Id);
        public Task<ApiResponse> PaymentAsync(PaymentDTO model , TransactionsDTO? transection = null);
        public Task<ApiResponse> OpenTableAsync(CreateTableDishV model);
        public Task<ApiResponse> GetInfoDishCurrentTable(int tableId);
        public Task<ApiResponse> UpdateTableDishAsync(CreateTableDishV model);
        public Task<ApiResponse> AbortedDishOnTableAsync(AbortedTableDTO model);
        public Task<ApiResponse> ChangeTableDish(int tableIdOld, int tableIdNew);

    }
}
