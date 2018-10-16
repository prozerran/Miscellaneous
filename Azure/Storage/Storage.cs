
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    class Storage
    {
        /*
        static void Main(string[] args)
        {
            var storage = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));

            var blobClient = storage.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");

            container.CreateIfNotExists(BlobContainerPublicAccessType.Blob);

            var blockBlob = container.GetBlockBlobReference("me1.png");

            // upload to storage
            if (!blockBlob.Exists())
            {
                using (var fs = File.OpenRead(@"C:\Users\tim.hsu\Pictures\Test\img1.png"))
                {
                    blockBlob.UploadFromStream(fs);
                }
            }

            // download from storage
            string blobfile = @"C:\Users\tim.hsu\Pictures\Test\me1.png";

            if (!File.Exists(blobfile))
            {
                using (var fs = File.OpenWrite(blobfile))
                {
                    blockBlob.DownloadToStream(fs);
                }
            }

            // async blob copy
            var blockBlobCopy = container.GetBlockBlobReference("me2.png");
            blockBlobCopy.DeleteIfExists(); // remove remote copy first before copy

            var cb = new AsyncCallback(x => Console.WriteLine("Done Copy!"));
            blockBlobCopy.BeginStartCopy(blockBlob.Uri, cb, null);

            // list all files in cloud
            foreach (var i in container.ListBlobs())
            {
                Console.WriteLine(i.Uri);
            }
            Console.ReadKey();
        }
        */
    }
}
