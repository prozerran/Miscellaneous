
using Amazon.CloudTrail;
using Amazon.CloudTrail.Model;

namespace ConsoleAppAWS
{
    public class CloudTrailExample
    {
        private readonly IAmazonCloudTrail _cloudTrailClient;

        public CloudTrailExample(IAmazonCloudTrail cloudTrailClient)
        {
            _cloudTrailClient = cloudTrailClient;
        }

        public async Task<ListTrailsResponse> ListTrailsAsync()
        {
            ListTrailsRequest request = new ListTrailsRequest();
            ListTrailsResponse response = await _cloudTrailClient.ListTrailsAsync(request);
            return response;
        }

        public async Task<GetTrailStatusResponse> GetTrailStatusAsync(string trailName)
        {
            GetTrailStatusRequest request = new GetTrailStatusRequest()
            {
                Name = trailName
            };
            GetTrailStatusResponse response = await _cloudTrailClient.GetTrailStatusAsync(request);
            return response;
        }
    }
}
