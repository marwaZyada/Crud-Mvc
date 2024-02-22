using AutoMapper;
using Company.DAL.Model;
using Company.PL.ViewModels;

namespace Company.PL.MapperProfiles
{
    public class UserMapperProfile:Profile
    {
        public UserMapperProfile()
        {
            CreateMap<ApplicationUser,UserViewModel>().ReverseMap();
        }
    }
}
