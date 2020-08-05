using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.WebApi.DtoParameters;
using NetCore.WebApi.Entities;
using NetCore.WebApi.Helper;

namespace NetCore.WebApi.Services
{
    public interface ICompanyRepository
    {
        //Task<PagedList<Company>> GetCompaniesAsync(CompanyDtoParameters parameters);
        Task<PageList<Company>> GetCompaniesAsync(CompanyParametersDto parameters);
        Task<Company> GetCompanyAsync(Guid companyId);
        Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds);
        void AddCompany(Company company);
        void UpdateCompany(Company company);
        void DeleteCompany(Company company);
        Task<bool> CompanyExistsAsync(Guid companyId);

        //Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, EmployeeDtoParameters parameters);
        Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,EmployeeParametersDto parameters);
        Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId);
        void AddEmployee(Guid companyId, Employee employee);
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(Employee employee);

        Task<bool> SaveAsync();
    }
}
