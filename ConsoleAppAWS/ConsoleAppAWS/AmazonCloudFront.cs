
using Amazon.CloudFront;
using Amazon.CloudFront.Model;

namespace ConsoleAppAWS
{
    public class CloudFrontHelper
    {
        private readonly IAmazonCloudFront _cloudFrontClient;
        private readonly string _distributionId;

        public CloudFrontHelper(string accessKey, string secretKey, string distributionId)
        {
            _cloudFrontClient = new AmazonCloudFrontClient(accessKey, secretKey, Amazon.RegionEndpoint.USEast1);
            _distributionId = distributionId;
        }

        public async Task CreateInvalidationAsync(string path)
        {
            var invalidation = new CreateInvalidationRequest
            {
                DistributionId = _distributionId,
                InvalidationBatch = new InvalidationBatch
                {
                    CallerReference = Guid.NewGuid().ToString(),
                    Paths = new Paths
                    {
                        Quantity = 1,
                        Items = new List<string> { path }
                    }
                }
            };

            var response = await _cloudFrontClient.CreateInvalidationAsync(invalidation);

            if (response.HttpStatusCode == System.Net.HttpStatusCode.Created)
            {
                Console.WriteLine($"Created invalidation for path: {path}");
            }
            else
            {
                Console.WriteLine($"Failed to create invalidation for path: {path}");
            }
        }

        public async Task<string> GetCloudFrontUrlAsync(string s3Key)
        {
            var cloudFrontUrl = "";

            var distributionConfig = await _cloudFrontClient.GetDistributionConfigAsync(new GetDistributionConfigRequest
            {
                Id = _distributionId
            });

            if (distributionConfig.HttpStatusCode == System.Net.HttpStatusCode.OK)
            {
                //var s3Origin = (S3Origin)distributionConfig.DistributionConfig.Origins.Items[0].S3OriginConfig.OriginAccessIdentity;
                var defaultCacheBehavior = distributionConfig.DistributionConfig.DefaultCacheBehavior;
                var viewerProtocolPolicy = defaultCacheBehavior.ViewerProtocolPolicy;

                cloudFrontUrl = $"{viewerProtocolPolicy.ToString().ToLower()}://{defaultCacheBehavior.TargetOriginId}.cloudfront.net/{s3Key}";
            }

            return cloudFrontUrl;
        }
    }
}