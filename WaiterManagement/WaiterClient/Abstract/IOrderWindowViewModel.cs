using System.Collections.Generic;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.Abstract
{
    public interface IOrderWindowViewModel
    {
        IList<Table> ListOfTables { get; }
        IList<MenuItem> ListOfMenuItems { get; }
        IList<MenuItemCategory> ListOfCategories { get; }

        void LogOut();

        bool InitializeUser(out string error);

        bool CancelOrder(out string error);

        bool RealizeOrder(out string error);
    }
}
