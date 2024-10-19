using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class Tables
    {
        [Key]
        public int Id { get; set; }
        public int AreaId { get; set; }
        [ForeignKey(nameof(AreaId))]
        public Areas? Areas { get; set; }

        [Required]
        public string? NameTable { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsBooking { get; set; } = false ;

        public bool? HasHourlyRate { get; set; } = false;
        public DateTime? TimeStart { get; set; } = null;
        public DateTime? TimeEnd { get; set; } = null;
        public decimal? PriceOfMunite { get; set; } = 0;

    }
}
