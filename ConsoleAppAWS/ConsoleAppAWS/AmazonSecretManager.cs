
using System.Text;
using Amazon;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Newtonsoft.Json;

namespace ConsoleAppAWS
{
    public class AwsSecretsManager
    {
        private readonly RegionEndpoint _region;
        private AmazonSecretsManagerClient _client;

        public AwsSecretsManager(RegionEndpoint region)
        {
            _region = region;
            _client = new AmazonSecretsManagerClient(_region);
        }

        public async Task<string> GetSecret(string secretName)
        {
            var request = new GetSecretValueRequest
            {
                SecretId = secretName,
                VersionStage = "AWSCURRENT"      // Use the latest version of the secret
            };

            var response = await _client.GetSecretValueAsync(request);

            if (response.SecretString != null)
            {
                // The secret string was retrieved in plain-text
                return response.SecretString;
            }
            else
            {
                // The secret binary blob needs to be decoded
                return Encoding.UTF8.GetString(response.SecretBinary.ToArray());
            }
        }

        public async Task<bool> PutSecret(string secretName, string secretValue)
        {
            var request = new CreateSecretRequest
            {
                Name = secretName,
                SecretString = JsonConvert.SerializeObject(secretValue)
            };

            var response = await _client.CreateSecretAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.Created;
        }

        public async Task<bool> UpdateSecret(string secretName, string secretValue)
        {
            var request = new UpdateSecretRequest
            {
                SecretId = secretName,
                SecretString = JsonConvert.SerializeObject(secretValue)
            };

            var response = await _client.UpdateSecretAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }

        public async Task<bool> DeleteSecret(string secretName)
        {
            var request = new DeleteSecretRequest
            {
                SecretId = secretName
            };

            var response = await _client.DeleteSecretAsync(request);

            return response.HttpStatusCode == System.Net.HttpStatusCode.OK;
        }
    }
}