namespace ClassLib.DbDataStructures
{
	/// <summary>
	/// Klasa bazowa dla klas bazodanowych
	/// </summary>
	public class DbEntity
	{
		public int Id { get; private set; }
		public bool IsDeleted { get; set; }
	}
}