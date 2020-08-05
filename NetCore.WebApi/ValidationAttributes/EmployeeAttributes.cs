using System.ComponentModel.DataAnnotations;
using NetCore.WebApi.Models;

namespace NetCore.WebApi.ValidationAttributes
{
    public class EmployeeAttributes:ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var employee = (EmployeeAdd0RUpdateDto) validationContext.ObjectInstance;

            if (employee.EmployeeNo==employee.FirstName)
            {
                //return new ValidationResult(ErrorMessage, new[] { nameof(EmployeeAddDto) });自定义error消息
                return new ValidationResult("员工编号不能等于名",new[]{nameof(EmployeeAdd0RUpdateDto) });
            }

            return  ValidationResult.Success;
        }
    }
}
