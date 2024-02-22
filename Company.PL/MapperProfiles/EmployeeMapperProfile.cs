using AutoMapper;
using Company.DAL.Model;
using Company.PL.ViewModels;

namespace Company.PL.MapperProfiles
{
    public class EmployeeMapperProfile:Profile
    {
        public EmployeeMapperProfile()
        {
            CreateMap<EmployeeViewModel,Employee>().ReverseMap();
        }
    }
}
