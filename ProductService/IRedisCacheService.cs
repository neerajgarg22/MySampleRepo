namespace ProductService
{
    public interface IRedisCacheService
    {
        T? GetData<T>(string key);
        void SetData<T>(string key, T value);

        void RemoveData<T>(string key);
    }
}
