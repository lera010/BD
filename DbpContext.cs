using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static _3._3pz.Models;

namespace _3._3pz
{
    public class DbpContext : DbContext
    {
        public DbSet<Role> Roles { get; set; } 
        public DbSet<User> Users { get; set; } 
        public DbSet<Service> Services { get; set; } 
        public DbSet<Cart> Carts { get; set; } 
        public DbSet<CartItem> CartItems { get; set; } 
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=pztest;Username=postgres;Password=postgres");
        }

    }
}
