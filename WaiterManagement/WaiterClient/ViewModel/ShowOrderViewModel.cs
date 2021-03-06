﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using WaiterClient.Abstract;
using WaiterClient.WaiterDataAccessWCFService;

namespace WaiterClient.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za pokazywanie danych zamówienia
    /// </summary>
    public class ShowOrderViewModel : IShowOrderViewModel, INotifyPropertyChanged
    {
        private Order CurrentOrder;

        public string TableNumberString
        {
            get
            {
                if (CurrentOrder == null || CurrentOrder.Table == null)
                    return "";

                return CurrentOrder.Table.Number.ToString();
            }
        }

        public IList<MenuItemQuantity> ListOfItems
        {
            get
            {
                if (CurrentOrder == null || CurrentOrder.MenuItems == null)
                    return null;

                return CurrentOrder.MenuItems.ToList();
            }
        }

        public void RefreshOrder(Order order)
        {
            CurrentOrder = order;

            if (null != this.PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs("TableNumberString"));
                PropertyChanged(this, new PropertyChangedEventArgs("ListOfItems"));
            }

        }

        # region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion
    }
}
