using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;

namespace DataAccess
{
    /// <summary>
    /// Kontekst bazy danych. Udostępnia zbiory encji modelowych. 
    /// </summary>
    public class DataAccessProvider : DbContext
    {
        public DataAccessProvider()
            : base("SmarterASPDB")
        { }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<UserContext> Users { get; set; }
        public DbSet<Password> Passwords { get; set; }
        public DbSet<MenuItemCategory> MenuItemCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<MenuItemQuantity> MenuItemQuantities { get; set; }
    }
}
