using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Medyam.Core.Entities;

namespace Medyam.Services.Interfaces
{
    public interface IPhotoService
    {
        void Create(PhotoEntity entity, Stream photoFile);
        List<PhotoEntity> GetAll();

        Task CreateAsync(PhotoEntity entity, Stream photoFile);
        Task<List<PhotoEntity>> GetAllAsync();
    }
}
