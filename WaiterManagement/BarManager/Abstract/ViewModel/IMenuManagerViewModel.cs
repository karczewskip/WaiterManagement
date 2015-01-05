using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarManager.ManagerDataAccessWCFService;
using Caliburn.Micro;

namespace BarManager.Abstract
{
    public interface IMenuManagerViewModel
    {
        IList<MenuItem> AllMenuItems { get; set; }
        BindableCollection<MenuItem> MenuItems { get; set; }
        IList<MenuItemCategory> Categories { get; set; }
        IList<MenuItemCategory> AvailableCategories { get; set; }

        void DeleteItem();


        void ShowCurrentCategory(MenuItemCategory category);

        void AddNewMenuItem(MenuItem addingMenuItem);

        void AddCategoryToViewModel(MenuItemCategory addingCategory);

        void CloseDialogs();
    }
}
