using System.ComponentModel.DataAnnotations;

namespace ClassLib.DbDataStructures
{
	public class PasswordEntity
	{
		[Key]
		public int Id { get; private set; }
		public int UserId { get; set; }
		public string Hash { get; set; }
	}
}