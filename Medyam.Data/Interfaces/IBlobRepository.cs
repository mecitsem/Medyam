using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Medyam.Data.Interfaces
{
    public interface IBlobRepository
    {
       Task<string> UploadBlobAsync(HttpPostedFileBase photo);
    }
}
