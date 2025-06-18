using Microsoft.EntityFrameworkCore;
using Shopping_Cart.Models;
using System.Collections.Generic;

namespace Shopping_Cart.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Product> product { get; set; }

        public DbSet<CartItem> cartItem { get; set; }
        //public DbSet<Discount> discount { get; set; }
        //public DbSet<FlatDiscount> flatDiscount { get; set; }
    }
}
