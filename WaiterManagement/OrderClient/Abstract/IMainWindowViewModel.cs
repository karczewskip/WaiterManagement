using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient.Abstract
{
    public interface IMainWindowViewModel
    {
        void CancelOrder();

        void LogIn();

        void StartGettingOrders();

        void ShowPayingDialog();

        void CloseOrder();
    }
}
