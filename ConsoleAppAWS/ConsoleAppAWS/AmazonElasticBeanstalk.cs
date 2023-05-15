
using Amazon.ElasticBeanstalk;
using Amazon.ElasticBeanstalk.Model;

namespace ConsoleAppAWS
{
    class ElasticBeanstalkService
    {
        private readonly IAmazonElasticBeanstalk _elasticBeanstalkClient;

        public ElasticBeanstalkService(IAmazonElasticBeanstalk elasticBeanstalkClient)
        {
            _elasticBeanstalkClient = elasticBeanstalkClient;
        }

        public async Task<List<EnvironmentDescription>> GetEnvironmentsAsync()
        {
            var request = new DescribeEnvironmentsRequest();
            var response = await _elasticBeanstalkClient.DescribeEnvironmentsAsync(request);

            return response.Environments;
        }

        public async Task CreateEnvironmentAsync(string environmentName, string applicationName)
        {
            var request = new CreateEnvironmentRequest
            {
                EnvironmentName = environmentName,
                ApplicationName = applicationName
            };

            await _elasticBeanstalkClient.CreateEnvironmentAsync(request);
        }

        public async Task UploadApplicationAsync(string applicationName, string versionLabel, byte[] fileData)
        {
            var request = new CreateApplicationVersionRequest
            {
                ApplicationName = applicationName,
                VersionLabel = versionLabel,
                SourceBundle = new S3Location
                {
                    S3Bucket = "my-bucket",
                    S3Key = "my-key"
                }
            };

            await _elasticBeanstalkClient.CreateApplicationVersionAsync(request);
        }
    }
}