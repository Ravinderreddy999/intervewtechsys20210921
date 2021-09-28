CREATE PROCEDURE [dbo].[ProductAdd] @Name VARCHAR(256) 
   ,@Description VARCHAR(256) 
   ,@ProductImageUris VARCHAR(MAX) 
   ,@ValidSkus VARCHAR(MAX) 
   ,@AttributeList dbo.CustomAttributeList readonly
   ,@ProductCategories dbo.StringList readonly
AS
BEGIN
   DECLARE @ProductInstance INT

   BEGIN TRY
      BEGIN TRANSACTION

      --Insert Product
      INSERT INTO Instances.Products (
         [Name]
         ,[Description]
         ,ProductImageUris
         ,ValidSkus
         )
      VALUES (
         @Name
         ,@Description
         ,@ProductImageUris
         ,@ValidSkus
         )

      --Get the insance Id
      SET @ProductInstance = SCOPE_IDENTITY()

      --insert Product attributes
      INSERT INTO Instances.ProductAttributes
      SELECT @ProductInstance
         ,[Key]
         ,[Value]
      FROM @AttributeList

      --insert Product Categories
      INSERT INTO Instances.ProductCategories
      SELECT @ProductInstance
         ,Categories.InstanceId
      FROM Instances.Categories AS Categories
      JOIN @ProductCategories pc ON Categories.Name = pc.Value

      IF (@@TRANCOUNT > 0)
      BEGIN
         COMMIT
      END

      SELECT @ProductInstance
   END TRY

   BEGIN CATCH
      IF (@@TRANCOUNT > 0)
      BEGIN
         ROLLBACK TRANSACTION
      END

      SELECT - 1
   END CATCH
END