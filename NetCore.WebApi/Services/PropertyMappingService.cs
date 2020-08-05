using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetCore.WebApi.Dto;
using NetCore.WebApi.Entities;

namespace NetCore.WebApi.Services
{
    public class PropertyMappingService:IPropertyMappingService
    {

        private readonly Dictionary<string, PropertyMappingValue> _companyPropertyMapping
            = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "Id", new PropertyMappingValue(new List<string> {"Id"})
                },
                {
                    "CompanyName", new PropertyMappingValue(new List<string> {"Name"})
                },
                {
                    "Country", new PropertyMappingValue(new List<string> {"Country"})
                },
                {
                    "Industry", new PropertyMappingValue(new List<string> {"Industry"})
                },
                {
                    "Product", new PropertyMappingValue(new List<string> {"Product"})
                },
                {
                    "CompanyId", new PropertyMappingValue(new List<string> {"Introduction"},true)
                }
            };

        private readonly Dictionary<string, PropertyMappingValue> _employeePropertyMapping
            = new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {
                    "Id", new PropertyMappingValue(new List<string> {"Id"})
                },
                {
                    "Name", new PropertyMappingValue(new List<string> {"FirstName", "LastName"})
                },
                {
                    "EmployeeNo", new PropertyMappingValue(new List<string> {"EmployeeNo"})
                },
                {
                    "Gender", new PropertyMappingValue(new List<string> {"Gender"})
                },
                {
                    "Age", new PropertyMappingValue(new List<string> {"DateOfBirth"})
                },
                {
                    "CompanyId", new PropertyMappingValue(new List<string> {"CompanyId"},true)
                }
            };

        private readonly IList<IPropertyMapping>_propertyMappings=new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<EmployeeDto,Employee>(_employeePropertyMapping));
            _propertyMappings.Add(new PropertyMapping<CompanyDto, Company>(_companyPropertyMapping));
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            var propertyMappings = matchMapping.ToList();

            if (propertyMappings.Count>0)
            {
                return propertyMappings.First().MappingDictionary;
            }

            throw new Exception($"无法找到唯一映射关系:{typeof(TSource)},{typeof(TDestination)}");
        }

        public bool ValidMappingExists<TSource, TDestination>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var mapping = GetPropertyMapping<TSource, TDestination>();

            var fieldForSplit = fields.Split(",");

            foreach (var item in fieldForSplit)
            {
                var trimField = item.Trim();

                var spaceIndex = trimField.IndexOf(" ", StringComparison.Ordinal);

                var propertyName = spaceIndex == -1 ? trimField : trimField.Remove(spaceIndex);

                if (!mapping.ContainsKey(propertyName))
                {
                    return false;
                }
            }

            return true;
        }



    }
}
