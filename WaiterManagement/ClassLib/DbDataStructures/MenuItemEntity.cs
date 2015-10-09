using System;
using ClassLib.DataStructures;

namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Klasa modeluj¹ca jeden element z menu.
	/// </summary>
	public class MenuItemEntity : DbEntity, IEquatable<MenuItemEntity>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public virtual MenuItemCategoryEntity Category { get; set; }
		public Money Price { get; set; }

		/// <summary>
		/// Metoda porównuj¹ca wszystkie w³aœciwoœci klasy (oprócz Id)
		/// </summary>
		public bool Equals(MenuItemEntity other)
		{
			if (this.Category == null && other.Category != null)
				return false;
			else if (this.Category != null && other.Category == null)
				return false;
			else if (this.Category != null && other.Category != null)
				if (!this.Category.Equals(other.Category))
					return false;

			return this.Name.Equals(other.Name)
			       && this.Description.Equals(other.Description)
			       && this.Price.Equals(other.Price);
		}

		public void CopyData(MenuItem menuItemTransferObject)
		{
			this.Name = menuItemTransferObject.Name;
			this.Description = menuItemTransferObject.Description;
			this.Price = menuItemTransferObject.Price;
		}
	}
}