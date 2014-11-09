using ClassLib.DbDataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WaiterClient.Abstract;

namespace WaiterClient.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za dodawanie pozycji w menu do zamówienia
    /// </summary>
    public class AddItemViewModel: IAddItemViewModel
    {
        private IOrderWindowViewModel OrderWidnowViewModel;
        private IAddOrderViewModel AddOrderViewModel;

        public IList<MenuItem> ListOfMenuItems { get { return OrderWidnowViewModel.ListOfMenuItems; } }

        public AddItemViewModel(IOrderWindowViewModel orderWindowViewModel, IAddOrderViewModel addOrderViewModel)
        {
            AddOrderViewModel = addOrderViewModel;

            OrderWidnowViewModel = orderWindowViewModel;


        }

        public void AddItem(MenuItem menuItem)
        {
            AddOrderViewModel.AddItem(menuItem);
        }
    }
}
