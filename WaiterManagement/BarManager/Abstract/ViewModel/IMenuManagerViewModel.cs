﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLib.DbDataStructures;

namespace BarManager.Abstract
{
    public interface IMenuManagerViewModel
    {
        IList<MenuItem> ListOfMenuItems { get; set; }
        IList<MenuItemCategory> ListOfCategories { get; set; }
        IList<MenuItemCategory> ShowingCategories { get; set; }

        bool DeleteSelectedItem(out string error);


        void ShowCurrentCategory(MenuItemCategory category);

        void AddNewMenuItem(MenuItem addingMenuItem);
    }
}
