using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Medyam.Core.Common;
using Medyam.Core.Entities;
using Medyam.Data.Interfaces;
using Microsoft.WindowsAzure.Storage.Table;

namespace Medyam.Data.Repositories
{
    public class TableRepository : ITableRepository
    {
        private readonly CloudTableClient _tableClient;

        public TableRepository()
        {
            var storeAccount = StorageUtils.StorageAccount;

            _tableClient = storeAccount.CreateCloudTableClient();

            var table = _tableClient.GetTableReference(Constants.Azure.Tables.Photos);
            table.CreateIfNotExists();
        }

        public void CreateEntity(PhotoEntity entity)
        {
            var table = _tableClient.GetTableReference(Constants.Azure.Tables.Photos);

            var insertOperation = TableOperation.Insert(entity);

            table.Execute(insertOperation);
        }

        public List<PhotoEntity> GetEntities(string filter)
        {
            var table = _tableClient.GetTableReference(Constants.Azure.Tables.Photos);

            var query = new TableQuery<PhotoEntity>().Where(TableQuery.GenerateFilterCondition("Owner", QueryComparisons.Equal, filter));

            var photos = table.ExecuteQuery(query).Select(item => new PhotoEntity()
            {
                PhotoId = item.PhotoId,
                Owner = item.Owner,
                Description = item.Description,
                IsDone = item.IsDone,
                PhotoUrl = item.PhotoUrl,
                Title = item.Title,
                Tags = item.Tags
            }).ToList();

            return photos;
        }

        public PhotoEntity GetEntity(string partitionKey, string rowKey)
        {
            var table = _tableClient.GetTableReference(Constants.Azure.Tables.Photos);

            var tableOperation = TableOperation.Retrieve<PhotoEntity>(partitionKey, rowKey);

            var entity = table.Execute(tableOperation).Result as PhotoEntity;

            return entity;

        }
    }
}
