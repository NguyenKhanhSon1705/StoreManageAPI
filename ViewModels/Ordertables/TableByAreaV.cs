using StoreManageAPI.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Ordertables
{
    public class TableByAreaV
    {
        public int Id { get; set; }
       
        public string? areaName { get; set; }
        public string? NameTable { get; set; }
        public bool IsActive { get; set; } = false;
        public bool IsBooking { get; set; } = false;

        public bool? HasHourlyRate { get; set; } = false;
        public DateTime? TimeStart { get; set; } = null;
        public DateTime? TimeEnd { get; set; } = null;
        public decimal? PriceOfMunite { get; set; } = 0;
    }
}
