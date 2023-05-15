
using Amazon.ElastiCache;
using Amazon.ElastiCache.Model;

namespace ConsoleAppAWS
{
    class ElastiCacheExample
    {
        static AmazonElastiCacheClient client = new AmazonElastiCacheClient();

        public static async Task<DescribeCacheClustersResponse> DescribeCacheClustersAsync()
        {
            var request = new DescribeCacheClustersRequest();
            return await client.DescribeCacheClustersAsync(request);
        }

        public static async Task CreateCacheClusterAsync(string clusterId)
        {
            var request = new CreateCacheClusterRequest
            {
                CacheClusterId = clusterId,
                CacheNodeType = "cache.t2.micro",
                Engine = "redis",
                NumCacheNodes = 1
            };
            await client.CreateCacheClusterAsync(request);
        }

        public static async Task DeleteCacheClusterAsync(string clusterId)
        {
            var request = new DeleteCacheClusterRequest
            {
                CacheClusterId = clusterId
            };
            await client.DeleteCacheClusterAsync(request);
        }
    }
}