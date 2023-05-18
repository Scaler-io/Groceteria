using Npgsql;
using ILogger = Serilog.ILogger;
using Groceteria.Shared.Extensions;
using Groceteria.Discount.Grpc.Models.Constants;

namespace Groceteria.Discount.Grpc.Extensions
{
    public static class DataSeedingExtension
    {
        public static WebApplication MigrateDatabase(this WebApplication app, int? retry = 0)
        {
            int retryForAvailability = retry.Value;

            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;
            var configuration = services.GetRequiredService<IConfiguration>();
            var logger = services.GetRequiredService<ILogger>();

            try
            {
                logger.Here().Information("Migrating discount database.");
                using var connection = new NpgsqlConnection(configuration["DiscountDb:ConnectionString"]);
                connection.Open();

                var command = new NpgsqlCommand
                {
                    Connection = connection
                };

                // drop table coupon if exists
                command.CommandText = DiscountDbCommands.DropIfExist;
                command.ExecuteNonQuery();

                // create table coupon
                command.CommandText = DiscountDbCommands.CreateCouponTable;
                command.ExecuteNonQuery();

                // insert data into coupon
                command.CommandText = $"INSERT INTO Coupon(ProductId, ProductName, Description, Amount, CreatedAt, UpdatedAt) VALUES('6464f0ba15023275a9087c3c', 'Iphone 12 Pro', 'IPhone Discount 1', 150, '{DateTime.Now}', '{DateTime.Now}');";
                command.ExecuteNonQuery();

                command.CommandText = $"INSERT INTO Coupon(ProductId, ProductName, Description, Amount, CreatedAt, UpdatedAt) VALUES('6464f0ba15023275a9087c3d', 'Samsung S13', 'Samsung Discount', 100, '{DateTime.Now}', '{DateTime.Now}');";
                command.ExecuteNonQuery();

                logger.Here().Information("Migrated discount database.");
            }
            catch (NpgsqlException sqlException)
            {
                logger.Error(sqlException, "An error occured while migrating discount database.");
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    System.Threading.Thread.Sleep(2000);
                    MigrateDatabase(app, retryForAvailability);
                }
            }
            return app;
        }
    }
}