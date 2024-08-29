using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class RefreshToken
    {
        [Key]
        public string? JwtId { get; set; }

        public string UserId { get; set; } = "";
        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public string TokenRefresh { get; set; } = "";

        public bool IsUsed { get; set; }
        public bool IsRevoked { get; set; }

        public string? IpAddress { get; set; } = ""; // Địa chỉ IP
        public string? DeviceInfo { get; set; } = "";

        public bool IsMobile { get; set; } = false;

        public DateTime IssuedAt { get; set; }
        public DateTime ExpiredAt { get; set; }
    }
}
