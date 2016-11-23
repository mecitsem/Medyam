using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Medyam.Core.Entities
{
    public class PhotoEntity : TableEntity
    {
        public PhotoEntity() { }

        public PhotoEntity(string partitionKey,string rowKey)
        {
            PartitionKey = partitionKey;
            RowKey = rowKey;
        }


        public Guid ID { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }

    }
}
