using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetCore.WebApi.Entities;

namespace NetCore.WebApi.Models
{
    public class CompanyAddDto
    {
        [DisplayName("公司名称")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(12,MinimumLength = 2,ErrorMessage = "{0}的长度范围是{2}到{1}")]
        public string Name { get; set; }
        [DisplayName("所在国家")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0}的长度范围是{2}到{1}")]
        public string Country { get; set; }
        [DisplayName("公司生产类别")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}的长度范围是{2}到{1}")]
        public string Industry { get; set; }
        [DisplayName("公司产品")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "{0}的长度范围是{2}到{1}")]
        public string Product { get; set; }
        [DisplayName("公司简介")]
        [Required(ErrorMessage = "{0}是必填的")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "{0}的长度范围是{2}到{1}")]
        public string Introduction { get; set; }

        public ICollection<EmployeeAddDto> Employees { get; set; }=new List<EmployeeAddDto>();

        
    }
}
