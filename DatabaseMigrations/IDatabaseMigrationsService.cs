using StoreManageAPI.ModelReturnData;

namespace StoreManageAPI.DatabaseMigrations
{
    public interface IDatabaseMigrationsService
    {
        Task<ApiResponse> CreateDatabaseAsync();
        Task<ApiResponse> ListTableInDataBaseAsync();
        Task<ApiResponse> CheckConnectDatabaseAsync();
        Task<ApiResponse> DeleteDatabaseAsync(string secretKey);
        Task<ApiResponse> MigrationsAddDatabaseAsync();
        Task<ApiResponse> SeedDataDefaultAsync();


    }
}
