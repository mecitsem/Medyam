using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Medyam.Core.Common;
using Medyam.Core.Entities;
using Medyam.Data.Interfaces;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Medyam.Data.Repositories
{
    public class BlobRepository : IBlobRepository
    {
        private const string ContentType = "image/jpeg";


        public async Task<string> UploadBlobAsync(Stream photo, PhotoEntity entity)
        {
            if (photo == null)
                throw new ArgumentNullException(nameof(photo));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            string fullPath = null;
            //Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var storageAccount = StorageUtils.StorageAccount;
                // Create the blob client and reference the container
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(Constants.Azure.Blobs.Images);
                if (await container.CreateIfNotExistsAsync())
                {
                    // Enable public access on the newly created "images" container
                    await container.SetPermissionsAsync(new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        });

                }


                // Create a unique name for the images we are about to upload
                string imageName = $"photo-{entity.ID}.jpg";

                // Upload image to Blob Storage
                var blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = ContentType;
                await blockBlob.UploadFromStreamAsync(photo);

                // Convert to be HTTP based URI (default storage path is HTTPS)
                var uriBuilder = new UriBuilder(blockBlob.Uri) { Scheme = "https" };
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

        public string UploadBlob(Stream photo, PhotoEntity entity)
        {
            if (photo == null)
                throw new ArgumentNullException(nameof(photo));

            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            string fullPath = null;
            //Stopwatch timespan = Stopwatch.StartNew();

            try
            {
                var storageAccount = StorageUtils.StorageAccount;
                // Create the blob client and reference the container
                var blobClient = storageAccount.CreateCloudBlobClient();
                var container = blobClient.GetContainerReference(Constants.Azure.Blobs.Images);
                if ( container.CreateIfNotExists())
                {
                    // Enable public access on the newly created "images" container
                    container.SetPermissions(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });

                }
                // Create a unique name for the images we are about to upload
                string imageName = $"photo-{entity.ID}.jpg";

                // Upload image to Blob Storage
                var blockBlob = container.GetBlockBlobReference(imageName);
                blockBlob.Properties.ContentType = ContentType;
                blockBlob.UploadFromStream(photo);

                // Convert to be HTTP based URI (default storage path is HTTPS)
                var uriBuilder = new UriBuilder(blockBlob.Uri) { Scheme = "https" };
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
