using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCore.WebApi.Helper
{
    public static class EnumrableExtens
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource>source,string fields)
        {
            if (source==null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObj=new List<ExpandoObject>(source.Count());

            var propertyInfo = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfo.AddRange(propertyInfos);
            }
            else
            {
                var fieldsSplit = fields.Split(",");

                foreach (var field in fieldsSplit)
                {
                    var trimField = field.Trim();
                    var propertyInfos = typeof(TSource).GetProperty(trimField,
                        BindingFlags.IgnoreCase|BindingFlags.Instance|BindingFlags.Public);

                    if (propertyInfos==null)
                    {
                        throw new Exception($"Property{trimField}没有找到{typeof(TSource)}");
                    }

                    propertyInfo.Add(propertyInfos);

                }
            }

            foreach (var item in source)
            {
                var expands=new ExpandoObject();

                foreach (var property in propertyInfo)
                {
                    var propertyValue = property.GetValue(item);

                    ((IDictionary<string,object>)expands).Add(property.Name,propertyValue);
                }

                expandoObj.Add(expands);
            }

            return expandoObj;
        }
    }
}
