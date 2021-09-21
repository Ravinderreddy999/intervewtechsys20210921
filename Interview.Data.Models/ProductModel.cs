using System;
using System.Collections.Generic;

namespace Interview.Data.Models
{
    public class ProductModel : BaseModel
    {
     
        //TODO: Add model validations
        
        public string Name { get; set; }
        public string Description { get; set; }
        public string ProductImageUris { get; set; }       
        public string ValidSkus { get; set; }
        public List<ProductAttributeModel> Attributes { get; set; }
        public List<string> Categories { get; set; }

        public ProductModel()
        {
            Attributes = new List<ProductAttributeModel>();
            Categories = new List<string>();
        }
    }
}
