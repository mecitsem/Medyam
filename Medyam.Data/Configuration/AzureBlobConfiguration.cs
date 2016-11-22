using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medyam.Core.Common;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Medyam.Data.Configuration
{
    public class AzureBlobConfiguration
    {


        public async void CreateAndConfigureAsync()
        {
            try
            {
                CloudStorageAccount storageAccount = StorageUtils.StorageAccount;

                // Create a blob client and retrieve reference to images container
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(Constants.Azure.Blobs.Images);

                // Create the "images" container if it doesn't already exist.
                if (await container.CreateIfNotExistsAsync())
                {
                    // Enable public access on the newly created "images" container
                    await container.SetPermissionsAsync(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });

                    //log.Information("Successfully created Blob Storage Images Container and made it public");
                }
            }
            catch (Exception ex)
            {
                //log.Error(ex, "Failure to Create or Configure images container in Blob Storage Service");
            }
        }


    }
}
