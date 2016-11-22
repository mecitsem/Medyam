using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Medyam.Core.Entities;

namespace Medyam.Services.Interfaces
{
    public interface IPhotoService
    {
        void Create(PhotoEntity entity, HttpPostedFileBase photoFile);
        List<PhotoEntity> GetAll();
    }
}
