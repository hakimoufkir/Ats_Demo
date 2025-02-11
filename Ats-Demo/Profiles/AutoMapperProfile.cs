using AutoMapper;
using Ats_Demo.Dtos;
using Ats_Demo.Entities;

namespace Ats_Demo.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // Mapping for creating a new employee
            CreateMap<CreateEmployeeDto, Employee>();

            // Mapping for updating an employee
            CreateMap<UpdateEmployeeDto, Employee>();

            // Mapping for retrieving employee details
            CreateMap<Employee, EmployeeDetailsDto>().ReverseMap();

            // General mapping between Employee and EmployeeDto (used in API responses)
            CreateMap<Employee, EmployeeDto>().ReverseMap();
        }
    }
}
