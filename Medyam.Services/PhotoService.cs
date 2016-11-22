using System;
using System.Collections.Generic;
using System.Web;
using Medyam.Core.Entities;
using Medyam.Data.Interfaces;
using Medyam.Services.Interfaces;

namespace Medyam.Services
{
    public class PhotoService:IPhotoService
    {
        private readonly ITableRepository _tableRepository;
        private readonly IBlobRepository _blobRepository;

        public PhotoService(ITableRepository tableRepository, IBlobRepository blobRepository)
        {
            _tableRepository = tableRepository;
            _blobRepository = blobRepository;
        }

        public async void Create(PhotoEntity entity, HttpPostedFileBase photoFile)
        {
           if(entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (photoFile == null || photoFile.ContentLength == 0)
                throw new ArgumentNullException(nameof(photoFile));

            var photoUrl = await _blobRepository.UploadBlobAsync(photoFile);

            entity.PhotoUrl = photoUrl;
            entity.Owner = "medyam";

            _tableRepository.CreateEntity(entity);

            
        }

        public List<PhotoEntity> GetAll()
        {
            return _tableRepository.GetEntities("medyam");
        }
    }
}
