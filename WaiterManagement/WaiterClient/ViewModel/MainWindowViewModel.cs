﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WaiterClient.Abstract;

namespace WaiterClient.ViewModel
{
    /// <summary>
    /// Klasa odpowiedzialna za możliwość logowania
    /// </summary>
    public class MainWindowViewModel : IMainWindowViewModel
    {
        private IOrderWindowViewModel OrderWindowViewModel;
        private IWaiterClientModel WaiterClientModel;

        public MainWindowViewModel(IWaiterClientModel waiterClientModel,IOrderWindowViewModel orderWindowViewModel)
        {
            WaiterClientModel = waiterClientModel;
            OrderWindowViewModel = orderWindowViewModel;
        }

        public bool LoginUser(string login, string password, out string error)
        {
            if(WaiterClientModel.LogInUser(login, password))
            {
                var result = OrderWindowViewModel.InitializeUser(out error);
                
                if(!result)
                    WaiterClientModel.LogOut();
                return result;
            }
            else
            {
                error = "User login or password was wrong";
                return false;
            }
        }
    }
}
