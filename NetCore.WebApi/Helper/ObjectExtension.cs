using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace NetCore.WebApi.Helper
{
    public static class ObjectExtension
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            if (source==null)
            {
                    throw  new ArgumentNullException(nameof(source));
            }

            var expandoObj=new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos =
                    typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Instance |
                                                  BindingFlags.Public);

                foreach (var propertyInfo in propertyInfos)
                {
                    var propertyValue = propertyInfo.GetValue(source);

                    ((IDictionary<string, object>) expandoObj).Add(propertyInfo.Name, propertyValue);
                }
            }
            else
            {
                var properties = fields.Split(",");

                foreach (var field in properties)
                {
                    var propertyTrim = field.Trim();

                    var propertyInfos = typeof(TSource).GetProperty(propertyTrim,BindingFlags.IgnoreCase|BindingFlags.Instance|BindingFlags.Public);

                    if (propertyInfos==null)
                    {
                        throw new Exception($"property{propertyTrim}在{typeof(TSource)}没找到");
                    }

                    var propertyValue = propertyInfos.GetValue(source);

                    ((IDictionary<string,object>)expandoObj).Add(propertyInfos.Name,propertyValue);
                }
            }

            return expandoObj;
        }
    }
}
