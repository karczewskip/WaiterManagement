using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using ClassLib.DataStructures;

///Zbiór klas bazodanowych
namespace ClassLib.DbDataStructures
{
    /// <summary>
    /// Klasa bazowa dla klas bazodanowych
    /// </summary>
    public class DbEntity
    {
        public int Id { get; private set; }
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Klasa modelująca jeden element z menu.
    /// </summary>
    public class MenuItemEntity : DbEntity, IEquatable<MenuItemEntity>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual MenuItemCategoryEntity Category { get; set; }
        public Money Price { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(MenuItemEntity other)
        {
            if (this.Category == null && other.Category != null)
                return false;
            else if (this.Category != null && other.Category == null)
                return false;
            else if (this.Category != null && other.Category != null)
                if (!this.Category.Equals(other.Category))
                    return false;

            return this.Name.Equals(other.Name)
                && this.Description.Equals(other.Description)
                && this.Price.Equals(other.Price);
        }

        public void CopyData(MenuItem menuItemTransferObject)
        {
            this.Name = menuItemTransferObject.Name;
            this.Description = menuItemTransferObject.Description;
            this.Price = menuItemTransferObject.Price;
        }
    }

    /// <summary>
    /// Reprezentuje kategorię, do której należy MenuItem
    /// </summary>
    public class MenuItemCategoryEntity : DbEntity, IEquatable<MenuItemCategoryEntity>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(MenuItemCategoryEntity other)
        {
            return this.Name.Equals(other.Name)
                && this.Description.Equals(other.Description);
        }

        public void CopyData(MenuItemCategory menuItemCategoryTransferObject)
        {
            this.Name = menuItemCategoryTransferObject.Name;
            this.Description = menuItemCategoryTransferObject.Description;
        }
    }

    /// <summary>
    /// Klasa reprezentująca zamówienie
    /// </summary>
    public class OrderEntity : DbEntity, IEquatable<OrderEntity>
    {
        public OrderEntity()
        {
            MenuItems = new HashSet<MenuItemQuantityEntity>();
        }

        public int UserId { get; set; }
        public virtual UserContextEntity Waiter { get; set; }
        public virtual TableEntity Table { get; set; }
        //Item1 - menuItemId, Item2 - quantity
        public virtual ICollection<MenuItemQuantityEntity> MenuItems { get; set; }
        public OrderState State { get; set; }
        public DateTime PlacingDate { get; set; }
        public DateTime ClosingDate { get; set; }

        public bool Equals(OrderEntity other)
        {
            if(this.MenuItems.Count != other.MenuItems.Count)
                return false;

            if (!this.MenuItems.OrderBy(mI => mI.Id).SequenceEqual(other.MenuItems.OrderBy(mI => mI.Id)))
                return false;

            return this.UserId.Equals(other.UserId)
                && this.Waiter.Equals(other.Waiter)
                && this.Table.Equals(other.Table)
                && this.State == other.State
                && this.PlacingDate.Equals(other.PlacingDate)
                && this.ClosingDate.Equals(other.ClosingDate);
        }
    }

    /// <summary>
    /// Klasa pośrednia pomiędzy zamówieniem a elementem z menu.
    /// </summary>
    [DataContract]
    public class MenuItemQuantityEntity : DbEntity, IEquatable<MenuItemQuantityEntity>
    {
        [DataMember]
        public virtual MenuItemEntity MenuItem { get; set; }
        [DataMember]
        public int Quantity { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(MenuItemQuantityEntity other)
        {
            return this.MenuItem.Equals(other.MenuItem)
                && this.Quantity.Equals(other.Quantity);
        }
    }

    /// <summary>
    /// Klasa reprezentująca stolik/stół w barze/restauracji
    /// </summary>
    public class TableEntity : DbEntity, IEquatable<TableEntity>
    {
        public int Number { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(TableEntity other)
        {
            return this.Number.Equals(other.Number)
                && this.Description.Equals(other.Description);
        }

        public void CopyData(Table tableTransferObject)
        {
            this.Number = tableTransferObject.Number;
            this.Description = tableTransferObject.Description;
        }
    }

    public class PasswordEntity
    {
        [Key]
        public int Id { get; private set; }
        public int UserId { get; set; }
        public string Hash { get; set; }
    }

    /// <summary>
    /// Kontekst bazowy wszystkich użytkowników systemu. Jego klasy pochodne są zwracane po udanym zalogowaniu się do systemu
    /// </summary>
    public class UserContextEntity : DbEntity, IEquatable<UserContextEntity>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public UserRole Role { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(UserContextEntity other)
        {
            return this.FirstName.Equals(other.FirstName)
                && this.LastName.Equals(other.LastName)
                && this.Login.Equals(other.Login)
                && this.Role.HasFlag(other.Role);
        }

        public void CopyData(UserContext userContextTransferObject)
        {
            this.FirstName = userContextTransferObject.FirstName;
            this.LastName = userContextTransferObject.LastName;
            this.Login = userContextTransferObject.Login;
            this.Role = userContextTransferObject.Role;
        }
    }
}
