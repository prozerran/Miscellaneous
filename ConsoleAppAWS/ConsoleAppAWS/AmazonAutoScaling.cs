
using Amazon.AutoScaling;
using Amazon.AutoScaling.Model;

namespace ConsoleAppAWS
{
    class AutoScalingExample
    {
        private readonly IAmazonAutoScaling _autoScalingClient;

        public AutoScalingExample()
        {
            _autoScalingClient = new AmazonAutoScalingClient();
        }

        public async Task<DescribeAutoScalingGroupsResponse> DescribeAutoScalingGroupsAsync()
        {
            var request = new DescribeAutoScalingGroupsRequest();

            return await _autoScalingClient.DescribeAutoScalingGroupsAsync(request);
        }

        public async Task<CreateAutoScalingGroupResponse> CreateAutoScalingGroupAsync()
        {
            var request = new CreateAutoScalingGroupRequest
            {
                AutoScalingGroupName = "my-auto-scaling-group",
                LaunchConfigurationName = "my-launch-configuration",
                MinSize = 1,
                MaxSize = 10,
                VPCZoneIdentifier = "subnet-123456"
            };

            return await _autoScalingClient.CreateAutoScalingGroupAsync(request);
        }

        public async Task<DeleteAutoScalingGroupResponse> DeleteAutoScalingGroupAsync(string autoScalingGroupName)
        {
            var request = new DeleteAutoScalingGroupRequest
            {
                AutoScalingGroupName = autoScalingGroupName
            };

            return await _autoScalingClient.DeleteAutoScalingGroupAsync(request);
        }
    }
}