
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using MockApiUnitTest.Configurations;
using MockApiUnitTest.Model;
using MockApiUnitTest.Services;
using MockApiUnitTest.Services.Interfaces;
using Moq;
using RichardSzalay.MockHttp;

namespace MockApiUnitTest.Tests
{
    [UsesVerify]
    public class MockTest : IAsyncDisposable
    {
        private readonly RedisCacheService _redis = TestHelper.CreateTestRedisCacheService();

        private readonly HttpClient _httpClient;
        private readonly SqliteConnection _connection;
        private readonly DbContextOptions<CoreContext> _dbContextOptions;
        private readonly Mock<IRedisCacheService> _redisCacheService;

        public MockTest()
        {
            _connection = new SqliteConnection("Data source=:memory:");
            _connection.Open();

            _dbContextOptions = new DbContextOptionsBuilder<CoreContext>()
                .UseSqlite(_connection)
                .Options;

            using var ctx = new CoreContext(_dbContextOptions);
            ctx.Database.EnsureCreated();

            _httpClient = new HttpClient();
            _redisCacheService = new Mock<IRedisCacheService>();
        }

        [Fact]
        public async Task Test()
        {
            await CreateService().Sync();

            var value = await _redis.HashGetAsync("Key", "en-GB");

            await VerifyJson(value)
                .DontScrubDateTimes()   // for DatetimeOffset
                .DontIgnoreEmptyCollections();
        }

        private ProgramService CreateService()
        {
            var http = CreateMockHttp();

            var serviceConfig = new ServiceConfiguration()
            {
                CmsEndpoint = new CmsEndpoint()
                {
                    BaseUrl = "",
                    Environment = "uat"
                },
                AWS = new AWS()
                {
                    SecretManager = new()
                    {
                        SecretKey = "",
                        AWSAPIKeyValue = "",
                        KeyID = "",
                        ServiceURL = "",
                        AWSAccessToken = ""
                    },
                },
            };

            var dbContext = new CoreContext(_dbContextOptions);
            //return new ProgramService(dbContext, http, serviceConfig, _redisCacheService.Object, NullLogger<ProgramService>.Instance);
            return new ProgramService(dbContext, http, serviceConfig, _redis, NullLogger<ProgramService>.Instance);
        }

        private HttpClient CreateMockHttp()
        {
            var path = Path.Join(TestHelper.ProjectDirectory, $"MockData/expected_response.json");
            var mockHttp = new MockHttpMessageHandler();
            mockHttp
                .When(HttpMethod.Get, "http://www.google.com")
                .WithHeaders("access_token", "cms-token")
                .WithHeaders("api_key", "cms-apikey")
                .Respond("application/json", File.ReadAllText(path));

            return mockHttp.ToHttpClient();
        }

        public async ValueTask DisposeAsync()
        {
            _httpClient.Dispose();
            await _connection.DisposeAsync();
        }
    }
}