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
        [DataMember]
        public int Id { get; set; }

        public BaseTransferObject(DbEntity dbEntity)
        {
            Id = dbEntity.Id;
        }

        public BaseTransferObject()
        { }
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
            Name = menuItem.Name;
            Description = menuItem.Description;
            Category = new MenuItemCategory(menuItem.Category);
            Price = menuItem.Price;
        }

        public MenuItem()
        { }
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
            Name = menuItemCategory.Name;
            Description = menuItemCategory.Description;
        }

        public MenuItemCategory() { }

    }

    [DataContract]
    public class Order : BaseTransferObject
    {
        [DataMember]
        public UserContext Client { get; set; }
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
            Client = new UserContext(order.Client);

            if (order.Waiter != null)
                Waiter = new UserContext(order.Waiter);

            if (order.Table != null)
                Table = new Table(order.Table);

            if (order.MenuItems != null)
            {
                MenuItems = new HashSet<MenuItemQuantity>();
                foreach (var menuItemQuant in order.MenuItems)
                    MenuItems.Add(new MenuItemQuantity(menuItemQuant));
            }
        

            State = order.State;
            PlacingDate = order.PlacingDate;
            ClosingDate = order.ClosingDate;
        }

        public Order()
        {
            MenuItems = new List<MenuItemQuantity>();
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
            MenuItem = new MenuItem(menuItemQuantity.MenuItem);
            Quantity = menuItemQuantity.Quantity;
        }

        public MenuItemQuantity()
        { }
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
            Number = table.Number;
            Description = table.Description;
        }

        public Table()
        { }
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
            FirstName = userContext.FirstName;
            LastName = userContext.LastName;
            Login = userContext.Login;
            Role = userContext.Role;
        }

        public UserContext()
        { }
    }
}
