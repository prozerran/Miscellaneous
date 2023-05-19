using Amazon.Lambda.APIGatewayEvents;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Linq;
using System;
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
                    message = "Hello, This is from Tim Hsu PART 2!"
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

        public APIGatewayCustomAuthorizerResponse Authorize(APIGatewayCustomAuthorizerRequest request, ILambdaContext context)
        {
            var principleID = "test-principal";

            // Perform any authorization logic here to determine whether the user is authorized to access the resource.
            // For this example, we always return "Allow" for demonstration purposes.
            var effect = "Allow";

            var authStr = request.Headers["authorizationToken"];

            if (!string.IsNullOrEmpty(authStr) && authStr.Equals("auth123")) 
            {
                effect = "Allow";
            }           

            var policy = new APIGatewayCustomAuthorizerPolicy
            {
                Version = DateTime.Now.Date.ToString("yyyy-MM-dd"),
                Statement = new List<APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement>()
            };

            policy.Statement[0] = new APIGatewayCustomAuthorizerPolicy.IAMPolicyStatement();
            policy.Statement[0].Effect = effect;
            policy.Statement[0].Action = new HashSet<string> { "execute-api:Invoke" };
            policy.Statement[0].Resource = new HashSet<string> { request.MethodArn };

            //// Create the policy document.
            //var policyDocument = new JObject(
            //    new JProperty("Version", "2012-10-17"),
            //    new JProperty("Statement", new JArray(
            //        new JObject(
            //            new JProperty("Action", "execute-api:Invoke"),
            //            new JProperty("Effect", effect),
            //            new JProperty("Resource", request.MethodArn)
            //        )
            //    ))
            //);

            // Create the response object.
            var response = new APIGatewayCustomAuthorizerResponse
            {
                PrincipalID = principleID,
                PolicyDocument = policy
            };
            return response;
        }

        public async Task<DateTime> GetCurrentTime(ILambdaContext context)
        {
            return await Task.FromResult(DateTime.UtcNow);
        }
    }
}