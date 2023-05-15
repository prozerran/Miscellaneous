
using Amazon.Glacier;
using Amazon.Glacier.Model;
using Amazon.Glacier.Transfer;

namespace AmazonGlacierDemo
{
    public class GlacierTransfer
    {
        private readonly string vaultName;
        private readonly string filePath;

        public GlacierTransfer(string vaultName, string filePath)
        {
            this.vaultName = vaultName;
            this.filePath = filePath;
        }

        public async Task<string> UploadArchiveAsync(string archiveDescription)
        {
            using var amazonGlacierClient = new AmazonGlacierClient();

            using var fileStream = new FileStream(filePath, FileMode.Open);

            var glacierUploader = new ArchiveTransferManager(amazonGlacierClient);

            var uploadResult = await glacierUploader.UploadAsync(vaultName, archiveDescription, fileStream.ToString()).ConfigureAwait(false);

            return uploadResult.ArchiveId;
        }

        public async Task DownloadArchiveAsync(string archiveId, string downloadPath)
        {
            using var amazonGlacierClient = new AmazonGlacierClient();

            var describeJobRequest = new DescribeJobRequest
            {
                VaultName = vaultName,
                JobId = await GetJobIdForArchiveIdAsync(archiveId).ConfigureAwait(false)
            };

            var describeJobResponse = await amazonGlacierClient.DescribeJobAsync(describeJobRequest).ConfigureAwait(false);

            //while (!describeJobResponse.JobOutput.HasChecksum || !describeJobResponse.JobOutput.HasArchiveSHA256TreeHash)
            //{
            //    await Task.Delay(TimeSpan.FromSeconds(30)).ConfigureAwait(false);

            //    describeJobResponse = await amazonGlacierClient.DescribeJobAsync(describeJobRequest).ConfigureAwait(false);
            //}

            using var fileStream = new FileStream(downloadPath, FileMode.Create);

            var getJobOutputRequest = new GetJobOutputRequest
            {
                VaultName = vaultName,
                JobId = describeJobResponse.JobId
            };

            var getJobOutputResponse = await amazonGlacierClient.GetJobOutputAsync(getJobOutputRequest).ConfigureAwait(false);

            await getJobOutputResponse.Body.CopyToAsync(fileStream).ConfigureAwait(false);
        }

        private async Task<string> GetJobIdForArchiveIdAsync(string archiveId)
        {
            using var amazonGlacierClient = new AmazonGlacierClient();

            var jobsRequest = new ListJobsRequest { VaultName = vaultName };
            ListJobsResponse jobsResponse;

            do
            {
                jobsResponse = await amazonGlacierClient.ListJobsAsync(jobsRequest).ConfigureAwait(false);

                foreach (var job in jobsResponse.JobList)
                {
                    if (job.ArchiveId.Equals(archiveId, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return job.JobId;
                    }
                }

                jobsRequest.Marker = jobsResponse.Marker;
            } while (!string.IsNullOrEmpty(jobsResponse.Marker));

            throw new Exception($"No job found for the archive ID '{archiveId}'.");
        }
    }
}
