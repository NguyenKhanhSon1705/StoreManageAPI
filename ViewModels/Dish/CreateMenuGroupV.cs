namespace StoreManageAPI.ViewModels.Dish
{
    public class CreateMenuGroupV
    {
        public int? Id  { get; set; }
        public int? ShopId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; } = "";
        public IFormFile? Image { get; set; }
        public int? Order { get; set; }
        public bool Status { get; set; } = true;
    }
}
