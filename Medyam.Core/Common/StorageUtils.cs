using Medyam.Core.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;

namespace Medyam.Core.Common
{
    public class StorageUtils
    {
        private static CloudStorageAccount _storageAccount;

        public static CloudStorageAccount StorageAccount => _storageAccount ?? (_storageAccount = GetStorageAccount());


        public static CloudStorageAccount GetStorageAccount()
        {
            var account = CloudConfigurationManager.GetSetting(Constants.AppSettings.StorageAccountName);
            var key = CloudConfigurationManager.GetSetting(Constants.AppSettings.StorageAccountAccessKey);
            var creds = new StorageCredentials(account, key);
            return new CloudStorageAccount(creds, useHttps: true); ;
        }
    }
}
