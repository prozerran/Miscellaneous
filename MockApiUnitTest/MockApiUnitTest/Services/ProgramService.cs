using Microsoft.Extensions.Logging;
using MockApiUnitTest.Configurations;
using MockApiUnitTest.Model;
using MockApiUnitTest.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MockApiUnitTest.Services
{
    public class ProgramService
    {
        private static readonly string DefaultLocaleCode = "en-GB";

        private static readonly IReadOnlyDictionary<string, string> LocaleCodeMappings = new Dictionary<string, string>
        {
            { "en-us", DefaultLocaleCode },
            { "zh-cn", "zh-CN" },
            { "zh-hk", "zh-HK" },
            { "zh-jp", "zh-JP" },
            { "zh-th", "zh-TH" },
            { "zh-vn", "zh-VN" },
        };

        private static readonly IReadOnlySet<string> LocaleCodes = LocaleCodeMappings.Keys.ToHashSet();

        private readonly CoreContext _coreContext;
        private readonly HttpClient _httpClient;
        private readonly IRedisCacheService _redis;
        private readonly ILogger<ProgramService> _logger;
        private readonly IServiceConfiguration _configuration;

        public ProgramService(CoreContext coreContext, HttpClient httpClient, IServiceConfiguration configuration, IRedisCacheService redis, ILogger<ProgramService> logger)
        {
            _coreContext = coreContext;
            _httpClient = httpClient;
            _configuration = configuration;
            _redis = redis;
            _logger = logger;
        }

        public async Task Sync()
        {
            _logger.LogInformation("Start");

            var data = await FetchData();

            await UpdateDataToRedis(data);
            
            _logger.LogInformation("End");
        }

        private async Task<Dictionary<string, List<Data>>> FetchData()
        {
            var dic = new Dictionary<string, List<Data>>();
            foreach (var locale in LocaleCodes)
            {
                var url = $"http://www.contentstack.com/data_article/entries?enviornment=uat&include[]=data_articles&locale={locale}";
                using var request = new HttpRequestMessage(HttpMethod.Get, url);

                //request.Headers.Add("Accept", "application/json");
                request.Headers.Add("api_key", _configuration.AWS.SecretManager.AWSAPIKeyValue);
                request.Headers.Add("access_token", _configuration.AWS.SecretManager.AWSAccessToken);

                using var response = await _httpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());

                var entries = json.RootElement.GetProperty("employees").EnumerateArray();
                entries.MoveNext();

                var result = entries.Current.GetProperty("result").Deserialize<List<Data>>()!;
                dic.Add(LocaleCodeMappings[locale], result);
            }
            return dic;
        }

        private async Task UpdateDataToRedis(Dictionary<string, List<Data>> data)
        {
            foreach (var (lang, dt) in data)
            {
                var redisData = dt
                    .Select(data => new DataRedis
                    {
                        Uid = data.Uid,
                        Name = data.Name,
                        Age = data.Age,
                        Company = data.Company,
                        Sex = data.Sex,
                        Website = data.Website
                    }).ToList();

                await _redis.HashSetAsync("CMS-KEY", lang, JsonConvert.SerializeObject(redisData));
            }
        }

        private record Data
        {
            [JsonPropertyName("uid")]
            public string Uid { get; init; } = null!;

            [JsonPropertyName("name")]
            public string Name { get; init; } = null!;

            [JsonPropertyName("age")]
            public int Age { get; init; } = 0;

            [JsonPropertyName("company")]
            public string Company { get; init; } = null!;

            [JsonPropertyName("sex")]
            public string Sex { get; init; } = null!;

            [JsonPropertyName("website")]
            public List<WebSite> Website { get; init; } = null!;
        }
    }
}
