using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace WebShop.Data.Models
{
    public class WebShopContext : IdentityDbContext<ShopUser>
    {
        public WebShopContext(DbContextOptions<WebShopContext> options)
            : base(options)
        {
        }

        public DbSet<ShopUser> ShopUsers { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<Cart> Carts { get; set; }
    }
}
