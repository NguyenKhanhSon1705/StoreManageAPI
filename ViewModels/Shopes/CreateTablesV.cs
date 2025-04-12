using StoreManageAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Shopes
{
    public class CreateTablesV
    {
        public int Id { get; set; }
        public int AreaId { get; set; }
        [Required]
        public string? NameTable { get; set; }

        public bool? HasHourlyRate { get; set; } = false;
        public decimal? PriceOfMunite { get; set; } = 0;
    }
}
