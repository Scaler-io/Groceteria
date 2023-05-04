using Dapper;
using Groceteria.Discount.Grpc.Entities;
using Groceteria.Discount.Grpc.Models.Constants;
using Groceteria.Shared.Extensions;
using Npgsql;
using ILogger = Serilog.ILogger;

namespace Groceteria.Discount.Grpc.DataAccess.Repositories
{
    public class DiscountRepository : IDiscountRepository
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly NpgsqlConnection _connection;

        public DiscountRepository(ILogger logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _connection = EstablishConnection();
        }

        public async Task<IEnumerable<Coupon>> GetAllCoupons()
        {
            var command = new CommandDefinition(DiscountDbCommands.SelectAll);
            var coupons = await _connection.QueryAsync<Coupon>(command);
            return coupons;
        }

        public async Task<Coupon> GetCoupon(int id)
        {
            var command = new CommandDefinition(DiscountDbCommands.SelectByProductId);
            var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>(command);
            return coupon;
        }

        public async Task<Coupon> GetCouponByProductName(string productName)
        {
            var command = new CommandDefinition(DiscountDbCommands.SelectByProductName);
            var coupon = await _connection.QueryFirstOrDefaultAsync<Coupon>(command);
            return coupon;
        }

        public async Task<bool> CreateCoupon(Coupon discountCoupon)
        {
            var affected = 0;
            var command = new CommandDefinition(DiscountDbCommands.Insert, new
            {
                ProductName = discountCoupon.ProductName,
                Description = discountCoupon.Description,
                Amount = discountCoupon.Amount,
                CreatedAt = discountCoupon.CreatedAt,
                UpdatedAt = discountCoupon.UpdatedAt
            });
            affected = await _connection.ExecuteAsync(command);
            return affected > 0;
        }

        public async Task<bool> UpdateCoupon(Coupon discountCoupon)
        {
            var affected = 0;
            var command = new CommandDefinition(DiscountDbCommands.Update, new
            {
                ProductName = discountCoupon.ProductName,
                Description = discountCoupon.Description,
                Amount = discountCoupon.Amount,
                UpdatedAt = discountCoupon.UpdatedAt,
                Id = discountCoupon.Id
            });
            affected = await _connection.ExecuteAsync(command);
            return affected > 0;
        }

        public async Task<bool> DeleteCoupon(int id)
        {
            var affected = 0;
            var command = new CommandDefinition(DiscountDbCommands.Delete, new { Id = id });
            affected = await _connection.ExecuteAsync(command);
            _logger.Here().MethodExited();
            return affected > 0;
        }

        private NpgsqlConnection EstablishConnection()
        {
            using var connection = new NpgsqlConnection(
                    _configuration["DiscountDb:ConnectionString"]
                );
            _logger.Here().Information("Connection established to discount db");
            return connection;
        }
    }
}
