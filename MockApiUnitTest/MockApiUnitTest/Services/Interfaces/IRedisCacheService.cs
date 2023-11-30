using StackExchange.Redis;

namespace MockApiUnitTest.Services.Interfaces
{
    public interface IRedisCacheService
    {
        Task<bool> SetCache(string key, string value, int minutes = 5);
        Task<string> GetCache(string key);
        Task<bool> RemoveCache(string key);
        List<string> FindAll(string pattern, int pageSize = 100, int pageOffset = 0);
        Task<bool> HashSetAsync(string key, string field, string value);
        Task<RedisValue> HashGetAsync(string key, string field);
    }
}