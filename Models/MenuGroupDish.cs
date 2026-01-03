using System.ComponentModel.DataAnnotations.Schema;

namespace StoreManageAPI.Models
{
    public class MenuGroupDish
    {
        public int Dish_Id { get; set; }
        public int Menu_Group_Id { get; set; }
        [ForeignKey("Menu_Group_Id")]
        public MenuGroup? Menu_Group { get; set; }
        [ForeignKey("Dish_Id")]
        public Dish? Dish { get; set; }
    }
}
