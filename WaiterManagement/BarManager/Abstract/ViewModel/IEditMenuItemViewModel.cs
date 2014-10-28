﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarManager.Abstract
{
    public interface IEditMenuItemViewModel
    {
        void RefreshItem(ClassLib.DbDataStructures.MenuItem menuItem);

        bool EditMenuItem(out string error);
    }
}
