using Microsoft.EntityFrameworkCore;
using core.Entities;
using core.enums;
namespace Infrastructure.Data
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {

        }
        #region DbSet Section
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<RestaurantTable> Tables { get; set; }

        #endregion
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<User>().HasData(
                 new User
                 {
                     Id = 1,
                     UserName = "SuperAdmin",
                     Email = "SuperAdmin@example.com",
                     PasswordHash = "oc6LAjoTOIr8gjgbG8iLWg==.ww9pESGofYsgXj9h6eYfZD0QC8Ia/wKizwZWneRSOzA=", // SuperAdmin
                     Role = UserRole.SuperAdmin,
                     ResetToken = null,
                     ResetTokenExpiry = null
                 }
             );
        }
    }
}

