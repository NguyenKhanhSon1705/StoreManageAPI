using AutoMapper;
using StoreManageAPI.Models;
using StoreManageAPI.ViewModels.Shopes;
using StoreManageAPI.ViewModels.Stores;
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
        
        }
    }
}
