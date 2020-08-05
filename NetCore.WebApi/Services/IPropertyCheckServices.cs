namespace NetCore.WebApi.Services
{
    public interface IPropertyCheckServices
    {
        public bool HasProperty<T>(string fields);
    }
}