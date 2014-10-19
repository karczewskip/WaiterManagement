using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Collections.ObjectModel;

namespace BarManager.ViewModel
{
    public class MenuManagerViewModel : IMenuManagerViewModel
    {
        public IList<MenuItem> ListOfMenuItems { get; set; }

        public MenuManagerViewModel()
        {
            ListOfMenuItems = new ObservableCollection<MenuItem>();

            ListOfMenuItems.Add(new MenuItem() { Id = 1, Name = "Piwo", Category = new MenuItemCategory() { Id = 1, Name = "Drinks" }, Description = "0,5L", Price = new Money() { Amount = 6, Currency = "zl" } });
            ListOfMenuItems.Add(new MenuItem() { Id = 2, Name = "Wódka", Category = new MenuItemCategory() { Id = 1, Name = "Drinks" }, Description = "0,5L", Price = new Money() { Amount = 30, Currency = "zl" } });
        }
        
    }
}
