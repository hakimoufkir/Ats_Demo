using Ats_Demo.Domain.Dtos;
using Ats_Demo.Domain.Entities;
using AutoMapper;


namespace Ats_Demo.Application.Profiles
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
