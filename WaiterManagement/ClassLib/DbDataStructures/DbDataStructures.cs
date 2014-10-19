using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

///Zbiór klas bazodanowych
namespace ClassLib.DbDataStructures
{
    /// <summary>
    /// Klasa modelująca jeden element z menu.
    /// </summary>
    public class MenuItem
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual MenuItemCategory Category { get; set; }
        public Money Price { get; set; }

        public void CopyData(MenuItem original)
        {
            Name = original.Name;
            Description = original.Description;
            Category = original.Category;
            Price = original.Price;
        }
    }

    /// <summary>
    /// Reprezentuje kategorię, do której należy MenuItem
    /// </summary>
    public class MenuItemCategory
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public void CopyData(MenuItemCategory original)
        {
            Name = original.Name;
            Description = original.Description;
        }
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
        public virtual IEnumerable<MenuItem> MenuItems { get; private set; }
    }

    /// <summary>
    /// Klasa reprezentująca stolik/stół w barze/restauracji
    /// </summary>
    public class Table
    {
        public int Id { get; private set; }
        public int Number { get; set; }
        public string Description { get; set; }

        public void CopyData(Table original)
        {
            Number = original.Number;
            Description = original.Description;
        }
    }

    /// <summary>
    /// Reprezentuje dane kelnera. Jest zwracana po udanym zalogowaniu się
    /// </summary>
    public class WaiterContext
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public void CopyData(WaiterContext original)
        {
            FirstName = original.FirstName;
            LastName = original.LastName;
            Login = original.Login;
            Password = original.Password;
        }
    }
}
