using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ClassLib.DbDataStructures;

//Zbiór Data Transfer Objects
namespace ClassLib.DataStructures
{
    [DataContract]
    public class BaseTransferObject
    {
        public int Id { get; set; }

        public BaseTransferObject(DbEntity dbEntity)
        {
            this.Id = dbEntity.Id;
        }
    }

    [DataContract]
    public class MenuItem : BaseTransferObject
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public MenuItemCategory Category { get; set; }
        [DataMember]
        public Money Price { get; set; }

        public MenuItem(MenuItemEntity menuItem)
            : base(menuItem)
        {
            this.Name = menuItem.Name;
            this.Description = menuItem.Description;
            this.Category = new MenuItemCategory(menuItem.Category);
            this.Price = menuItem.Price;
        }
    }

    [DataContract]
    public class MenuItemCategory : BaseTransferObject
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

        public MenuItemCategory(MenuItemCategoryEntity menuItemCategory)
            : base(menuItemCategory)
        {
            this.Name = menuItemCategory.Name;
            this.Description = menuItemCategory.Description;
        }

    }

    [DataContract]
    public class Order : BaseTransferObject
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public UserContext Waiter { get; set; }
        [DataMember]
        public Table Table { get; set; }
        //Item1 - menuItemId, Item2 - quantity
        [DataMember]
        public ICollection<MenuItemQuantity> MenuItems { get; set; }
        [DataMember]
        public OrderState State { get; set; }
        [DataMember]
        public DateTime PlacingDate { get; set; }
        [DataMember]
        public DateTime ClosingDate { get; set; }

        public Order(OrderEntity order)
            : base(order)
        {
            this.UserId = order.UserId;
            if (order.Waiter != null)
                this.Waiter = new UserContext(order.Waiter);

            if (order.Table != null)
                this.Table = new Table(order.Table);

            if (order.MenuItems != null)
            {
                this.MenuItems = new List<MenuItemQuantity>();
                foreach (var menuItemQuant in order.MenuItems)
                    this.MenuItems.Add(new MenuItemQuantity(menuItemQuant));
            }
        

            this.State = order.State;
                    this.PlacingDate = order.PlacingDate;
                    this.ClosingDate = order.ClosingDate;
        }
    }

    [DataContract]
    public class MenuItemQuantity : BaseTransferObject
    {
        [DataMember]
        public MenuItem MenuItem { get; set; }
        [DataMember]
        public int Quantity { get; set; }

        public MenuItemQuantity(MenuItemQuantityEntity menuItemQuantity)
            : base(menuItemQuantity)
        {
            this.MenuItem = new MenuItem(menuItemQuantity.MenuItem);
            this.Quantity = menuItemQuantity.Quantity;
        }
    }

    [DataContract]
    public class Table : BaseTransferObject
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
        public string Description { get; set; }

        public Table(TableEntity table)
            :base(table)
        {
            this.Number = table.Number;
            this.Description = table.Description;
        }
    }

    [DataContract]
    public class UserContext : BaseTransferObject
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
        public UserRole Role { get; set; }

        public UserContext(UserContextEntity userContext)
            : base(userContext)
        {
            this.FirstName = userContext.FirstName;
            this.LastName = userContext.LastName;
            this.Login = userContext.Login;
            this.Role = userContext.Role;
        }
    }
}
