using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetCore.WebApi.Dto;

namespace NetCore.WebApi.Controllers
{
    [Route("api",Name = nameof(GetRoot))]
    [ApiController]
    public class RootController:ControllerBase
    {
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>
            {
                new LinkDto(Url.Link(nameof(GetRoot), new { }), "self", "Get"),
                new LinkDto(Url.Link(nameof(CompaniesController.GetCompanies), new { }), "Get Companies", "Get"),
                new LinkDto(Url.Link(nameof(CompaniesController.CreateCompany), new { }), "Create Companies",
                    "Post")
            };

            return Ok(links);
        }
    }
}
