using System;
using ClassLib.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ClassLib.Tests.DataStructures
{
	[TestClass]
	public class OrderCartTests
	{
		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemoveLines_no_this_type_of_menu_items()
		{
			//Arrange
			var cart = new Cart();
			var testMenuItem = new MenuItem();

			//Act
			cart.RemoveLine(testMenuItem);
		}
	}
}