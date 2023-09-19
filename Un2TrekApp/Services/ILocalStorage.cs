namespace Un2TrekApp.Services
{
    public interface ILocalStorage
    {
        Task SetAsync(string key, string value);
        Task<string> GetAsync(string key);
    }
}