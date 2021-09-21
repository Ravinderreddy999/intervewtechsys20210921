using Interview.Data.Models;
using Sparcpoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Data.Repository
{
    public interface IProductRepository : IAsyncCollection<ProductModel>
    {
        Task<List<ProductModel>> Search(ProductModel product);
    }
}
