using System;
using System.Collections.Generic;
using System.Linq;

namespace ClassLib.DataStructures
{
	public class Cart
	{
		private readonly List<MenuItemQuantity> lineCollection = new List<MenuItemQuantity>();

		public string Currency
		{
			get { return ApplicationResources.DefaultCurrency; }
		}

		public IEnumerable<MenuItemQuantity> Lines
		{
			get { return lineCollection; }
		}

		public void AddItem(MenuItem menuItem, int quantity)
		{
			var line = lineCollection.FirstOrDefault(p => p.MenuItem.Id == menuItem.Id);
			if (line == null)
			{
				lineCollection.Add(new MenuItemQuantity
				{
					MenuItem = menuItem,
					Quantity = quantity
				});
			}
			else
			{
				line.Quantity += quantity;
			}
		}

		public void RemoveLine(MenuItem menuItem)
		{
			if(lineCollection.All(l => l.MenuItem.Id != menuItem.Id))
				throw new ArgumentException("No this type of item");

			lineCollection.RemoveAll(l => l.MenuItem.Id == menuItem.Id);
		}

		public decimal ComputeTotalValue()
		{
			return lineCollection.Sum(e => (decimal) (e.MenuItem.Price.Amount*e.Quantity));
		}

		public void Clear()
		{
			lineCollection.Clear();
		}
	}
}