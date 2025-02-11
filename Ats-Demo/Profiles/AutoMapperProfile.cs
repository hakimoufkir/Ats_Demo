using AutoMapper;

namespace Ats_Demo.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Ats_Demo.Entities.Employee, Ats_Demo.Dtos.EmployeeDto>().ReverseMap();
        }
    }
}
