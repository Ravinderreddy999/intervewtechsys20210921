CREATE PROCEDURE dbo.InventoryAdd	
	@ProductInstanceId INT ,
	@Quantity DECIMAL(19,6) ,		
	@TypeCategory VARCHAR(32) 
AS
BEGIN
	insert into Transactions.InventoryTransactions(	
	ProductInstanceId ,
	Quantity,	
	TypeCategory )
	values (@ProductInstanceId, @Quantity, @TypeCategory)

END