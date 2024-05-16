namespace User.Services.Interfaces
{
    public interface IRedisService
    {
        void SetValue(string key, string value, TimeSpan? expiry = null);
        string GetValue(string key);
        void RemoveValue(string key);
    }
}
