using System;
using System.Collections.Generic;
using System.Linq;
using ClassLib.DataStructures;

namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Klasa reprezentuj¹ca zamówienie
	/// </summary>
	public class OrderEntity : DbEntity, IEquatable<OrderEntity>
	{
		public OrderEntity()
		{
			MenuItems = new HashSet<MenuItemQuantityEntity>();
		}

		public UserContextEntity Client { get; set; }
		public virtual UserContextEntity Waiter { get; set; }
		public virtual TableEntity Table { get; set; }
		//Item1 - menuItemId, Item2 - quantity
		public virtual ICollection<MenuItemQuantityEntity> MenuItems { get; set; }
		public OrderState State { get; set; }
		public DateTime PlacingDate { get; set; }
		public DateTime ClosingDate { get; set; }

		public bool Equals(OrderEntity other)
		{
			if(this.MenuItems.Count != other.MenuItems.Count)
				return false;

			if (!this.MenuItems.OrderBy(mI => mI.Id).SequenceEqual(other.MenuItems.OrderBy(mI => mI.Id)))
				return false;

			return this.Client.Equals(other.Client)
			       && this.Waiter.Equals(other.Waiter)
			       && this.Table.Equals(other.Table)
			       && this.State == other.State
			       && this.PlacingDate.Equals(other.PlacingDate)
			       && this.ClosingDate.Equals(other.ClosingDate);
		}
	}
}