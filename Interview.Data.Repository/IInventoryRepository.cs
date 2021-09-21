using Interview.Data.Models;
using Sparcpoint;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Data.Repository
{
    public interface IInventoryRepository : IAsyncCollection<InventoryTransactionModel>
    {
        Task<int> InventoryCountByProduct(int productInstanceId);
    }
}
