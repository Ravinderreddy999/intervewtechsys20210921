CREATE PROCEDURE [dbo].[ProductSearch] @Name VARCHAR(256) =null
   ,@Description VARCHAR(256) =null
   ,@ProductImageUris VARCHAR(MAX) =null
   ,@ValidSkus VARCHAR(MAX) =null
   ,@AttributeList dbo.CustomAttributeList readonly
   ,@ProductCategories dbo.StringList readonly
AS
BEGIN
   SET NOCOUNT ON

   SELECT  prod.InstanceId,
   prod.Name,
   prod.Description,
   prod.ProductImageUris,
   prod.ValidSkus,
   prod.CreatedTimestamp,
   cat.InstanceId as CatInstanceId,
   cat.Name,
   prodAttr.InstanceId prodAttrInstanceId,
   prodattr.[Key],
   prodAttr.Value
   FROM Instances.Products prod
   LEFT JOIN Instances.ProductCategories prodCat
      ON prod.InstanceId = prodCat.InstanceId
   LEFT JOIN Instances.Categories cat
      ON prodCat.CategoryInstanceId = cat.InstanceId
   LEFT JOIN Instances.ProductAttributes prodAttr
      ON prodAttr.InstanceId = prod.InstanceId
   LEFT JOIN @ProductCategories catquery
      ON catquery.Value = cat.Name
   LEFT JOIN @AttributeList attrlist
      ON attrlist.[Key] = prodAttr.[Key]
   WHERE prod.Name = coalesce(@Name, prod.Name)
      AND prod.Description = coalesce(@Description, prod.Description)
      AND prod.ProductImageUris = coalesce(@ProductImageUris, prod.ProductImageUris)
      AND prod.ValidSkus = coalesce(@ValidSkus, prod.ValidSkus)
END