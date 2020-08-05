using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace NetCore.WebApi.Entities
{
    public class Company
    {
        
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [DisplayName("姓名")]
        [StringLength(maximumLength:12,MinimumLength = 2,ErrorMessage = "{0}应该介于{1}到{2}字符")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(20)")]
        public string Country { get; set; }

        public string Industry { get; set; }

        public string Product { get; set; }

        public string Introduction { get; set; }

        public DateTime? BankruptTime { get; set; }

        public ICollection<Employee> Employees { get; set; }


    }
}
