using Dapper;
using Interview.Data.Models;
using Sparcpoint.SqlServer.Abstractions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Interview.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ISqlExecutor _sqlExecutor;
        public ProductRepository(ISqlExecutor sqlExecutor)
        {
            _sqlExecutor = sqlExecutor;
        }

        public Task Add(ProductModel item)
        {
            DataTable attributes = new DataTable();
            attributes.Columns.Add("Key");
            attributes.Columns.Add("Value");

            DataTable categories = new DataTable();
            categories.Columns.Add("Value");

            item.Categories.ForEach(x =>
            {
                DataRow dr = categories.NewRow();
                dr["Value"] = x;
                categories.Rows.Add(dr);
            });

            item.Attributes.ForEach(x =>
            {
                DataRow dr = attributes.NewRow();
                dr["Key"] = x.Key;
                dr["Value"] = x.Value;
                attributes.Rows.Add(dr);
            });

            return _sqlExecutor.ExecuteAsync(async (conn, trn) =>
            {
                var param = new DynamicParameters();
                param.Add("@Name", item.Name);
                param.Add("@Description", item.Description);
                param.Add("@ProductImageUris", item.ProductImageUris);
                param.Add("@ValidSkus", item.ValidSkus);
                param.Add("@AttributeList", attributes.AsTableValuedParameter("dbo.CustomAttributeList"));
                param.Add("@ProductCategories", categories.AsTableValuedParameter("dbo.StringList"));

                await conn.QueryAsync("dbo.ProductAdd", param, trn, commandType:CommandType.StoredProcedure);
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

        public IEnumerator<ProductModel> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Task<bool> Remove(ProductModel item)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductModel>> Search(ProductModel product)
        {
            DataTable attributes = new DataTable();
            attributes.Columns.Add("Key");
            attributes.Columns.Add("Value");

            DataTable categories = new DataTable();
            categories.Columns.Add("Value");

            product.Categories.ForEach(x =>
            {
                DataRow dr = categories.NewRow();
                dr["Value"] = x;
                categories.Rows.Add(dr);
            });

            product.Attributes.ForEach(x =>
            {
                DataRow dr = attributes.NewRow();
                dr["Key"] = x.Key;
                dr["Value"] = x.Value;
                attributes.Rows.Add(dr);
            });

            Dictionary<int, ProductModel> prodDict = new Dictionary<int, ProductModel>();

            return _sqlExecutor.ExecuteNonTran(async (conn) =>
            {

                var p = new DynamicParameters();
                if (!string.IsNullOrEmpty(product.Name) )
                    p.Add("@Name", product.Name);

                if (!string.IsNullOrEmpty(product.Description))
                    p.Add("@Description", product.Description);

                if (!string.IsNullOrEmpty(product.ProductImageUris))
                    p.Add("@ProductImageUris", product.ProductImageUris);

                if (!string.IsNullOrEmpty(product.ValidSkus))
                    p.Add("@ValidSkus", product.ValidSkus);

                if (product.Attributes.Count>0)
                    p.Add("@AttributeList", attributes.AsTableValuedParameter("[dbo].[CustomAttributeList]"));

                if (product.Categories.Count>0)
                    p.Add("@ProductCategories", categories.AsTableValuedParameter("dbo.StringList"));

                IEnumerable<ProductModel> result1 = conn.Query<ProductModel,CategoryModel, ProductAttributeModel, ProductModel>("dbo.ProductSearch", 
                    (prod,cat,attr) => {
                        ProductModel productToAdd = new ProductModel();

                        if (!prodDict.TryGetValue(prod.InstanceId, out productToAdd))
                        {
                            productToAdd = new ProductModel
                            {
                                InstanceId = prod.InstanceId,
                                Name = prod.Name,
                                ProductImageUris = prod.ProductImageUris,
                                ValidSkus = prod.ValidSkus,
                                CreateDateTime = prod.CreateDateTime,
                                Description = prod.Description
                            };

                            prodDict.Add(prod.InstanceId, productToAdd);
                        }

                        productToAdd.Categories.Add(cat.Name);
                        productToAdd.Attributes.Add(new ProductAttributeModel() { Key = attr.Key, Value = attr.Value });


                        return productToAdd;
                    },                    
                    splitOn: "InstanceId,CatInstanceId,prodAttrInstanceId"
                    );
              return prodDict.Values.ToList();
            });
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
