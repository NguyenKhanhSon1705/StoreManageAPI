using StoreManageAPI.ModelReturnData;

namespace StoreManageAPI.Services.Interfaces
{
    public interface IRolesService
    {
        public Task<ApiResponse> GetListRoleShop();
    }
}
