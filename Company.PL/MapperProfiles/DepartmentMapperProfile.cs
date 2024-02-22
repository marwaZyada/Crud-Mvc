using AutoMapper;
using Company.DAL.Model;
using Company.PL.ViewModels;

namespace Company.PL.MapperProfiles
{
    public class DepartmentMapperProfile:Profile
    {
        public DepartmentMapperProfile()
        {
            CreateMap<DepartmentViewModel, Department>().ReverseMap();
        }
    }
}
