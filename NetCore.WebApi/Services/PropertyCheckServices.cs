using System.Reflection;

namespace NetCore.WebApi.Services
{
    public class PropertyCheckServices : IPropertyCheckServices
    {
        public bool HasProperty<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var splitFields = fields.Split(",");

            foreach (var field in splitFields)
            {
                var fieldTrim = field.Trim();

                var propertyInfo = typeof(T).GetProperty(field,
                    BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

                if (propertyInfo==null)
                {
                    return false;
                }

            }

            return true;
        }
    }
}
