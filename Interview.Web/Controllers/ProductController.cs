using Interview.Data.Models;
using Interview.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Interview.Web.Controllers
{
    [Route("api/v1/products")]
    public class ProductController : Controller
    {

        private readonly IProductRepository productReposiory;
        private readonly IInventoryRepository inventoryRepository;

        public ProductController(IProductRepository prodRepo, IInventoryRepository invetoryRepo)
        {
            productReposiory = prodRepo;
            inventoryRepository = invetoryRepo;
        }

        [HttpGet]
        public Task<List<ProductModel>> GetAllProducts()
        {
            return productReposiory.Search(new ProductModel());
        }


        /// <summary>
        /// Search Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost(Name ="SearchProduct")]
        public Task<List<ProductModel>> SearchProduct([FromBody] ProductModel product)
        {
            return productReposiory.Search(product);
        }


        /// <summary>
        /// Add Product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost(Name = "AddProduct")]
        public Task AddProduct([FromBody]ProductModel product)
        {
            return productReposiory.Add(product);
        }


        /// <summary>
        /// Add Product to inventory
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpPost("{productInstanceId}/Inventory",Name = "AddProductToInventory")]
        public Task AddInventory([FromBody] InventoryTransactionModel productInventory)
        {
            return inventoryRepository.Add(productInventory);
        }


        /// <summary>
        /// Remove Product from inventory
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpDelete("{productInstanceId}/Inventory", Name = "RemoveProductfromInventory")]
        public Task RemoveProductfromInventory([FromBody] InventoryTransactionModel productInventory)
        {
            return inventoryRepository.Remove(productInventory);
        }


        /// <summary>
        /// product Inventory count
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        [HttpGet("{productInstanceId}/count", Name = "GetProductInventoryCount")]
        public Task<int> GetProductCount(int productInstanceId)
        {
            return inventoryRepository.InventoryCountByProduct(productInstanceId);
        }
    }
}
