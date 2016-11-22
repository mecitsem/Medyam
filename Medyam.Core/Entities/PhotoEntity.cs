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
        public int PhotoId { get; set; }
        [Required]
        public string Owner { get; set; }
        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        public string PhotoUrl { get; set; }
        public IList<string> Tags { get; set; }
        public bool IsDone { get; set; }

    }
}
