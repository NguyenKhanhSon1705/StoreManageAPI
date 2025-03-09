using AutoMapper;
using StoreManageAPI.DTO.Dish;
using StoreManageAPI.DTO.Payment;
using StoreManageAPI.Models;
using StoreManageAPI.ViewModels.Dish;
using StoreManageAPI.ViewModels.Shopes;
using StoreManageAPI.ViewModels.UserManager;

namespace StoreManageAPI.Config
{
    public class AutoMapper : Profile
    {
        public AutoMapper() 
        {
            CreateMap<User, UserInfoV>().ReverseMap();
            CreateMap<User, CreateUserV>().ReverseMap();
            CreateMap<Shop, GetShopV>().ReverseMap();
            CreateMap<Tables , TableInfoV>().ReverseMap();
            CreateMap<MenuGroup, MenuGroupV>().ReverseMap();
            CreateMap<Dish, CreateDishV>().ReverseMap();
            CreateMap<Dish, DishV>().ReverseMap();
            CreateMap<Transactions, TransactionsDTO>().ReverseMap();

            CreateMap<DishPriceVersion , PriceDishInfoDTO>()
                .ForMember(dest => dest.price_id, opt => opt.MapFrom(src => src.id))
                .ReverseMap()
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.price_id));
        }
    }
}
