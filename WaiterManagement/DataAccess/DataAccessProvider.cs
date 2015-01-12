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

        public DbSet<MenuItemEntity> MenuItems { get; set; }
        public DbSet<UserContextEntity> Users { get; set; }
        public DbSet<PasswordEntity> Passwords { get; set; }
        public DbSet<MenuItemCategoryEntity> MenuItemCategories { get; set; }
        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<TableEntity> Tables { get; set; }
        public DbSet<MenuItemQuantityEntity> MenuItemQuantities { get; set; }
    }
}
