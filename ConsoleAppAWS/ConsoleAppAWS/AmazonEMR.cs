
using Amazon.ElasticMapReduce;
using Amazon.ElasticMapReduce.Model;

namespace ConsoleAppAWS
{
    public class EMRClient
    {
        private readonly IAmazonElasticMapReduce _emrClient;

        public EMRClient()
        {
            _emrClient = new AmazonElasticMapReduceClient();
        }

        public async Task<string> AddJobFlowAsync(string name)
        {
            RunJobFlowRequest request = new RunJobFlowRequest
            {
                Name = name,
                Instances = new JobFlowInstancesConfig
                {
                    InstanceCount = 2,
                    MasterInstanceType = "m3.xlarge",
                    SlaveInstanceType = "m3.xlarge"
                }
            };

            RunJobFlowResponse response = await _emrClient.RunJobFlowAsync(request);
            return response.JobFlowId;
        }
    }
}
