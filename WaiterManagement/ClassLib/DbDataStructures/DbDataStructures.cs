using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public class MenuItem : DbEntity, IEquatable<MenuItem>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual MenuItemCategory Category { get; set; }
        public Money Price { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(MenuItem other)
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
    }

    /// <summary>
    /// Reprezentuje kategorię, do której należy MenuItem
    /// </summary>
    public class MenuItemCategory : DbEntity, IEquatable<MenuItemCategory>
    {
        public string Name { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(MenuItemCategory other)
        {
            return this.Name.Equals(other.Name)
                && this.Description.Equals(other.Description);
        }
    }

    /// <summary>
    ///Klasa pomocnicza do reprezentująca wartości pieniężne
    /// </summary>
    [ComplexType]
    public class Money: IEquatable<Money>
    {
        public float Amount { get; set; }
        public string Currency { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(Money other)
        {
            return this.Amount.Equals(other.Amount)
                && this.Currency.Equals(other.Currency);
        }
    }

    /// <summary>
    /// Klasa reprezentująca zamówienie
    /// </summary>
    public class Order : DbEntity, IEquatable<Order>
    {
        public Order()
        {
            MenuItems = new HashSet<MenuItemQuantity>();
        }

        public int UserId { get; set; }
        public virtual UserContext Waiter { get; set; }
        public virtual Table Table { get; set; }
        //Item1 - menuItemId, Item2 - quantity
        public virtual ICollection<MenuItemQuantity> MenuItems { get; set; }
        public OrderState State { get; set; }
        public DateTime PlacingDate { get; set; }
        public DateTime ClosingDate { get; set; }

        bool IEquatable<Order>.Equals(Order other)
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
    /// Stan Zamówienia
    /// </summary>
    public enum OrderState
    {
        /// <summary>
        /// Złożone
        /// </summary>
        Placed,
        /// <summary>
        /// Zaakceptowane
        /// </summary>
        Accepted,
        /// <summary>
        /// Zrealizowane
        /// </summary>
        Realized,
        /// <summary>
        /// Nie zrealizowane
        /// </summary>
        NotRealized,
    }

    /// <summary>
    /// Klasa pośrednia pomiędzy zamówieniem a elementem z menu.
    /// </summary>
    public class MenuItemQuantity : DbEntity, IEquatable<MenuItemQuantity>
    {
        public virtual MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(MenuItemQuantity other)
        {
            return this.MenuItem.Equals(other.MenuItem)
                && this.Quantity.Equals(other.Quantity);
        }
    }

    /// <summary>
    /// Klasa reprezentująca stolik/stół w barze/restauracji
    /// </summary>
    public class Table : DbEntity, IEquatable<Table>
    {
        public int Number { get; set; }
        public string Description { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(Table other)
        {
            return this.Number.Equals(other.Number)
                && this.Description.Equals(other.Description);
        }
    }

    public class Password
    {
        [Key]
        public int UserId { get; set; }
        public string Hash { get; set; }
    }

    /// <summary>
    /// Kontekst bazowy wszystkich użytkowników systemu. Jego klasy pochodne są zwracane po udanym zalogowaniu się do systemu
    /// </summary>
    public class UserContext : DbEntity, IEquatable<UserContext>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public UserRole Role { get; set; }

        /// <summary>
        /// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
        /// </summary>
        public bool Equals(UserContext other)
        {
            return this.FirstName.Equals(other.FirstName)
                && this.LastName.Equals(other.LastName)
                && this.Login.Equals(other.Login)
                && this.Role.HasFlag(other.Role);
        }
    }

    /// <summary>
    /// Wyliczenie możliych ról użytkownika
    /// </summary>
    public enum UserRole
    {
        Client = 0x001,
        Waiter = 0x010,
        Manager = 0x100,
    }
}
