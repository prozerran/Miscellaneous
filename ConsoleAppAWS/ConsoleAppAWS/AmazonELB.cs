
using Amazon.ElasticLoadBalancingV2;
using Amazon.ElasticLoadBalancingV2.Model;

namespace ConsoleAppAWS
{
    public class ElasticLoadBalancer
    {
        private readonly AmazonElasticLoadBalancingV2Client client;

        public ElasticLoadBalancer()
        {
            client = new AmazonElasticLoadBalancingV2Client();
        }

        public async Task<List<LoadBalancer>> GetLoadBalancersAsync()
        {
            var request = new DescribeLoadBalancersRequest();
            var response = await client.DescribeLoadBalancersAsync(request);
            return response.LoadBalancers;
        }

        public async Task<string?> RegisterInstanceWithLoadBalancerAsync(string loadBalancerArn, string instanceId)
        {
            var request = new RegisterTargetsRequest
            {
                TargetGroupArn = loadBalancerArn,
                Targets = new List<TargetDescription>
                {
                    new TargetDescription
                    {
                        Id = instanceId
                    }
                }
            };

            var response = await client.RegisterTargetsAsync(request);
            return response.ToString();
        }
    }
}