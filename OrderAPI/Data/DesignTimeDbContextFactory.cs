using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace OrderAPI.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<OrdersDbContext>
    {
        public OrdersDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<OrdersDbContext>();
            optionsBuilder.UseSqlServer("Server=your-server;Database=OrdersDb;User Id=your-user;Password=your-password;");

            return new OrdersDbContext(optionsBuilder.Options);
        }
    }
}
