using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Medyam.Core.Helpers
{
    public class CloudConfigurationManager
    {
        private const string Prefix = "medyam";

        public static string GetSetting(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            return ConfigurationManager.AppSettings[$"{Prefix}:{key}"];
        }
    }
}
