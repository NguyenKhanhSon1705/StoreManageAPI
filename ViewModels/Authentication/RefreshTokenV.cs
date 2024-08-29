namespace StoreManageAPI.ViewModels.Authentication
{
    public class RefreshTokenV
    {
        public string? UserId { get; set; }
        public string? TokenRefresh { get; set; }
        public string? IpAddress { get; set; } // Địa chỉ IP
        public string? DeviceInfo { get; set; }

        public bool? IsMobile { get; set; } = false;
    }
}
