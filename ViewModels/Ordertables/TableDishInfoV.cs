using StoreManageAPI.ViewModels.Shopes;

namespace StoreManageAPI.ViewModels.Ordertables
{
    public class TableDishInfoV : TableByAreaV
    {
        public IList<DishInfoV>? dish { get; set; }
    }
}
