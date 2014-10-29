using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;
using DataAccess;
using ClassLib.DbDataStructures;

namespace WaiterClient.ViewModel
{
    public class OrderWindowViewModel : IOrderWindowViewModel
    {
        private IWaiterClientModel WaiterClientModel;

        private int WaiterId;

        public IList<Table> ListOfTables { get; set; }
        public IList<MenuItem> ListOfMenuItems { get; set; }
        public IList<MenuItemCategory> ListOfCategories { get; set; }

        public OrderWindowViewModel(IWaiterClientModel waiterClientModel)
        {
            WaiterClientModel = waiterClientModel;

            Initialize();
        }

        private void Initialize()
        {
            ListOfTables = WaiterClientModel.GetTables();
            ListOfMenuItems = WaiterClientModel.GetMenuItems();
            ListOfCategories = WaiterClientModel.GetCategories();
        }

        public void LogOut()
        {
            WaiterClientModel.LogOut(WaiterId);
        }


        public void InitializeUser(int id)
        {
            WaiterId = id;
        }
    }
}
