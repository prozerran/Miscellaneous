using ConsoleAppEFCore.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppEFCore.Data
{
    public class ContosoPizzaContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<OrderDetail> OrderDetails { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder.IsConfigured)
                return;

            var builder = new MySqlConnectionStringBuilder
            {
                Server = "localhost",
                Database = "temus",
                UserID = "root",
                Password = "test"
            };

            var serverVersion = new MariaDbServerVersion(new Version(10, 11, 2));
            optionsBuilder.UseMySql(builder.ConnectionString, serverVersion);
        }
    }
}
