using System.Collections.Generic;

namespace NetCore.WebApi.Services
{
    public interface IPropertyMappingService
    {
        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExists<TSource, TDestination>(string fields);
    }
}