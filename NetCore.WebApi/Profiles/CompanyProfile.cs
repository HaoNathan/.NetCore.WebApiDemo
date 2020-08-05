using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using NetCore.WebApi.Dto;
using NetCore.WebApi.Entities;
using NetCore.WebApi.Models;

namespace NetCore.WebApi.Profiles
{
    public class CompanyProfile:Profile
    {

        public static Mapper InitializeAutoMapper()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                //给不同属性赋等同值
                mc.CreateMap<Company, CompanyDto>().ForMember(m => m.CompanyName,
                    act => act.MapFrom(m => m.Name));
                mc.CreateMap<CompanyAddDto, Company>();
                mc.CreateMap<EmployeeAddDto, Employee>();
            });


            var mapper = new Mapper(mappingConfig);
            return mapper;

        }

    }
}
