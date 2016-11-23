using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Medyam.Web.Models
{
    public class PhotoModel
    {
        public Guid Id  { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
    }
}