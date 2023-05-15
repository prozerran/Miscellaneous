
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;

namespace ConsoleAppAWS
{
    public class SnsClient
    {
        private readonly string _topicArn;
        private readonly IAmazonSimpleNotificationService _snsClient;

        public SnsClient(string accessKey, string secretKey, string region, string topicArn)
        {
            var config = new AmazonSimpleNotificationServiceConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(region)
            };
            _snsClient = new AmazonSimpleNotificationServiceClient(accessKey, secretKey, config);
            _topicArn = topicArn;
        }

        public async Task PublishAsync(string message)
        {
            var request = new PublishRequest
            {
                TopicArn = _topicArn,
                Message = message
            };
            var response = await _snsClient.PublishAsync(request);
            Console.WriteLine($"MessageId: {response.MessageId}");
        }

        public async Task<string> CreateTopicAsync(string topicName)
        {
            var request = new CreateTopicRequest
            {
                Name = topicName
            };
            var response = await _snsClient.CreateTopicAsync(request);
            return response.TopicArn;
        }

        public async Task SubscribeAsync(string protocol, string endpoint)
        {
            var request = new SubscribeRequest
            {
                Protocol = protocol,
                Endpoint = endpoint,
                TopicArn = _topicArn
            };
            var response = await _snsClient.SubscribeAsync(request);
            Console.WriteLine($"SubscriptionArn: {response.SubscriptionArn}");
        }

        public async Task DeleteTopicAsync()
        {
            var request = new DeleteTopicRequest
            {
                TopicArn = _topicArn
            };
            await _snsClient.DeleteTopicAsync(request);
        }
    }
}