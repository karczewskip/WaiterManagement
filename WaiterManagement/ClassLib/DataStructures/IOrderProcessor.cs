namespace ClassLib.DataStructures
{
	public interface IProcessOrderCommand
	{
		bool Execute(Cart cart, OrderDetails orderDetails);
	}
}