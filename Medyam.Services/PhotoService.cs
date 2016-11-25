using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Medyam.Core.Entities;
using Medyam.Data.Interfaces;
using Medyam.Services.Interfaces;

namespace Medyam.Services
{
    public class PhotoService : IPhotoService
    {
        private const string Owner = "medyam";
        private readonly ITableRepository _tableRepository;
        private readonly IBlobRepository _blobRepository;
        private readonly IVisionService _visionService;

        public PhotoService(ITableRepository tableRepository,
                            IBlobRepository blobRepository,
                            IVisionService visionService)
        {
            _tableRepository = tableRepository;
            _blobRepository = blobRepository;
            _visionService = visionService;
        }

        public void Create(PhotoEntity entity, Stream photoFile)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (photoFile == null)
                throw new ArgumentNullException(nameof(photoFile));

            //Upload Photo
            var photoUrl = _blobRepository.UploadBlob(photoFile, entity);
            //Set Photo Url
            entity.Url = photoUrl;
            entity.Owner = Owner;

            //Vision Service
            if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                try
                {
                    var task = _visionService.DescripteUrlAsync(entity.Url);
                    task.Wait();
                    var analysisResult = task.Result;
                    entity.Tags = string.Join(",", analysisResult.Description.Tags);
                }
                catch
                {
                    // ignored
                }
            }



            _tableRepository.CreateEntity(entity);


        }

        public List<PhotoEntity> GetAll()
        {
            return _tableRepository.GetEntities(Owner);
        }

        public async Task CreateAsync(PhotoEntity entity, Stream photoFile)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (photoFile == null)
                throw new ArgumentNullException(nameof(photoFile));

            //Upload Photo
            var photoUrl = await _blobRepository.UploadBlobAsync(photoFile, entity);
            //Set Photo Url
            entity.Url = photoUrl;
            entity.Owner = Owner;

            //Vision Service
            if (!string.IsNullOrWhiteSpace(photoUrl))
            {
                try
                {
                    var analysisResult = await _visionService.DescripteUrlAsync(entity.Url);
                    entity.Tags = string.Join(",", analysisResult.Description.Tags);
                }
                catch
                {
                    // ignored
                }
            }



            await _tableRepository.CreateEntityAsync(entity);
        }

        public async Task<List<PhotoEntity>> GetAllAsync()
        {
            return await _tableRepository.GetEntitiesAsync(Owner);
        }
    }
}
