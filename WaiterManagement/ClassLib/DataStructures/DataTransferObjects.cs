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
            if(dbEntity == null)
                throw new ArgumentNullException("dbEntity");

            Id = dbEntity.Id;
        }

        /// <summary>
        /// Konstruktor kopiujący
        /// </summary>
        /// <param name="baseTransferObject">obiekt do skopiowania</param>
        public BaseTransferObject(BaseTransferObject baseTransferObject)
        {
            Id = baseTransferObject.Id;
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

        public MenuItem(MenuItem menuItemToCopy)
            :base(menuItemToCopy)
        {
            if(menuItemToCopy == null)
                throw new ArgumentNullException("menuItemToCopy");

            Name = menuItemToCopy.Name;
            Description = menuItemToCopy.Description;
            Category = new MenuItemCategory(menuItemToCopy.Category);
            Price = menuItemToCopy.Price;
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

        /// <summary>
        /// Konstruktor kopiujący
        /// </summary>
        /// <param name="menuItemCategoryToCopy">Kategoria do skopiowania</param>
        public MenuItemCategory(MenuItemCategory menuItemCategoryToCopy)
            :base(menuItemCategoryToCopy)
        {
            if(menuItemCategoryToCopy == null)
                throw new ArgumentNullException("menuItemCategoryToCopy");

            Name = menuItemCategoryToCopy.Name;
            Description = menuItemCategoryToCopy.Description;
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
        public List<MenuItemQuantity> MenuItems { get; set; }
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
                MenuItems = new List<MenuItemQuantity>();
                foreach (var menuItemQuant in order.MenuItems)
                    MenuItems.Add(new MenuItemQuantity(menuItemQuant));
            }
        

            State = order.State;
            PlacingDate = order.PlacingDate;
            ClosingDate = order.ClosingDate;
        }

        /// <summary>
        /// Konstruktor kopiujący
        /// </summary>
        /// <param name="orderToCopy">Zamówienie do skopiowania</param>
        public Order(Order orderToCopy)
            :base(orderToCopy)
        {
            if(orderToCopy == null)
                throw new ArgumentNullException("orderToCopy");

            Client = new UserContext(orderToCopy.Client);

            if (orderToCopy.Waiter != null)
                Waiter = new UserContext(orderToCopy.Waiter);

            if (orderToCopy.Table != null)
                Table = new Table(orderToCopy.Table);

            if (orderToCopy.MenuItems != null)
            {
                MenuItems = new List<MenuItemQuantity>();
                foreach (var menuItemQuant in orderToCopy.MenuItems)
                    MenuItems.Add(new MenuItemQuantity(menuItemQuant));
            }


            State = orderToCopy.State;
            PlacingDate = orderToCopy.PlacingDate;
            ClosingDate = orderToCopy.ClosingDate;
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

        /// <summary>
        /// Konstruktor kopiujący
        /// </summary>
        /// <param name="menuItemQuantityToCopy"> Element zamówienia do skopiowania</param>
        public MenuItemQuantity(MenuItemQuantity menuItemQuantityToCopy)
        {
            MenuItem = new MenuItem((menuItemQuantityToCopy.MenuItem));
            Quantity = menuItemQuantityToCopy.Quantity;
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

        /// <summary>
        /// Konstruktor kopiujący
        /// </summary>
        /// <param name="tableToCopy">Stolik do skopiowania</param>
        public Table(Table tableToCopy)
            :base(tableToCopy)
        {
            if(tableToCopy == null)
                throw new ArgumentNullException("tableToCopy");

            Number = tableToCopy.Number;
            Description = tableToCopy.Description;
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

        /// <summary>
        /// Konstruktor kopiujący
        /// </summary>
        /// <param name="userContextToCopy">Kontekst uzytkownika do skopiowania</param>
        public UserContext(UserContext userContextToCopy)
            :base(userContextToCopy)
        {
            if(userContextToCopy == null)
                throw new ArgumentNullException("userContextToCopy");

            FirstName = userContextToCopy.FirstName;
            LastName = userContextToCopy.LastName;
            Login = userContextToCopy.Login;
            Role = userContextToCopy.Role;
        }

        public UserContext()
        { }
    }
}
