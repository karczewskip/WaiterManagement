using System;
using ClassLib.DataStructures;

namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Klasa reprezentuj¹ca stolik/stó³ w barze/restauracji
	/// </summary>
	public class TableEntity : DbEntity, IEquatable<TableEntity>
	{
		public int Number { get; set; }
		public string Description { get; set; }

		/// <summary>
		/// Metoda porównuj¹ca wszystkie w³aœciwoœci klasy (oprócz Id)
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