using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

///Zbiór klas bazodanowych
namespace ClassLib.DbDataStructures
{
    /// <summary>
    /// Klasa bazowa dla klas bazodanowych
    /// </summary>
    [DataContract]
    public class DbEntity
    {
        [DataMember]
        public int Id { get; private set; }
        [DataMember]
        public bool IsDeleted { get; set; }
    }

    /// <summary>
    /// Klasa modelująca jeden element z menu.
    /// </summary>
    [DataContract]
    public class MenuItem : DbEntity, IEquatable<MenuItem>
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public virtual MenuItemCategory Category { get; set; }
        [DataMember]
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
    [DataContract]
    public class MenuItemCategory : DbEntity, IEquatable<MenuItemCategory>
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
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
    [DataContract]
    public class Money: IEquatable<Money>
    {
        [DataMember]
        public float Amount { get; set; }
        [DataMember]
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
    [DataContract]
    public class Order : DbEntity, IEquatable<Order>
    {
        public Order()
        {
            MenuItems = new HashSet<MenuItemQuantity>();
        }

        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public virtual UserContext Waiter { get; set; }
        [DataMember]
        public virtual Table Table { get; set; }
        //Item1 - menuItemId, Item2 - quantity
        [DataMember]
        public virtual ICollection<MenuItemQuantity> MenuItems { get; set; }
        [DataMember]
        public OrderState State { get; set; }
        [DataMember]
        public DateTime PlacingDate { get; set; }
        [DataMember]
        public DateTime ClosingDate { get; set; }

        public bool Equals(Order other)
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
    [DataContract]
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
    [DataContract]
    public class MenuItemQuantity : DbEntity, IEquatable<MenuItemQuantity>
    {
        [DataMember]
        public virtual MenuItem MenuItem { get; set; }
        [DataMember]
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
    [DataContract]
    public class Table : DbEntity, IEquatable<Table>
    {
        [DataMember]
        public int Number { get; set; }
        [DataMember]
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
    [DataContract]
    public class UserContext : DbEntity, IEquatable<UserContext>
    {
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Login { get; set; }
        [DataMember]
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
    [DataContract]
    public enum UserRole
    {
        Client = 0x001,
        Waiter = 0x010,
        Manager = 0x100,
    }
}
