using Dapper;
using Interview.Data.Models;
using Sparcpoint.SqlServer.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Data.Repository
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly ISqlExecutor _sqlExecutor;
        public InventoryRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public Task Add(InventoryTransactionModel item)
        {            
            return _sqlExecutor.ExecuteAsync(async (conn, trn) =>
            {
                var p = new DynamicParameters();
                p.Add("@ProductInstanceId", item.ProductInstanceId);
                p.Add("@Quantity", item.Quantity);
                p.Add("@TypeCategory", item.TypeCategory);
               

                await conn.ExecuteAsync("dbo.InventoryAdd", p);
            });
        }

        public Task Clear()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetCount()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<InventoryTransactionModel> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Task<int> InventoryCountByProduct(int productInstanceId)
        {
            return _sqlExecutor.ExecuteAsync(async (conn, trn) =>
            {
                var p = new DynamicParameters();
                p.Add("@ProductInstanceId", productInstanceId);


                int result1 = await conn.QueryFirstAsync<int>("dbo.InventoryCount", p);
                return result1;
            });
        }

        public Task<bool> Remove(InventoryTransactionModel item)
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
