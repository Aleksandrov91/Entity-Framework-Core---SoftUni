namespace FastFood.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class FastFoodDbContext : DbContext
    {
        public FastFoodDbContext()
        {
        }

        public FastFoodDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Position> Positions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<Item> Items { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                builder.UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Position>(p => p.HasAlternateKey(n => n.Name));

            builder.Entity<Order>(p => p.Ignore(tp => tp.TotalPrice));

            builder.Entity<OrderItem>(ori =>
            ori.HasOne(o => o.Order)
               .WithMany(or => or.OrderItems)
               .OnDelete(DeleteBehavior.Restrict));

            builder.Entity<OrderItem>(ori =>
            ori.HasOne(i => i.Item)
               .WithMany(or => or.OrderItems)
               .OnDelete(DeleteBehavior.Restrict));

            builder.Entity<OrderItem>(ori => ori.HasKey(or => new { or.ItemId, or.OrderId }));
        }
    }
}