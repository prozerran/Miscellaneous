
using Amazon;
using Amazon.CloudFormation;
using Amazon.CloudFormation.Model;

namespace ConsoleAppAWS
{
    public class CloudFormationManager
    {
        private readonly IAmazonCloudFormation _client;

        public CloudFormationManager()
        {
            _client = new AmazonCloudFormationClient(RegionEndpoint.USWest2);
        }

        public async Task<CreateStackResponse> CreateStackAsync(string stackName, string templateBody)
        {
            var request = new CreateStackRequest
            {
                StackName = stackName,
                TemplateBody = templateBody
            };

            return await _client.CreateStackAsync(request);
        }

        public async Task<DeleteStackResponse> DeleteStackAsync(string stackName)
        {
            var request = new DeleteStackRequest
            {
                StackName = stackName
            };

            return await _client.DeleteStackAsync(request);
        }

        public async Task<DescribeStacksResponse> DescribeStacksAsync(string stackName)
        {
            var request = new DescribeStacksRequest
            {
                StackName = stackName
            };

            return await _client.DescribeStacksAsync(request);
        }

        public async Task<UpdateStackResponse> UpdateStackAsync(string stackName, string templateBody)
        {
            var request = new UpdateStackRequest
            {
                StackName = stackName,
                TemplateBody = templateBody
            };

            return await _client.UpdateStackAsync(request);
        }
    }
}