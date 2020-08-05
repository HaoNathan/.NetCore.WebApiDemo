using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using NetCore.WebApi.Services;

namespace NetCore.WebApi.Helper
{
    public static class QuerybleExtentions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T>source,string orderBy
            ,Dictionary<string,PropertyMappingValue>propertyMapping)
        {
            if (source==null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            if (propertyMapping == null)
            {
                throw new ArgumentNullException(nameof(propertyMapping));
            }

            var orderByAfterSplit = orderBy.Split(",");

            foreach (var item in orderByAfterSplit.Reverse())
            {
                var trimOrderBy = item.Trim();

                var orderDescending = trimOrderBy.EndsWith(" desc");

                var spaceOfIndex = trimOrderBy.IndexOf(" ", StringComparison.Ordinal);

                var propertyName=spaceOfIndex==-1?trimOrderBy:trimOrderBy.Remove(spaceOfIndex);

                if (!propertyMapping.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"么有找到名为{propertyName}的key");
                }

                var propertyMappingValue = propertyMapping[propertyName];

                if (propertyMappingValue==null)
                {
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                }

                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    if (propertyMappingValue.Revert)
                    {
                        orderDescending = !orderDescending;
                    }

                    source = source.OrderBy(destinationProperty+(orderDescending ? " descending":" ascending"));
                }
            }

            return source;
        }
    }
}
