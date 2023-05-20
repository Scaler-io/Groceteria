namespace Groceteria.Discount.Grpc.Models.Constants
{
    public class DiscountDbCommands
    {
        public const string SelectAll = "SELECT * FROM Coupon";
        public const string SelectByProductInfo = "SELECT * FROM coupon WHERE productid = @ProductId and productname = @ProductName";
        public const string SelectByProductName = "SELECT * FROM Coupon WHERE productname = @ProductName";
        public const string Insert = "INSERT INTO Coupon (ProductName, Description, Amount, CreatedAt, UpdatedAt) VALUES (@ProductName, @Description, @Amount, @CreatedAt, @UpdatedAt)";
        public const string Update = "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount, UpdatedAt = @UpdatedAt WHERE Id = @Id";
        public const string Delete = "DELETE FROM Coupon WHERE productid=@ProductId and productname=@ProductName";

        // DDL COMMANDS
        public const string DropIfExist = "DROP TABLE IF EXISTS Coupon";
        public const string CreateCouponTable = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductId  VARCHAR(50) NOT NULL,
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT,
                                                                CreatedAt TIMESTAMP,
                                                                UpdatedAt TIMESTAMP)";
        public const string IfCouponTableExists = "SELECT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = @tableName)";
    }
}
