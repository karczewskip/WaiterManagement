﻿namespace OrderClient.Abstract
{
    internal interface ICurrentOrder
    {
        void RefreshOrder();
        void SetOrderWindowReference(IOrderViewModel orderViewModel);
    }
}