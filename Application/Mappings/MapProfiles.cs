using Application.DTO.Group;
using Application.DTOs;
using Application.DTOs.Lesson;
using Application.DTOs.Permission;
using Application.DTOs.Role;
using Application.DTOs.Teacher;
using AutoMapper;
using Domain.Entities.IdentityEntities;

namespace Application.Mappings
{
    public class MapProfiles : Profile
    {
        public MapProfiles()
        {
            CreateMap<Dealership, DealershipBaseDTO>().ReverseMap();
            CreateMap<Dealership, DealershipGetDTO>().ReverseMap()
                .ForMember(desc => desc.SoldCars, t => t.Ignore())
                .ForMember(desc => desc.Cars, t => t.Ignore())
                .ForMember(desc => desc.Monitorings, t => t.Ignore());
            CreateMap<Dealership, DealershipCreateDTO>().ReverseMap();
               
            CreateMap<Dealership, DealershipUpdateDTO>().ReverseMap();


            CreateMap<Role, RoleCreateDTO>().ReverseMap()
            .ForMember(x => x.Permissions, t => t.Ignore())
            .ForMember(x => x.EmployeeRoles, t => t.Ignore());



            CreateMap<RoleUpdateDTO, Role>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RoleId));      
            CreateMap<Role, RoleGetDTO>().ReverseMap()
                .ForMember(x => x.EmployeeRoles, t => t.Ignore());




            CreateMap<PermissionCreateDTO, Permission>().ReverseMap();
            CreateMap<PermissionGetDTO, Permission>().ReverseMap();



            CreateMap<EmployeeCreateDTO, Employee>().ReverseMap();

            CreateMap<EmployeeUpdateDTO, Employee>().ReverseMap();
            CreateMap<Employee, EmployeeGetDTO>()
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(x => x.RoleName).ToArray()));

            CreateMap<Car, CarGetDTO>();
            CreateMap<CarCreateDTO, Car>()
                .ForMember(x => x.SoldCars, t => t.Ignore())
                .ForMember(x => x.Dealership, t => t.Ignore());
            
            CreateMap<SoldCar, SoldCarGetDTO>();
            CreateMap<SoldCarCreateDTO, SoldCar>()
                .ForMember(x => x.Car, t => t.Ignore())
                .ForMember(x => x.Employee, t => t.Ignore());

        }
    }
}
