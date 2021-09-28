CREATE PROCEDURE [dbo].[InventoryCount]
	@ProductInstanceId INT 
AS
begin
	select count(*) from Transactions.InventoryTransactions 
	where ProductInstanceId = @ProductInstanceId
end