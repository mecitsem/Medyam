using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Medyam.Core.Entities;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Medyam.Data.Interfaces
{
    public interface IBlobRepository
    {
        Task<string> UploadBlobAsync(Stream photo, PhotoEntity entity);
        string UploadBlob(Stream photo, PhotoEntity entity);
    }
}
