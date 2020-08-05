using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using NetCore.WebApi.Entities;
using NetCore.WebApi.ValidationAttributes;

namespace NetCore.WebApi.Models
{
    [EmployeeAttributes]
    public abstract class EmployeeAdd0RUpdateDto:IValidatableObject
    {
        [Required]
        [DisplayName("员工编号")]
        [StringLength(10,MinimumLength = 5,ErrorMessage = "{0}的长度范围是{1}到{2}")]
        public string EmployeeNo { get; set; }
        [Required]
        [DisplayName("名")]
        public string FirstName { get; set; }
        [Required]
        [DisplayName("姓")]
        public string LastName { get; set; }
        [Required]
        [DisplayName("性别")]
        public Gender Gender { get; set; }
        [Required]
        [DisplayName("出生日期")]
        public DateTime DateOfBirth { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FirstName == LastName)
            {
                yield return new ValidationResult("姓和名不能一样"
                    , new[] { nameof(FirstName), nameof(LastName) });
            }
        }
       
    }
}
