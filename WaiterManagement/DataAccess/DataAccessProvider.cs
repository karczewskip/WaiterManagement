using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;

namespace DataAccess
{
    public class DataAccessProvider : DbContext
    {
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<WaiterContext> Waiters { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<MenuItemQuantity> MenuItemQuantities { get; set; }
    }
}
