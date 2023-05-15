
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;

namespace ConsoleAppAWS
{
    public class AwsS3Example
    {
        private IAmazonS3 _s3Client;

        public AwsS3Example()
        {
            // add credentials here
            _s3Client = new AmazonS3Client();
        }

        public async Task UploadFileAsync(string bucketName, string filePath, string fileName)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.UploadAsync(filePath, bucketName, fileName);
        }

        public async Task DownloadFileAsync(string bucketName, string keyName, string filePath)
        {
            var fileTransferUtility = new TransferUtility(_s3Client);
            await fileTransferUtility.DownloadAsync(filePath, bucketName, keyName);
        }

        public async Task DeleteFileAsync(string bucketName, string keyName)
        {
            var deleteObjectRequest = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = keyName
            };
            await _s3Client.DeleteObjectAsync(deleteObjectRequest);
        }

        public async Task CopyFileAsync(string sourceBucketName, string sourceKeyName, string destinationBucketName, string destinationKeyName)
        {
            var copyObjectRequest = new CopyObjectRequest
            {
                SourceBucket = sourceBucketName,
                SourceKey = sourceKeyName,
                DestinationBucket = destinationBucketName,
                DestinationKey = destinationKeyName
            };
            await _s3Client.CopyObjectAsync(copyObjectRequest);
        }

        public async Task MoveFileAsync(string sourceBucketName, string sourceKeyName, string destinationBucketName, string destinationKeyName)
        {
            await CopyFileAsync(sourceBucketName, sourceKeyName, destinationBucketName, destinationKeyName);
            await DeleteFileAsync(sourceBucketName, sourceKeyName);
        }

        public async Task CreateBucketAsync(string bucketName)
        {
            var putBucketRequest = new PutBucketRequest
            {
                BucketName = bucketName,
                UseClientRegion = true
            };
            await _s3Client.PutBucketAsync(putBucketRequest);
        }

        public async Task DeleteBucketAsync(string bucketName)
        {
            var deleteBucketRequest = new DeleteBucketRequest
            {
                BucketName = bucketName
            };
            await _s3Client.DeleteBucketAsync(deleteBucketRequest);
        }
    }
}
