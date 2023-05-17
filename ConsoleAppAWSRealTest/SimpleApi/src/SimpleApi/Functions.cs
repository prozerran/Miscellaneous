using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using System.Net;
using System.Text.Json;

// https://www.youtube.com/watch?v=rImaNyfKhZk

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace SimpleApi
{
    public class Functions
    {
        /// <summary>
        /// Default constructor that Lambda will invoke.
        /// </summary>
        public Functions()
        {
        }

        /// <summary>
        /// A Lambda function to respond to HTTP Get methods from API Gateway
        /// </summary>
        /// <param name="request"></param>
        /// <returns>The API Gateway response.</returns>
        public APIGatewayProxyResponse Get(APIGatewayProxyRequest request, ILambdaContext context)
        {
            context.Logger.LogInformation("Get Request\n");

            var query = request.QueryStringParameters;

            var rsp = new
            {
                query,
                message = "Hello, This is from Tim Hsu!"
            };

            var response = new APIGatewayProxyResponse
            {
                StatusCode = (int)HttpStatusCode.OK,
                //Body = "Hello AWS Serverless",
                Body = JsonSerializer.Serialize(rsp),
                Headers = new Dictionary<string, string> { { "Content-Type", "text/plain" } }
            };

            return response;
        }

        public async Task<DateTime> GetCurrentTime(ILambdaContext context)
        {
            return await Task.FromResult(DateTime.UtcNow);
        }
    }
}