using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using NetCore.WebApi.Entities;

namespace NetCore.WebApi.Dto
{
    public class EmployeeDto
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Company))]
        public Guid CompanyId { get; set; }
        
        public string EmployeeNo { get; set; }
    
        public string Name { get; set; }
   
        public string Gender { get; set; }

        public int Age { get; set; }

     
    }
}
