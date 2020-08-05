using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.WebApi.DtoParameters
{
    public class CompanyParametersDto
    {
        private const int MaxPageSize = 20;
        public string CompanyName { get; set; }
        public string Search { get; set; }
        public int PageNum { get; set; } = 1;   

        private int _pageSize=5;

        public string Fields { get; set; }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value> MaxPageSize)? MaxPageSize:value;
        }

        public string OrderBy { get; set; } = "CompanyName";
    }
}
