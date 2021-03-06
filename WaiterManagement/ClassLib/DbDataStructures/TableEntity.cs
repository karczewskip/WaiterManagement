using System;
using ClassLib.DataStructures;

namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Klasa reprezentująca stolik/stół w barze/restauracji
	/// </summary>
	public class TableEntity : DbEntity, IEquatable<TableEntity>
	{
		public int Number { get; set; }
		public string Description { get; set; }

		/// <summary>
		/// Metoda porównująca wszystkie właściwości klasy (oprócz Id)
		/// </summary>
		public bool Equals(TableEntity other)
		{
			return this.Number.Equals(other.Number)
			       && this.Description.Equals(other.Description);
		}

		public void CopyData(Table tableTransferObject)
		{
			this.Number = tableTransferObject.Number;
			this.Description = tableTransferObject.Description;
		}
	}
}