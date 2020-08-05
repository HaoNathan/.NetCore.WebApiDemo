using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetCore.WebApi.Entities;
using NetCore.WebApi.ValidationAttributes;

namespace NetCore.WebApi.Models
{

    //[EmployeeAttributes(ErrorMessage = "error!员工名不能等于名")] 自定义
    public class EmployeeAddDto:EmployeeAdd0RUpdateDto
    {
       
    }
}