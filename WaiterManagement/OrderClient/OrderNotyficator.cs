using OrderClient.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderClient
{
    class OrderNotyficator : IOrderNotyficator
    {
        private IOrderViewModel _orderViewModel;

        static private OrderNotyficator instance;

        private OrderNotyficator()
        {

        }

        public static IOrderNotyficator GetInstance()
        {
            if(instance == null)
            {
                instance = new OrderNotyficator();
            }

            return instance;
        }

        public void SetTarget(IOrderViewModel orderViewModel)
        {
            _orderViewModel = orderViewModel;
        }
    }
}
