using Amazon.Lambda.Core;
using Amazon.Lambda;
using Amazon.Lambda.Model;
using System.Text;

// protocol://service-code.region-code.amazonaws.com
// For example, https://dynamodb.us-west-2.amazonaws.com

namespace ConsoleAppAWSRealTest
{
    public class AmazonLambda
    {
        private static string key = "AKIAZKESX5QVG4TIJU5U";
        private static string secret = "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";

        public async Task TestMyLambda()
        {
            var lambdaClient = new AmazonLambdaClient(key, secret);
            var request = new InvokeRequest()
            {
                FunctionName = "arn:aws:lambda:ap-southeast-1:640257879082:function:MyLambdaTest",
                InvocationType = InvocationType.RequestResponse,
                Payload = "{ \"myKey\": \"myValue\" }"
            };

            InvokeResponse response = await lambdaClient.InvokeAsync(request);

            if (response.StatusCode == 200)
            {
                string payload = Encoding.ASCII.GetString(response.Payload.ToArray());
                // Do something with the payload
            }
        }
    }
}