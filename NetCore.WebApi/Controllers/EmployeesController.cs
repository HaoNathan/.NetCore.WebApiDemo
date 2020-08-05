using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetCore.WebApi.Dto;
using NetCore.WebApi.DtoParameters;
using NetCore.WebApi.Entities;
using NetCore.WebApi.Models;
using NetCore.WebApi.Profiles;
using NetCore.WebApi.Services;

namespace NetCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/companies/{companyId}/employees")]
    //[ResponseCache(CacheProfileName ="CacheProfile100")]//设置控制器下缓存周期
    public class EmployeesController:ControllerBase
    {
        private readonly ICompanyRepository _service;

        public EmployeesController(ICompanyRepository service)
        {
            _service = service;
        }
        [HttpGet(Name = nameof(GetEmployeesForCompany))]
        //[ResponseCache(Duration = 120)] //设置缓存生命周期，优先级高于控制器
        public async Task<ActionResult<IEnumerable<EmployeeDto>>> 
            GetEmployeesForCompany(Guid companyId, [FromQuery] EmployeeParametersDto parameters)
        {
            
            if (! await _service.CompanyExistsAsync(companyId))
            {
                return NotFound(new{title="2"});
            }

            var data = await _service.GetEmployeesAsync(companyId,parameters);

            var employees = EmployeeProfile.InitializeAutoMapper().Map<IEnumerable<EmployeeDto>>(data);

            return Ok(employees);

        }
        [HttpGet("{employeeId}",Name = nameof(GetEmployeeForCompany))]
        //[HttpCacheExpiration(MaxAge = 10,CacheLocation = CacheLocation.Public)]
        public async Task<ActionResult<EmployeeDto>> GetEmployeeForCompany(Guid companyId,Guid employeeId)
        {

            if (!await _service.CompanyExistsAsync(companyId))
            {
                return NotFound(new { msg = "未找到此资源" });
            }

            var employee = await _service.GetEmployeeAsync(companyId, employeeId);

            if (employee==null)
            {
                return NotFound(new { msg = "未找到此资源" });
            }

            var employees = EmployeeProfile.InitializeAutoMapper().Map<EmployeeDto>(employee);

            return Ok(employees);

        }
        [HttpPost(Name = nameof(EmployeeAdd))]
        public async Task<ActionResult<EmployeeDto>> EmployeeAdd(Guid companyId,EmployeeAddDto employeeAddDto)
        {
            if (!await _service.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = EmployeeProfile.InitializeAutoMapper().Map<Employee>(employeeAddDto);

             _service.AddEmployee(companyId,employee);

             await _service.SaveAsync();

             var employeeDto = EmployeeProfile.InitializeAutoMapper().Map<EmployeeDto>(employee);

            return CreatedAtRoute(nameof(GetEmployeeForCompany), new
            {
                companyId,
                employeeId=employeeDto.Id
            },employeeDto);
        }

        [HttpPut("{employeeId}")]
        public async Task<ActionResult<EmployeeDto>> UpdateEmployee(Guid companyId, Guid employeeId
            , EmployeeUpdateDto employeeUpdate)
        {
            if (!await _service.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await _service.GetEmployeeAsync(companyId, employeeId);

            if (employee == null)
            {
                var res = EmployeeProfile.InitializeAutoMapper().Map<Employee>(employeeUpdate);

                res.Id = employeeId;

                _service.AddEmployee(companyId, res);

                await _service.SaveAsync();

                var employeeDto = EmployeeProfile.InitializeAutoMapper().Map<EmployeeDto>(res);

                return CreatedAtRoute(nameof(GetEmployeeForCompany),
                    new {companyId, employeeId = res.Id}, employeeDto);
            }

            EmployeeProfile.InitializeAutoMapper().Map(employeeUpdate, employee);

            await _service.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{employeeId}")]
        public async Task<ActionResult> PartiallyUpdateEmployee(Guid companyId,Guid employeeId,
            JsonPatchDocument<EmployeeUpdateDto>patchDocument )
        {

            if (!await _service.CompanyExistsAsync(companyId))
            {
                return NotFound();
            }

            var employee = await _service.GetEmployeeAsync(companyId, employeeId);

            if (employee == null)
            {
                var emp=new EmployeeUpdateDto();
                patchDocument.ApplyTo(emp,ModelState);

                if (!TryValidateModel(emp))
                {
                    return ValidationProblem(ModelState);
                }

                var tem = EmployeeProfile.InitializeAutoMapper().Map<Employee>(emp);

                tem.Id = employeeId;

                _service.AddEmployee(companyId,tem);

                await _service.SaveAsync();

                var returnDto = EmployeeProfile.InitializeAutoMapper().Map<EmployeeDto>(tem);

                return CreatedAtRoute(nameof(GetEmployeeForCompany)
                    , new {companyId,employeeId }, returnDto);

            }

            var employeeDto = EmployeeProfile.InitializeAutoMapper().Map<EmployeeUpdateDto>(employee);  

            //patch数据映射
            patchDocument.ApplyTo(employeeDto,ModelState);

            //如果不符合数据约束
            if (!TryValidateModel(employeeDto))
            {
                return ValidationProblem(ModelState);//返回422数据错误
            }

            EmployeeProfile.InitializeAutoMapper().Map(employeeDto,employee);

            await _service.SaveAsync();

            return NoContent();
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployee(Guid companyId, Guid employeeId)
        {
            var employee = await _service.GetEmployeeAsync(companyId, employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            _service.DeleteEmployee(employee);
            await _service.SaveAsync();

            return NoContent();
        }

        //重写ValidationProblem方法使其使用startUp配置中的错误信息提示方法
        public override ActionResult ValidationProblem(ModelStateDictionary modelStateDictionary)
        {
            var option = HttpContext.RequestServices.GetRequiredService<IOptions<ApiBehaviorOptions>>();

            return (ActionResult) option.Value.InvalidModelStateResponseFactory(ControllerContext);
        }
    }
}
