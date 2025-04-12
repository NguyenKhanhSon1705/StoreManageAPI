using StoreManageAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Shopes
{
    public class TableInfoV
    {
        [Key]
        public int Id { get; set; }
        public int? AreaId { get; set; }
        
        public string? AreaName { get; set; }

        public string? NameTable { get; set; }
        public bool IsActive { get; set; } = false;

        public bool? HasHourlyRate { get; set; } = false;
     
        public decimal? PriceOfMunite { get; set; } = 0;
    }
}
