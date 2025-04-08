using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Ordertables
{
    public class CreateTableDishV
    {
        [Required]
        public int? tableId { get; set; }
        public List<OrderDish>? listDishId {  get; set; }
    }
}
