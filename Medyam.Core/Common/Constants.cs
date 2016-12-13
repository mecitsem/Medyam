using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medyam.Core.Common
{
    public class Constants
    {

        public class Azure
        {
            public class  Tables
            {
                public const string Photos = "photos";
            }

            public class Blobs
            {
                public const string Images = "images";
            }

        }

        public class AppSettings
        {
            public const string StorageAccountName = "StorageAccountName";
            public const string StorageAccountAccessKey = "StorageAccountAccessKey";
            public const string SubscriptionKey = "SubscriptionKey";
            public const string PassPhrase = "PassPhrase";
        }
    }
}
