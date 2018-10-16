using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using StorageWeb.Models;

namespace StorageWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var storage = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnection"));

            var blobClient = storage.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");

            var blobs = new List<BlobImage>();

            foreach (var i in container.ListBlobs())
            {
                if (i.GetType() == typeof(CloudBlockBlob))
                {
                    // use sas token from policy set in azure
                    // MySAS policy NEEDs to be first set in azure
                    //var sas = container.GetSharedAccessSignature(null, "MySAS");
                    //blobs.Add(new BlobImage { BlobUri = i.Uri.ToString() + sas });

                    // use pre-defined sas token from code
                    //var sas = GetSasToken(storage);
                    //blobs.Add(new BlobImage { BlobUri = i.Uri.ToString() + sas });

                    // No sas token required [test only]
                    blobs.Add(new BlobImage { BlobUri = i.Uri.ToString() });
                }
            }
            return View(blobs);
        }

        static string GetSasToken(CloudStorageAccount storage)
        {
            SharedAccessAccountPolicy policy = new SharedAccessAccountPolicy()
            {
                Permissions = SharedAccessAccountPermissions.Read | SharedAccessAccountPermissions.Write,
                Services = SharedAccessAccountServices.Blob,
                ResourceTypes = SharedAccessAccountResourceTypes.Object,
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(30),
                Protocols = SharedAccessProtocol.HttpsOnly
            };

            return storage.GetSharedAccessSignature(policy);
        }
    }
}