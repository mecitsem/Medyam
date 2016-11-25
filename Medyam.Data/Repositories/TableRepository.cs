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
        private readonly CloudTable _table;
        public TableRepository()
        {
            var storeAccount = StorageUtils.StorageAccount;
            var tableClient = storeAccount.CreateCloudTableClient();
            _table = tableClient.GetTableReference(Constants.Azure.Tables.Photos);
            _table.CreateIfNotExists();
        }

        public void CreateEntity(PhotoEntity entity)
        {

            var insertOperation = TableOperation.Insert(entity);

            _table.Execute(insertOperation);
        }

        public List<PhotoEntity> GetEntities(string filter)
        {


            var query = new TableQuery<PhotoEntity>().Where(TableQuery.GenerateFilterCondition("Owner", QueryComparisons.Equal, filter));
            var photos = _table.ExecuteQuery(query).OrderByDescending(o => o.Timestamp).ToList();
            return photos;
        }

        public PhotoEntity GetEntity(string partitionKey, string rowKey)
        {

            var tableOperation = TableOperation.Retrieve<PhotoEntity>(partitionKey, rowKey);

            var entity = _table.Execute(tableOperation).Result as PhotoEntity;

            return entity;

        }

        public async Task CreateEntityAsync(PhotoEntity entity)
        {
            var insertOperation = TableOperation.Insert(entity);

            await _table.ExecuteAsync(insertOperation);
        }

        public async Task<List<PhotoEntity>> GetEntitiesAsync(string filter)
        {
            TableContinuationToken token = null;
            var query = new TableQuery<PhotoEntity>().Where(TableQuery.GenerateFilterCondition("Owner", QueryComparisons.Equal, filter));
            var entities = new List<PhotoEntity>();
            do
            {
                var queryResult = await _table.ExecuteQuerySegmentedAsync(query, token);
                entities.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
            } while (token != null);

            return entities.OrderByDescending(o => o.Timestamp).ToList();
        }

        public async Task<PhotoEntity> GetEntityAsync(string partitionKey, string rowKey)
        {
            var tableOperation = TableOperation.Retrieve<PhotoEntity>(partitionKey, rowKey);
            var data = await _table.ExecuteAsync(tableOperation);
            return data.Result as PhotoEntity;
        }
    }
}
