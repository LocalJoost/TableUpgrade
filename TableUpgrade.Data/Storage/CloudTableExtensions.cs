using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace TableUpgrade.Data.Storage
{
    public static class CloudTableExtensions
    {
        public static async Task<T> GetAsync<T>(this CloudTable t, 
            string partitionKey, string rowKey) where T: TableEntity
        {
            var operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            var result = await t.ExecuteAsync(operation);
            return (T)result.Result;
        }

        public static async Task<T> GetAsync<T>(this CloudTable t, string rowKey) 
            where T : TableEntity
        {
            return await GetAsync<T>(t, "1", rowKey);
        }

        public static async Task<TableResult> StoreAsync<T>(this CloudTable t, 
            T entity, string partitionKey, string rowKey) where T : TableEntity
        {
            entity.PartitionKey = partitionKey;
            entity.RowKey = rowKey;
            return await t.ExecuteAsync(TableOperation.InsertOrReplace(entity));
        }

        public static async Task<TableResult> StoreAsync<T>(this CloudTable t,
            T entity, string rowKey) where T : TableEntity
        {
            return await t.StoreAsync(entity, "1", rowKey);
        }

        public static async Task<TableResult> DeleteAsync<T>(this CloudTable t,
            T entity) where T : TableEntity
        {
            return await t.ExecuteAsync(TableOperation.Delete(entity));
        }
    }
}
