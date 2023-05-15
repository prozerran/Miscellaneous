
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;

namespace ConsoleAppAWS
{
    public class DynamoDBService
    {
        private readonly IAmazonDynamoDB _dynamoDBClient;

        public DynamoDBService()
        {
            _dynamoDBClient = new AmazonDynamoDBClient();
        }

        public DynamoDBService(IAmazonDynamoDB dynamoDBClient)
        {
            _dynamoDBClient = dynamoDBClient;
        }

        public async Task CreateTableAsync(string tableName, List<AttributeDefinition> attributes, List<KeySchemaElement> keySchema, ProvisionedThroughput provisionedThroughput)
        {
            var request = new CreateTableRequest
            {
                TableName = tableName,
                AttributeDefinitions = attributes,
                KeySchema = keySchema,
                ProvisionedThroughput = provisionedThroughput
            };

            await _dynamoDBClient.CreateTableAsync(request);
        }

        public async Task<ScanResponse> ScanAsync(string tableName, string filterExpression, Dictionary<string, AttributeValue> expressionAttributeValues)
        {
            var request = new ScanRequest
            {
                TableName = tableName,
                FilterExpression = filterExpression,
                ExpressionAttributeValues = expressionAttributeValues
            };

            return await _dynamoDBClient.ScanAsync(request);
        }

        public async Task<QueryResponse> QueryAsync(string tableName, string keyConditionExpression, Dictionary<string, AttributeValue> expressionAttributeValues)
        {
            var request = new QueryRequest
            {
                TableName = tableName,
                KeyConditionExpression = keyConditionExpression,
                ExpressionAttributeValues = expressionAttributeValues
            };

            return await _dynamoDBClient.QueryAsync(request);
        }

        public async Task<PutItemResponse> PutItemAsync(string tableName, Document item)
        {
            var request = new PutItemRequest
            {
                TableName = tableName,
                Item = item.ToAttributeMap()
            };

            return await _dynamoDBClient.PutItemAsync(request);
        }

        public async Task<GetItemResponse> GetItemAsync(string tableName, Dictionary<string, AttributeValue> key)
        {
            var request = new GetItemRequest
            {
                TableName = tableName,
                Key = key
            };

            return await _dynamoDBClient.GetItemAsync(request);
        }
    }
}