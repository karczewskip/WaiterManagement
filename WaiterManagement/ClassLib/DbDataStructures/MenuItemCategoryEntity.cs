using System;
using ClassLib.DataStructures;

namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Reprezentuje kategorię, do której należy MenuItem
	/// </summary>
	public class MenuItemCategoryEntity : DbEntity, IEquatable<MenuItemCategoryEntity>
	{
		public string Name { get; set; }
		public string Description { get; set; }

		/// <summary>
		/// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
		/// </summary>
		public bool Equals(MenuItemCategoryEntity other)
		{
			return this.Name.Equals(other.Name)
			       && this.Description.Equals(other.Description);
		}

		public void CopyData(MenuItemCategory menuItemCategoryTransferObject)
		{
			this.Name = menuItemCategoryTransferObject.Name;
			this.Description = menuItemCategoryTransferObject.Description;
		}
	}
}