using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.WebApi.Dto;
using NetCore.WebApi.Entities;
using NetCore.WebApi.Helper;
using NetCore.WebApi.Models;
using NetCore.WebApi.Profiles;
using NetCore.WebApi.Services;

namespace NetCore.WebApi.Controllers
{
    [ApiController]
    [Route("api/companyCollections")]
    public class CompanyCollectionsController:ControllerBase
    {
        private readonly ICompanyRepository _service;

        public CompanyCollectionsController(ICompanyRepository service)
        {
            _service = service;
        }

        [HttpGet("{ids}", Name = nameof(GetCompanyCollection))]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanyCollection(
            [FromRoute][ModelBinder(typeof(ArrayModelBinder))]IEnumerable<Guid>ids)
        {
            if (ids==null)
            {
                return BadRequest();
            }

            var entities = await _service.GetCompaniesAsync(ids);

            if (entities.Count()!=ids.Count())
            {
                return NotFound();
            }

            var companies = CompanyProfile.InitializeAutoMapper().Map<IEnumerable<CompanyDto>>(entities);

            return Ok(companies);

        }
        
        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreateCompanyCollections(IEnumerable<CompanyAddDto> companies)
        {
            var companyEntities= CompanyProfile.InitializeAutoMapper().Map<IEnumerable<Company>>(companies);

            foreach (var company in companyEntities)
            {
                _service.AddCompany(company);
            }

            await _service.SaveAsync();

            var data = CompanyProfile.InitializeAutoMapper().Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(",", data.Select(m=>m.Id));

            return CreatedAtRoute(nameof(GetCompanyCollection), new {ids}, data);
        }


    }
}
