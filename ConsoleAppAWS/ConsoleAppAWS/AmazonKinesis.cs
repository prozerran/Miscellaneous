
using Amazon.Kinesis;
using Amazon.Kinesis.Model;

namespace ConsoleAppAWS
{
    public class KinesisStreamReader
    {
        private readonly string _streamName;
        private readonly IAmazonKinesis _kinesisClient;

        public KinesisStreamReader(string streamName, IAmazonKinesis kinesisClient)
        {
            _streamName = streamName;
            _kinesisClient = kinesisClient;
        }

        public async Task<List<Record>> ReadRecordsAsync(int limit)
        {
            var request = new GetRecordsRequest
            {
                ShardIterator = await GetShardIteratorAsync(),
                Limit = limit
            };

            var response = await _kinesisClient.GetRecordsAsync(request);
            return response.Records;
        }

        private async Task<string> GetShardIteratorAsync()
        {
            var request = new GetShardIteratorRequest
            {
                StreamName = _streamName,
                ShardId = "shardId-000000000001", // Replace with your shard ID
                ShardIteratorType = ShardIteratorType.LATEST
            };

            var response = await _kinesisClient.GetShardIteratorAsync(request);
            return response.ShardIterator;
        }
    }
}