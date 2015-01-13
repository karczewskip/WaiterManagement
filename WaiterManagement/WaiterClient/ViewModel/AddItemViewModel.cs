using System.Collections.Generic;
using WaiterClient.Abstract;
using WaiterClient.WaiterDataAccessWCFService;

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
