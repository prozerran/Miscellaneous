
using Amazon.Redshift;
using Amazon.Redshift.Model;

namespace ConsoleAppAWS
{
    public class RedshiftExample
    {
        private readonly IAmazonRedshift redshiftClient;

        public RedshiftExample(IAmazonRedshift redshiftClient)
        {
            this.redshiftClient = redshiftClient;
        }

        public async Task<string> GetClusterStatus(string clusterId)
        {
            var request = new DescribeClustersRequest()
            {
                ClusterIdentifier = clusterId
            };

            var response = await redshiftClient.DescribeClustersAsync(request);

            if (response.Clusters.Count == 0)
            {
                throw new Exception($"Cluster {clusterId} not found");
            }

            return response.Clusters[0].ClusterStatus;
        }
    }
}