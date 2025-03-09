using System.ComponentModel.DataAnnotations;

namespace StoreManageAPI.ViewModels.Dish
{
    public class MenuGroupV
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public int? Order { get; set; }
        public bool Status { get; set; } = true;
    }
}
