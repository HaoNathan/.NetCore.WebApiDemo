using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using NetCore.WebApi.Dto;
using NetCore.WebApi.DtoParameters;
using NetCore.WebApi.Entities;
using NetCore.WebApi.Helper;
using NetCore.WebApi.Models;
using NetCore.WebApi.Profiles;
using NetCore.WebApi.Services;

namespace NetCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/companies")]
    //[Route("api/[controller]")] 使用的是控制器的名称
    public class CompaniesController:ControllerBase
    {
        private readonly ICompanyRepository _service;
        private readonly IPropertyMappingService _mappingService;
        private readonly IPropertyCheckServices _propertyCheckServices;

        public CompaniesController(ICompanyRepository service,IMapper mapper
            ,IPropertyMappingService mappingService
            ,IPropertyCheckServices propertyCheckServices)
        {
            _service = service;
            _mappingService = mappingService;
            _propertyCheckServices = propertyCheckServices;
        }

        [HttpGet(Name = nameof(GetCompanies))]
        [HttpHead]//不返回body即不返回数据内容
        //使用ActionResult<T>语义更准确            
        public async Task<IActionResult> GetCompanies([FromQuery]CompanyParametersDto parameters)
        {
            if (!_propertyCheckServices.HasProperty<CompanyDto>(parameters.Fields))
            {
                return BadRequest();
            }
            //throw new Exception("error");
            if (!_mappingService.ValidMappingExists<CompanyDto,Company>(parameters.OrderBy))
            {
                return BadRequest();
            }

            var data = await _service.GetCompaniesAsync(parameters);

            var companyDto = CompanyProfile.InitializeAutoMapper().Map<IEnumerable<CompanyDto>>(data);

            var pageNationMetaData = new
            {
                data.PageSize,
                currentPage = data.CurrentPage,
                totalCount=data.Count,
                totalPages=data.TotalPages
            };

            Response.Headers.Add("x-pageNation",JsonSerializer.Serialize(pageNationMetaData,new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            var shapeData = companyDto.ShapeData(parameters.Fields);

            var links = CreateLinksForCompany(parameters,data.HasPrevious,data.HasNext);

            var shapeDataLinks = shapeData.Select(c =>
            {
                var companyDic = c as IDictionary<string, object>;

                var link = CreateLinksForCompany((Guid) companyDic["Id"],null);

                companyDic.Add("Links",link);

                return companyDic;
            });

            var resource = new
            {
                value= shapeDataLinks,
                links
            };

            return Ok(resource);
        }

        [HttpGet("{companyId}",Name = nameof(GetCompany))] //api/Companies/companyId
        //[Route("/{companyId}")]
        public async Task<IActionResult> GetCompany(Guid companyId,string fields,[FromHeader(Name = "Accept")]string mediaType)
        {
            if (!MediaTypeHeaderValue.TryParse(mediaType,out MediaTypeHeaderValue mediaTypeValue))
            {
                return BadRequest();
            }

            if (!_propertyCheckServices.HasProperty<CompanyDto>(fields))
            {
                return BadRequest();
            }

            var company = await _service.GetCompanyAsync(companyId);

            if (company==null)
            {
                return NotFound();
            }

            if (mediaTypeValue.MediaType=="application/vnd.company.hateoas.json")
            {
                var links = CreateLinksForCompany(companyId, fields);

                var resDic = CompanyProfile.InitializeAutoMapper().Map<CompanyDto>(company).ShapeData(fields) as IDictionary<string, object>;

                resDic.Add("Links", links);

                return Ok(resDic);
            }
             
            return Ok(CompanyProfile.InitializeAutoMapper().Map<CompanyDto>(company).ShapeData(fields));
        }

        [HttpPost(Name = nameof(CreateCompany))]
        public async Task<IActionResult> CreateCompany(CompanyAddDto company)
        {
            var res=CompanyProfile.InitializeAutoMapper().Map<Company>(company);

             _service.AddCompany(res);

             await _service.SaveAsync();

             var companyDto = CompanyProfile.InitializeAutoMapper().Map<CompanyDto>(res);

             var links = CreateLinksForCompany(companyDto.Id, null);

             var returnDto = companyDto.ShapeData(null) as IDictionary<string, object>;

             returnDto.Add("Links",links);

            return CreatedAtRoute(nameof(GetCompany), new {companyId = returnDto["Id"]},returnDto);
        }

        [HttpOptions]
        public IActionResult GetCompanyOptions()
        {
            //options请求响应返回,在header返回信息,一般不用这个
            Response.Headers.Add("msg","success");
            return Ok();
        }

        [HttpDelete("{companyId}",Name = nameof(DeleteCompany))]
        public async Task<IActionResult> DeleteCompany(Guid companyId)
        {
            var company = await _service.GetCompanyAsync(companyId);

            if (company==null)
            {
                return NotFound();
            }

            _service.DeleteCompany(company);

            await _service.SaveAsync();

            return NoContent();

        }

        private string CreateCompaniesResourceUri(CompanyParametersDto parameters,ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:

                    return Url.Link(nameof(GetCompanies), new
                    {
                        parameters.Fields,
                        parameters.OrderBy,
                        parameters.PageSize,
                        PageNum = parameters.PageNum - 1,
                        parameters.Search,
                        parameters.CompanyName
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        parameters.Fields,
                        parameters.OrderBy,
                        parameters.PageSize,
                        PageNum = parameters.PageNum + 1,
                        parameters.Search,
                        parameters.CompanyName
                    });

                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        parameters.Fields,
                        parameters.OrderBy,
                        parameters.PageSize,
                        parameters.PageNum,
                        parameters.Search,
                        parameters.CompanyName
                    });

            }
        }

        private IEnumerable<LinkDto> CreateLinksForCompany(Guid companyId, string fields)
        {
            var links = new List<LinkDto>();

            if (string.IsNullOrWhiteSpace(fields))
                links.Add(new LinkDto(Url.Link(nameof(GetCompany)
                    , new {companyId}), "self", "Get"));
            else
                links.Add(new LinkDto(Url.Link(nameof(GetCompany)
                    , new {companyId, fields}), "self", "Get"));

            links.Add(new LinkDto(
                Url.Link(nameof(DeleteCompany), new {companyId})
                , "Delete Company", "Delete"));

            links.Add(new LinkDto(
                Url.Link(nameof(EmployeesController.EmployeeAdd), new {companyId})
                , "create employee for company", "Post"));

            links.Add(new LinkDto(
                Url.Link(nameof(EmployeesController.GetEmployeesForCompany)
                    , new {companyId}), "Get Employees", "Get"));

            return links;
        }

        private IEnumerable<LinkDto> CreateLinksForCompany(CompanyParametersDto parameters, bool hasPrevious,
            bool hasNext)
        {
            var links = new List<LinkDto>();

            links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.CurrentPage), "self", "Get"));

            if (hasPrevious)
            {
                links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage),
                    "previousPage", "Get"));
            }

            if (hasNext)
            {
                links.Add(new LinkDto(CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage), "nextPage",
                    "Get"));
            }

            return links;
        }

    }
}
