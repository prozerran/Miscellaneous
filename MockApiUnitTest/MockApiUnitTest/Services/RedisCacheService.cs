using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockApiUnitTest.Configurations;
using MockApiUnitTest.Services.Interfaces;
using Newtonsoft.Json.Linq;
using StackExchange.Redis;

namespace MockApiUnitTest.Services
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IConnectionMultiplexer _redisClient;
        private readonly IServiceConfiguration _servieConfiguraton;

        public RedisCacheService(IConnectionMultiplexer redisClient, IServiceConfiguration servieConfiguraton)
        {
            _redisClient = redisClient;
            _servieConfiguraton = servieConfiguraton;
        }

        public async Task<bool> SetCache(string key, string value, int minutes = 5)
        {
            try
            {
                var db = _redisClient.GetDatabase();
                return await db.StringSetAsync(key, value, TimeSpan.FromMinutes(minutes));
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> GetCache(string key)
        {
            try
            {
                var db = _redisClient.GetDatabase();
                return await db.StringGetAsync(key);
            }
            catch
            {
                return string.Empty;
            }
        }

        public async Task<bool> RemoveCache(string key)
        {
            try
            {
                var db = _redisClient.GetDatabase();
                var rs = await db.StringGetAsync(key);
                if (string.IsNullOrEmpty(rs))
                { 
                    return true; 
                }
                return await db.KeyDeleteAsync(key);
            }
            catch
            {
                return false;
            }
        }

        public List<string> FindAll(string pattern, int pageSize = 100, int pageOffset = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> HashSetAsync(string key, string field, string value)
        {
            var db = _redisClient.GetDatabase();
            return await db.HashSetAsync(key, field, value);
        }

        public async Task<RedisValue> HashGetAsync(string key, string field)
        {
            var db = _redisClient.GetDatabase();
            return await db.HashGetAsync(key, field);
        }
    }
}
