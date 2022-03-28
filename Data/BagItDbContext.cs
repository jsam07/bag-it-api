using bagit_api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace bagit_api.Data;

public class BagItDbContext : IdentityDbContext
{
    public BagItDbContext(DbContextOptions<BagItDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ShoppingList> ShoppingLists { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        // Seeders
        modelBuilder.Entity<User>().HasData(SeedData.GetUser());
        modelBuilder.Entity<ShoppingList>().HasData(SeedData.GetList());

        // Model n:m relationship  b/w Users and Shopping Lists
        modelBuilder.Entity<UserShoppingList>()
            .HasKey(ul => new { ul.ListId, ul.UserId });

        modelBuilder.Entity<UserShoppingList>()
            .HasOne(sl => sl.User)
            .WithMany(u => u.UserShoppingLists)
            .HasForeignKey(sl => sl.UserId);

        modelBuilder.Entity<UserShoppingList>()
            .HasOne(sl => sl.List)
            .WithMany(sl => sl.UserShoppingLists)
            .HasForeignKey(sl => sl.ListId);
        
        
        // Model n:m relationship  b/w Products and Shopping Lists
        modelBuilder.Entity<ShoppingListProduct>()
            .HasKey(slp => new { slp.ListId, slp.ProductId });

        modelBuilder.Entity<ShoppingListProduct>()
            .HasOne(slp => slp.List)
            .WithMany(sl => sl.ShoppingListProducts)
            .HasForeignKey(slp => slp.ListId);

        modelBuilder.Entity<ShoppingListProduct>()
            .HasOne(slp => slp.Product)
            .WithMany(p => p.ShoppingListProducts)
            .HasForeignKey(slp => slp.ProductId);
    }
    
}
