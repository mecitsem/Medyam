using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Medyam.Core.Common;
using Medyam.Data.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Medyam.Data.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        public async Task<string> UploadBlobAsync(HttpPostedFileBase photo)
        {
            if (photo == null || photo.ContentLength == 0)
            {
                return null;
            }

            string fullPath = null;
            //Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var storageAccount = StorageUtils.StorageAccount;
                var mediaKey = Guid.NewGuid();
                // Create the blob client and reference the container
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(Constants.Azure.Blobs.Images);

                // Create a unique name for the images we are about to upload
                string imageName = $"photo-{mediaKey}{Path.GetExtension(photo.FileName)}";

                // Upload image to Blob Storage
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = photo.ContentType;
                await blockBlob.UploadFromStreamAsync(photo.InputStream);

                // Convert to be HTTP based URI (default storage path is HTTPS)
                var uriBuilder = new UriBuilder(blockBlob.Uri) { Scheme = "http" };
                fullPath = uriBuilder.ToString();

                //timespan.Stop();
                //log.TraceApi("Blob Service", "PhotoService.UploadPhoto", timespan.Elapsed, "imagepath={0}", fullPath);
            }
            catch (Exception ex)
            {
                //log.Error(ex, "Error upload photo blob to storage");
            }

            return fullPath;
        }
    }
}
