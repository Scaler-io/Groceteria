namespace Groceteria.Discount.Grpc.Models.Constants
{
    public class DiscountDbCommands
    {
        public const string SelectAll = "SELECT * FROM Coupon";
        public const string SelectByProductId = "SELECT * FROM coupon WHERE productid = @ProductId";
        public const string SelectByProductName = "SELECT * FROM Coupon WHERE ProductName = @ProductName";
        public const string Insert = "INSERT INTO Coupon (ProductName, Description, Amount, CreatedAt, UpdatedAt) VALUES (@ProductName, @Description, @Amount, @CreatedAt, @UpdatedAt)";
        public const string Update = "UPDATE Coupon SET ProductName=@ProductName, Description = @Description, Amount = @Amount, UpdatedAt = @UpdatedAt WHERE Id = @Id";
        public const string Delete = "DELETE FROM Coupon WHERE Id=@Id";

        // DDL COMMANDS
        public const string DropIfExist = "DROP TABLE IF EXISTS Coupon";
        public const string CreateCouponTable = @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductId  VARCHAR(50) NOT NULL,
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT,
                                                                CreatedAt TIMESTAMP,
                                                                UpdatedAt TIMESTAMP)";
    }
}
