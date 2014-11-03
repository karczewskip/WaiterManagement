﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WaiterClient.Abstract
{
    public interface IArchivedOrdersViewModel
    {
        void InitializeUser(int id);

        void AddArchivedOrder(ClassLib.DbDataStructures.Order SelectedOrder);

        void GetMore();
    }
}
