using System;
using AutoMapper;
using NetCore.WebApi.Dto;
using NetCore.WebApi.Entities;
using NetCore.WebApi.Models;

namespace NetCore.WebApi.Profiles
{
    public class EmployeeProfile:Profile
    {
        public static Mapper InitializeAutoMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                //给不同属性赋等同值
                mc.CreateMap<Employee, EmployeeDto>()
                    .ForMember(m => m.Name, 
                        act => act
                            .MapFrom(m => $"{m.FirstName}{m.LastName}"))
                    .ForMember(op=>op.Gender,
                        ac=>ac
                            .MapFrom(m => $"{(m.Gender==Gender.Male?'男':'女')}"))
                    .ForMember(m=>m.Age,
                        ac=>ac
                        .MapFrom(m=>$"{DateTime.Now.Year-m.DateOfBirth.Year}"));

                mc.CreateMap<EmployeeAddDto, Employee>();
                mc.CreateMap<EmployeeUpdateDto, Employee>();
                mc.CreateMap<Employee,EmployeeUpdateDto >();


            });

            var mapper = new Mapper(mappingConfig);
            return mapper;

        }
    }
}
