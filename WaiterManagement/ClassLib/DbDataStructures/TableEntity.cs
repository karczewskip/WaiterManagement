using System;
using ClassLib.DataStructures;

namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Klasa reprezentuj�ca stolik/st� w barze/restauracji
	/// </summary>
	public class TableEntity : DbEntity, IEquatable<TableEntity>
	{
		public int Number { get; set; }
		public string Description { get; set; }

		/// <summary>
		/// Metoda por�wnuj�ca wszystkie w�a�ciwo�ci klasy (opr�cz Id)
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