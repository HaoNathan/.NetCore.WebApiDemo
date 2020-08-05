using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetCore.WebApi.Data;
using NetCore.WebApi.Dto;
using NetCore.WebApi.DtoParameters;
using NetCore.WebApi.Entities;
using NetCore.WebApi.Helper;


namespace NetCore.WebApi.Services
{
    public class CompanyRepository:ICompanyRepository
    {
        private readonly DemoContext _db;
        private readonly IPropertyMappingService _service;

        public CompanyRepository(DemoContext db,IPropertyMappingService service)
        {
            this._db = db;
            this._service = service;
        }

        public async Task<PageList<Company>> GetCompaniesAsync(CompanyParametersDto parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(CompanyRepository));
            }

            var data = _db.Company as IQueryable<Company>;

            if (!string.IsNullOrWhiteSpace(parameters.CompanyName))
            {
                data = data.Where(m => m.Name.Equals(parameters.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(parameters.Search))
            {
                data = data.Where(m => m.Introduction.Contains(parameters.Search)
                                       || m.Country.Contains(parameters.Search)
                                       || m.Name.Contains(parameters.Search));
            }

            var mappingDictionary = _service.GetPropertyMapping<CompanyDto, Company>();

            data = data.ApplySort(parameters.OrderBy,mappingDictionary);

            return await PageList<Company>.Create(data,parameters.PageNum,parameters.PageSize);

        }

        public async Task<Company> GetCompanyAsync(Guid companyId)
        {
            if (companyId==Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _db.Company
                .FirstOrDefaultAsync(m => m.Id.Equals(companyId));
        }

        public async Task<IEnumerable<Company>> GetCompaniesAsync(IEnumerable<Guid> companyIds)
        {
            if (companyIds == null)
            {
                throw new ArgumentNullException(nameof(companyIds));
            }

            return await _db.Company
                .Where(m => companyIds.Contains(m.Id)).ToListAsync();
        }

        public void AddCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }

            _db.Add(company);

        }

        public void UpdateCompany(Company company)
        {
            
        }

        public void DeleteCompany(Company company)
        {
            if (company == null)
            {
                throw new ArgumentNullException(nameof(company));
            }
            _db.Remove(company);
        }

        public async Task<bool> CompanyExistsAsync(Guid companyId)
        {
            if (companyId==Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            return await _db.Company.AnyAsync(m => m.Id.Equals(companyId));
        }

        public async Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId,EmployeeParametersDto parameters)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            var data =  _db.Employee.Where(m => m.CompanyId.Equals(companyId));

            if (!string.IsNullOrWhiteSpace(parameters.Gender))
            {
                var sex = parameters.Gender.Trim();
                var displayGender = Enum.Parse<Gender>(sex);
                data = data.Where(m => m.Gender == displayGender);
            }

            if (!string.IsNullOrWhiteSpace(parameters.Q))
            {
                data = data.Where(m => m.EmployeeNo.Contains(parameters.Q) || m.FirstName.Contains(parameters.Q));
            }

            var mapping = _service.GetPropertyMapping<EmployeeDto, Employee>();

            data = data.ApplySort(parameters.OrderBy, mapping);

            return await data.ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }
            if (employeeId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(employeeId));
            }
            return await _db.Employee.FirstOrDefaultAsync(m => m.CompanyId.Equals(companyId) 
                                                                                && m.Id.Equals(employeeId));
        }

        public void AddEmployee(Guid companyId, Employee employee)
        {
            if (companyId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(companyId));
            }

            if (employee == null)
            {
                throw new ArgumentNullException(nameof(employee));
            }

            employee.CompanyId = companyId;

            _db.Employee.AddAsync(employee);
        }

        public void UpdateEmployee(Employee employee)
        {
            //_db.Employee.Update(employee);
        }

        public void DeleteEmployee(Employee employee)
        {
            if (employee==null)
            {
                throw new ArgumentNullException(nameof(employee));
            }
            _db.Entry(employee).State = EntityState.Deleted;
        }

        public async Task<bool> SaveAsync()
        {
           return await _db.SaveChangesAsync()==1;
        }
    }
}
