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
    public class MenuItem : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual MenuItemCategory Category { get; set; }
        public Money Price { get; set; }
    }

    /// <summary>
    /// Reprezentuje kategorię, do której należy MenuItem
    /// </summary>
    public class MenuItemCategory : DbEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    ///Klasa pomocnicza do reprezentująca wartości pieniężne
    /// </summary>
    [ComplexType]
    public class Money
    {
        public float Amount { get; set; }
        public string Currency { get; set; }
    }

    /// <summary>
    /// Klasa reprezentująca zamówienie
    /// </summary>
    public class Order : DbEntity
    {
        public Order()
        {
            MenuItems = new HashSet<MenuItemQuantity>();
        }

        public int UserId { get; set; }
        public virtual WaiterContext Waiter { get; set; }
        public virtual Table Table { get; set; }
        //Item1 - menuItemId, Item2 - quantity
        public virtual ICollection<MenuItemQuantity> MenuItems { get; set; }
        public OrderState State { get; set; }
        public DateTime PlacingDate { get; set; }
        public DateTime ClosingDate { get; set; }
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
    public class MenuItemQuantity : DbEntity
    {
        public virtual MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
    }

    /// <summary>
    /// Klasa reprezentująca stolik/stół w barze/restauracji
    /// </summary>
    public class Table : DbEntity
    {
        public int Number { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Kontekst bazowy wszystkich użytkowników systemu. Jego klasy pochodne są zwracane po udanym zalogowaniu się do systemu
    /// </summary>
    public abstract class UserContext : DbEntity
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }

    /// <summary>
    /// Reprezentuje dane klienta. 
    /// </summary>
    public class ClientContext : UserContext
    {
         
    }

    /// <summary>
    /// Reprezentuje dane kelnera.
    /// </summary>
    public class WaiterContext : UserContext
    {

    }

    /// <summary>
    /// Reprezentuje dane menedżera lokalu.
    /// </summary>
    public class ManagerContext : UserContext
    {

    }
}
