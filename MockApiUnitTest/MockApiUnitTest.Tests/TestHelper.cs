
using Neutral.TestingUtilities.RedisInMemory;
using NSubstitute;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MockApiUnitTest.Services;

namespace MockApiUnitTest.Tests
{
    public class TestHelper
    {
        public static readonly string ProjectDirectory = Path.Join(Directory.GetCurrentDirectory(), "../../..");

        public static RedisCacheService CreateTestRedisCacheService()
        {
            var database = new RedisDatabaseInMemory(Guid.NewGuid().ToString());
            var mockConnectionMultiplexer = Substitute.For<IConnectionMultiplexer>();
            mockConnectionMultiplexer.GetDatabase().Returns(database);
            return new RedisCacheService(mockConnectionMultiplexer, null);
        }
    }
}
