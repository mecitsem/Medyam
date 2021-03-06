﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medyam.Core.Entities;

namespace Medyam.Data.Interfaces
{
    public interface ITableRepository
    {
        void CreateEntity(PhotoEntity entity);
        List<PhotoEntity> GetEntities(string filter);
        PhotoEntity GetEntity(string partitionKey, string rowKey);


        Task CreateEntityAsync(PhotoEntity entity);
        Task<List<PhotoEntity>> GetEntitiesAsync(string filter);
        Task<PhotoEntity> GetEntityAsync(string partitionKey, string rowKey);
    }
}
