using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfServiceLib
{
    /// <summary>
    /// Klasa modelująca jeden element z menu.
    /// </summary>
    [DataContract]
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }
        public virtual MenuItemCategory Category { get; set; }
        public Money Price { get; set; }
    }

    /// <summary>
    /// Reprezentuje kategorię, do której należy MenuItem
    /// </summary>
    [DataContract]
    public class MenuItemCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    /// <summary>
    /// Klasa pomocnicza do reprezentująca wartości pieniężne
    /// </summary>
    [DataContract]
    [ComplexType]
    public class Money
    {
        public float Amount { get; set; }
        public string Currency { get; set; }
    }

    /// <summary>
    /// Klasa reprezentująca zamówienie
    /// </summary>
    [DataContract]
    public class Order
    {
        public Order()
        {
            MenuItems = new HashSet<MenuItem>();
        }
        public int Id { get; private set; }
        public int UserId { get; private set; }
        public int WaiterId { get; private set; }
        public int TableId { get; private set; }
        public IEnumerable<MenuItem> MenuItems { get; private set; }
    }

    /// <summary>
    /// Klasa reprezentująca stolik/stół w barze/restauracji
    /// </summary>
    public class Table
    {
        public int Id { get; private set; }
        public int Number { get; private set; }
        public string Description { get; private set; }
    }

    /// <summary>
    /// Reprezentuje dane kelnera. Jest zwracana po udanym zalogowaniu się
    /// </summary>
    [DataContract]
    public class WaiterContext
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
    }
}
