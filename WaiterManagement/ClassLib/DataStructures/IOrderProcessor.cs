namespace ClassLib.DataStructures
{
    public interface IOrderProcessor
    {
        void ProcessOrder(Cart cart, OrderDetails orderDetails); 
    }
}