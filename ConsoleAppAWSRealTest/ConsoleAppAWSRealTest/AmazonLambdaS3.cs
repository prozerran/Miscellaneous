using System;
using System.IO;
using System.Threading.Tasks;
using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.Lambda.Serialization.Json;
using Amazon.S3;
using Amazon.S3.Model;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace ConsoleAppAWSRealTest
{
    public class AmazonLambdaS3
    {
        private static readonly IAmazonS3 s3Client = new AmazonS3Client();

        public async Task<string> FunctionHandler(S3Event evnt, ILambdaContext context)
        {
            var s3Event = evnt.Records?[0].S3;
            if (s3Event == null)
            {
                return null;
            }

            var getObjectRequest = new GetObjectRequest
            {
                BucketName = s3Event.Bucket.Name,
                Key = s3Event.Object.Key
            };

            using var response = await s3Client.GetObjectAsync(getObjectRequest);

            if (response == null)
            {
                return null;
            }

            using var reader = new StreamReader(response.ResponseStream);
            var content = await reader.ReadToEndAsync();

            return content;
        }

        [LambdaSerializer(typeof(JsonSerializer))]
        public string HandleRequest(object evt, ILambdaContext context)
        {
            context.Logger.LogLine(evt.ToString());
            return "Hello from Lambda!";
        }

        [LambdaSerializer(typeof(JsonSerializer))]
        public async Task<DateTime> GetCurrentDateTime(ILambdaContext context)
        {
            return await Task.FromResult(DateTime.UtcNow);
        }
    }
}