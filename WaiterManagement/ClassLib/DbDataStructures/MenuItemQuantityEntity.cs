using System;
using System.Runtime.Serialization;

namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Klasa pośrednia pomiędzy zamówieniem a elementem z menu.
	/// </summary>
	[DataContract]
	public class MenuItemQuantityEntity : DbEntity, IEquatable<MenuItemQuantityEntity>
	{
		[DataMember]
		public virtual MenuItemEntity MenuItem { get; set; }
		[DataMember]
		public int Quantity { get; set; }

		/// <summary>
		/// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
		/// </summary>
		public bool Equals(MenuItemQuantityEntity other)
		{
			return this.MenuItem.Equals(other.MenuItem)
			       && this.Quantity.Equals(other.Quantity);
		}
	}
}