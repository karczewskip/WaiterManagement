using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaiterClient.Abstract
{
    public interface IOrderWindowViewModel
    {
        IList<Table> ListOfTables { get; }
        IList<MenuItem> ListOfMenuItems { get; }
        IList<MenuItemCategory> ListOfCategories { get; }

        void LogOut();

        bool InitializeUser(int id, out string error);

        bool AddNewOrder(Table SelectedTable, IList<MenuItemQuantity> ListOfItems, out string error);

        bool CancelOrder(out string error);
    }
}
