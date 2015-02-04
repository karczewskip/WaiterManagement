using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;

namespace ClassLib.DataStructures
{   
    /// <summary>
    /// Klasa sprawdzająca regularnie, czy nie nadszedł już czas na znalezienie kelnera dla zamówienia
    /// </summary>
    public class OrderAssignScheduler : IDisposable
    {
        #region Private Fields
        private long checkingInterval;
        private Func<Order, bool> orderAssignFunc;
        private Timer checkingTimer;
        private HashSet<OrderServicingDateWrapper> orders;
        private object ordersLockObject = new object();
        #endregion

        #region Constructors
        public OrderAssignScheduler(long checkingInterval, Func<Order, bool> orderAssignFunc )
        {
            if(checkingInterval <= 0)
                throw new ArgumentException("Checking interval has to be > 0", "checkingInterval");

            if(orderAssignFunc == null)
                throw new ArgumentNullException("orderAssignFunc");

            this.checkingInterval = checkingInterval;
            this.orderAssignFunc = orderAssignFunc;

            orders = new HashSet<OrderServicingDateWrapper>();

            checkingTimer = new Timer();
            checkingTimer.Interval = checkingInterval;
            checkingTimer.Elapsed += checkingTimer_Elapse;
        }
        #endregion

        #region Public Methods
        public void AddOrder(OrderServicingDateWrapper orderServicingDateWrapper)
        {
            lock (ordersLockObject)
                orders.Add(orderServicingDateWrapper);
        }
        #endregion

        #region Event Handlers
        private void checkingTimer_Elapse(object sender, ElapsedEventArgs args)
        {
            if (orders.Count == 0)
                return;

            IList<OrderServicingDateWrapper> toRemove = new List<OrderServicingDateWrapper>();

            foreach (var orderDateWrapper in orders)
                if (orderDateWrapper.ServicingDate.CompareTo(DateTime.Now) < 0)
                {
                    if(orderAssignFunc(orderDateWrapper.Order))
                        toRemove.Add(orderDateWrapper);
                }

            lock(ordersLockObject)
                foreach (var orderDataWrapper in toRemove)
                    orders.Remove(orderDataWrapper);
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
            checkingTimer.Elapsed -= checkingTimer_Elapse;
        }
        #endregion
    }

    /// <summary>
    /// Klasa owijająca zamówienie wraz z datą, w której zamówienie ma być obsłużone
    /// </summary>
    public class OrderServicingDateWrapper
    {
        public Order Order { get; private set; }
        public DateTime ServicingDate { get; private set; }

        public OrderServicingDateWrapper(Order order, DateTime servicingDate)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if(servicingDate.CompareTo(DateTime.Now) < 0)
                throw new ArgumentException("The specified servicing date is in the past", "servicingDate");

            Order = new Order(order);
            ServicingDate = servicingDate;
        }
    }
}
