using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;

namespace BarManager.Abstract
{
    public interface IMenuManagerViewModel
    {
        IList<MenuItem> AllMenuItems { get; set; }
        IList<MenuItemCategory> Categories { get; set; }
        IList<MenuItemCategory> AvailableCategories { get; set; }

        bool DeleteSelectedItem(out string error);


        void ShowCurrentCategory(MenuItemCategory category);

        void AddNewMenuItem(MenuItem addingMenuItem);

        void AddCategory(MenuItemCategory addingCategory);
    }
}
