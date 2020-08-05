using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore.WebApi.Entities
{
    public class Employee
    {

        public Guid Id { get; set; }=Guid.NewGuid();
        
        public Guid CompanyId { get; set; }

        [Required]
        [DisplayName("员工编号")]
        [StringLength(maximumLength:12,ErrorMessage = "{0}长度不能超过{1}")]
        public string EmployeeNo { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
     
        public Gender Gender { get; set; }

        public DateTime DateOfBirth { get; set; }
        
        public Company Company { get; set; }
    }
}