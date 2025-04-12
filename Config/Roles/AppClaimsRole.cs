namespace StoreManageAPI.Config.Roles
{
    public class AppClaimsRole
    {
        // cliams cho chủ sở hữu
        public const string ManageAll = "Quản lý tất cả";
        public const string AddDish = "Thêm mặt hàng";

        // claims cho quản lý
        public const string ManageStaff = "Quản lý nhân viên";
        public const string Revenue = "Doanh thu";
        public const string ManageSupplier = "Quản lý nhà cung cấp";

        // claims cho nhân viên 
        public const string Payment = "Thanh toán hóa đơn";

        public const string CancelDish = "Hủy món";

        public const string OpenTable = "Mở bàn";

        public const string ChangeTable = "Đổi bàn";


    }
}
