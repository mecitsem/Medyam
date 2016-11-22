using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medyam.Core.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Shared.Protocol;

namespace Medyam.Core.Common
{
    public class StorageUtils
    {
        private static CloudStorageAccount _storageAccount;

        public static CloudStorageAccount StorageAccount => _storageAccount ?? (_storageAccount = GetStorageAccount());


        private static CloudStorageAccount GetStorageAccount()
        {
            var account = CloudConfigurationManager.GetSetting(Constants.AppSettings.StorageAccountName);
            var key = CloudConfigurationManager.GetSetting(Constants.AppSettings.StorageAccountAccessKey);
            var connectionString = $"DefaultEndpointsProtocol=https;AccountName={account};AccountKey={key}";
            return CloudStorageAccount.Parse(connectionString);
        }
    }
}
