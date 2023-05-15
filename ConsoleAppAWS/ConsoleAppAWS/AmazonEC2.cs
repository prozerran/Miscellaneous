
using Amazon;
using Amazon.EC2;
using Amazon.EC2.Model;

namespace ConsoleAppAWS
{
    public class MyEC2Class
    {
        private readonly AmazonEC2Client _client;

        public MyEC2Class()
        {
            //Set up the Amazon EC2 client with appropriate region
            _client = new AmazonEC2Client(RegionEndpoint.USEast1); //Replace with your desired region
        }

        public async Task DescribeInstancesAsync()
        {
            //Create a request object
            DescribeInstancesRequest request = new DescribeInstancesRequest();

            //Send the request and get the response asynchronously
            DescribeInstancesResponse response = await _client.DescribeInstancesAsync(request);

            //Process the response
            foreach (Reservation reservation in response.Reservations)
            {
                foreach (Instance instance in reservation.Instances)
                {
                    Console.WriteLine("Instance ID: {0}", instance.InstanceId);
                }
            }
        }

        //Other methods for starting/stopping instances, etc. can be added here
    }
}
