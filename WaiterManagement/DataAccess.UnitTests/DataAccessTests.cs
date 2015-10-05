using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using DataAccess.Migrations;
using ClassLib.DataStructures;
using ClassLib.DbDataStructures;

namespace DataAccess.UnitTests
{
	[TestClass]
	public class DataAccessTests
	{
		#region Private Fields

		private IManagerDataAccess _managerDataAccess = null;
		private IWaiterDataAccess _waiterDataAccess = null;
		private IClientDataAccess _clientDataAccess = null;

		#endregion

		#region Test Initialization & CleanUp

		[TestInitialize]
		public void TestInitialize()
		{
			//Upewnienie, że baza posiada najnowszy model danych
			Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>());

			var dataAccessClass = new DataAccessClass();
			_managerDataAccess = dataAccessClass;
			_clientDataAccess = dataAccessClass;
			_waiterDataAccess = dataAccessClass;

			using (var db = new DataAccessProvider())
			{
				db.Database.ExecuteSqlCommand("DELETE FROM MenuItemQuantityEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM OrderEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM UserContextEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM PasswordEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM MenuItemEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM MenuItemCategoryEntities");
				db.Database.ExecuteSqlCommand("DELETE FROM TableEntities");
			}

		}


		#endregion

		#region Manager Method Tests

		[TestMethod]
		public void AddNewManagerTest()
		{
			var managerContext = _managerDataAccess.AddManager("Managerfirstname", "Managerlastname", "Managerlogin",
				HashClass.CreateFirstHash("Managerpassword", "Managerlogin"));

			Assert.IsNotNull(managerContext);
			Assert.AreNotEqual(managerContext.Id, 0);
			Assert.AreEqual("Managerfirstname", managerContext.FirstName);
			Assert.AreEqual("Managerlastname", managerContext.LastName);
			Assert.AreEqual(UserRole.Manager, managerContext.Role);
		}

		[TestMethod]
		public void ManagerLogInTest()
		{
			// Arrange manager
			_managerDataAccess.AddManager("Managerfirstname", "Managerlastname", "Managerlogin",
				HashClass.CreateFirstHash("Managerpassword", "Managerlogin"));
			
			// Act logging
			UserContext context = _managerDataAccess.LogIn("Managerlogin",
				HashClass.CreateFirstHash("Managerpassword", "Managerlogin"));

			// Asserts
			Assert.IsNotNull(context);
			Assert.AreEqual("Managerlogin", context.Login);
			Assert.AreEqual(UserRole.Manager, context.Role);

		}

		[TestMethod]
		public void ManagerLogOutTest()
		{
			// Arrange manager
			var managerContext = PrepareManager();

			// Act
			bool result = _managerDataAccess.LogOut(managerContext.Id);

			// Assert
			Assert.IsTrue(result);
		}

		private UserContext PrepareManager()
		{
			_managerDataAccess.AddManager("Managerfirstname", "Managerlastname", "Managerlogin",
				HashClass.CreateFirstHash("Managerpassword", "Managerlogin"));
			var managerContext = _managerDataAccess.LogIn("Managerlogin",
				HashClass.CreateFirstHash("Managerpassword", "Managerlogin"));
			return managerContext;
		}

		[TestMethod]
		public void AddNewWaiterTest()
		{
			// Arrange manager
			var managerContext = PrepareManager();

			// Act
			var waiterContext = _managerDataAccess.AddWaiter(managerContext.Id, "Waiterfirstname", "Waiterlastname",
				"Waiterlogin", "Waiterpassword");

			// Asserts
			Assert.IsNotNull(waiterContext);
			Assert.AreNotEqual(waiterContext.Id, 0);
			Assert.AreEqual(waiterContext.FirstName, "Waiterfirstname");
			Assert.AreEqual(waiterContext.LastName, "Waiterlastname");
			Assert.AreEqual(waiterContext.Login, "Waiterlogin");
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void AddRepeatedWaiterTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			_managerDataAccess.AddWaiter(managerContext.Id, "Waiterrepeatedname", "Waiterrepeatedlastname",
					"Waiterrepeatedlogin", "Waiterrepeatedpassword");

			// Act
			_managerDataAccess.AddWaiter(managerContext.Id, "Waiterrepeatedname", "Waiterrepeatedlastname",
					"Waiterrepeatedlogin", "Waiterrepeatedpassword");
		}

		[TestMethod]
		public void AddNewCategoryTest()
		{
			// Arrange
			var managerContext = PrepareManager();

			// Act
			var category = _managerDataAccess.AddMenuItemCategory(managerContext.Id, "categoryName", "categoryDescription");

			// Assert
			Assert.IsNotNull(category);
			Assert.AreNotEqual(category.Id, 0);
			Assert.AreEqual(category.Name, "categoryName");
			Assert.AreEqual(category.Description, "categoryDescription");
		}

		[TestMethod]
		public void AddNewMenuItemTest()
		{
			// Arrange
			var managerContext = PrepareManager();

			// Act
			var category = _managerDataAccess.AddMenuItemCategory(managerContext.Id, "categoryName", "categoryDescription");
			var menuItem = _managerDataAccess.AddMenuItem(managerContext.Id,"menuItemName", "menuItemDescription",
				category.Id, new Money() { Amount = 100.99f, Currency = "PLN" });

			Assert.IsNotNull(menuItem);
			Assert.AreNotEqual(menuItem.Id, 0);
			Assert.AreEqual(menuItem.Name, "menuItemName");
			Assert.AreEqual(menuItem.Description, "menuItemDescription");
			Assert.IsNotNull(menuItem.Category);
			Assert.AreEqual(menuItem.Category.Name, "categoryName");
			Assert.AreEqual(menuItem.Category.Description, "categoryDescription");
		}

		[TestMethod]
		public void AddNewTableTest()
		{
			// Arrange
			var managerContext = PrepareManager();

			// Act
			var table = _managerDataAccess.AddTable(managerContext.Id, 723, "tableDescription");

			// Asserts
			Assert.IsNotNull(table);
			Assert.AreNotEqual(table.Id, 0);
			Assert.AreEqual(table.Number, 723);
			Assert.AreEqual(table.Description, "tableDescription");
		}

		[TestMethod]
		public void EditMenuItemTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var menuItem = PrepareMenuItem();

			// Act
			menuItem.Description = "EditedDescription";
			bool result = _managerDataAccess.EditMenuItem(managerContext.Id, menuItem);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.AreEqual("EditedDescription",db.MenuItems.Single(x => x.Id == menuItem.Id).Description);
			}
		}

		private MenuItem PrepareMenuItem()
		{
			using (var db = new DataAccessProvider())
			{
				var category = db.MenuItemCategories.Add(new MenuItemCategoryEntity());
				var menuItem = db.MenuItems.Add(new MenuItemEntity(){Name = "MenuItemName",Description = "MenuItemDescription",Category = category, Price = new Money(){Amount = 3, Currency = "PLN"}});

				db.SaveChanges();

				return new MenuItem(menuItem);
			}
		}

		private MenuItemCategory PrepareMEnuItemCategory()
		{
			using (var db = new DataAccessProvider())
			{
				var category = db.MenuItemCategories.Add(new MenuItemCategoryEntity());

				db.SaveChanges();

				return new MenuItemCategory(category);
			}
		}

		[TestMethod]
		public void EditMenuItemCategoryTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var menuItemCategory = PrepareMEnuItemCategory();

			// Act
			menuItemCategory.Description = "EditedDescription";
			bool result = _managerDataAccess.EditMenuItemCategory(managerContext.Id, menuItemCategory);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.AreEqual("EditedDescription", db.MenuItemCategories.Single(x => x.Id == menuItemCategory.Id).Description);
			}
		}

		[TestMethod]
		public void EditTableTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var table = PrepareTable();

			// Act
			table.Description = "EditedDescription";
			bool result = _managerDataAccess.EditTable(managerContext.Id, table);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.AreEqual("EditedDescription", db.Tables.Single(x => x.Id == table.Id).Description);
			}
		}

		private Table PrepareTable()
		{
			using (var db = new DataAccessProvider())
			{
				var table = db.Tables.Add(new TableEntity());

				db.SaveChanges();

				return new Table(table);
			}
		}

		[TestMethod]
		public void EditWaiterTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var waiter = PrepareWaiter();

			// Act
			waiter.Login = "EditedLogin";
			bool result = _managerDataAccess.EditWaiter(managerContext.Id, waiter);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.AreEqual("EditedLogin", db.Users.Single(x => x.Id == waiter.Id).Login);
			}
		}

		private UserContext PrepareWaiter()
		{
			using (var db = new DataAccessProvider())
			{
				var waiter = db.Users.Add(new UserContextEntity(){Role = UserRole.Waiter});

				db.SaveChanges();

				return new UserContext(waiter);
			}
		}

		[TestMethod]
		public void RemoveMenuItemCategoryTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var menuItemCategory = PrepareMEnuItemCategory();

			// Act
			bool result = _managerDataAccess.RemoveMenuItemCategory(managerContext.Id, menuItemCategory.Id);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.IsTrue(db.MenuItemCategories.First(x => x.Id == menuItemCategory.Id).IsDeleted);
			}
		}

		[TestMethod]
		public void RemoveMenuItemTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var menuItem = PrepareMenuItem();

			// Act
			bool result = _managerDataAccess.RemoveMenuItem(managerContext.Id, menuItem.Id);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.IsTrue(db.MenuItems.First(x => x.Id == menuItem.Id).IsDeleted);
			}
		}

		[TestMethod]
		public void RemoveTableTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var table = PrepareTable();

			// Act
			bool result = _managerDataAccess.RemoveTable(managerContext.Id, table.Id);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.IsTrue(db.Tables.First(x => x.Id == table.Id).IsDeleted);
			}
		}

		[TestMethod]
		public void RemoveWaiterTest()
		{
			// Arrange
			var managerContext = PrepareManager();
			var waiter = PrepareWaiter();

			// Act
			bool result = _managerDataAccess.RemoveWaiter(managerContext.Id, waiter.Id);

			// Assert
			Assert.IsTrue(result);

			using (var db = new DataAccessProvider())
			{
				Assert.IsTrue(db.Users.First(x => x.Id == waiter.Id).IsDeleted);
			}
		}

		#endregion

		#region Waiter Method Tests

		[TestMethod]
		public void WaiterLogInTest()
		{
			// Arrange
			var manager = PrepareManager();
			_managerDataAccess.AddWaiter(manager.Id,"Waiterfirstname", "Waiterlastname", "Waiterlogin",
				HashClass.CreateFirstHash("Waiterpassword", "Waiterlogin"));

			// Act logging
			UserContext context = _waiterDataAccess.LogIn("Waiterlogin",
				HashClass.CreateFirstHash("Waiterpassword", "Waiterlogin"));

			// Asserts
			Assert.IsNotNull(context);
			Assert.AreEqual("Waiterlogin", context.Login);
			Assert.AreEqual(UserRole.Waiter, context.Role);
		}

		[TestMethod]
		public void WaiterLogOutTest()
		{
			// Arrange
			var manager = PrepareManager();
			_managerDataAccess.AddWaiter(manager.Id, "Waiterfirstname", "Waiterlastname", "Waiterlogin",
				HashClass.CreateFirstHash("Waiterpassword", "Waiterlogin"));
			UserContext context = _waiterDataAccess.LogIn("Waiterlogin",
				HashClass.CreateFirstHash("Waiterpassword", "Waiterlogin"));

			bool result = _waiterDataAccess.LogOut(context.Id);
			Assert.IsTrue(result);
		}

		#endregion

		#region Client Method Tests
		[TestMethod]
		public void ClientLogInTest()
		{
			// Arrange
			_clientDataAccess.AddClient("Clientfirstname", "Clientlastname", "Clientlogin",
				HashClass.CreateFirstHash("Clientpassword", "Clientlogin"));

			// Act logging
			UserContext context = _clientDataAccess.LogIn("Clientlogin",
				HashClass.CreateFirstHash("Clientpassword", "Clientlogin"));

			// Asserts
			Assert.IsNotNull(context);
			Assert.AreEqual("Clientlogin", context.Login);
			Assert.AreEqual(UserRole.Client, context.Role);
		}

		[TestMethod]
		public void ClientLogOutTest()
		{
			// Arrange
			_clientDataAccess.AddClient("Clientfirstname", "Clientlastname", "Clientlogin",
				HashClass.CreateFirstHash("Clientpassword", "Clientlogin"));
			UserContext context = _clientDataAccess.LogIn("Clientlogin",
				HashClass.CreateFirstHash("Clientpassword", "Clientlogin"));

			// Act
			bool result = _clientDataAccess.LogOut(context.Id);

			// Assert
			Assert.IsTrue(result);
		}

		[TestMethod]
		public void AddNewClient()
		{
			// Act
			_clientDataAccess.AddClient("ClientFirstName", "ClientLastName", "ClientLogin",
				HashClass.CreateFirstHash("ClientPassword", "ClientLogin"));

			// Asserts
			using (var db = new DataAccessProvider())
			{
				var clientContext = db.Users.First(x => x.Login == "ClientLogin");

				Assert.IsNotNull(clientContext);
				Assert.AreNotEqual(clientContext.Id, 0);
				Assert.AreEqual(clientContext.FirstName, "ClientFirstName");
				Assert.AreEqual(clientContext.LastName, "ClientLastName");
				Assert.AreEqual(clientContext.Role, UserRole.Client);
			}
		}

		[TestMethod]

		public void ClientAddOrderTest()
		{
			// Arrange
			var client = PrepareClient();
			var menuItem = PrepareMenuItem();
			var time = DateTime.Now.AddDays(3);

			var menuItems = new List<Tuple<int, int>>
			{
				new Tuple<int, int>(menuItem.Id, 3)
			};

			// Act
			var order = _clientDataAccess.AddOrder(client.Id, time , menuItems);

			// Asserts
			Assert.IsNotNull(order);

			Assert.AreEqual(menuItem.Name,order.MenuItems.First().MenuItem.Name);
			Assert.AreEqual(3, order.MenuItems.First().Quantity);
		}

		private UserContext PrepareClient()
		{
			_clientDataAccess.AddClient("Clientfirstname", "Clientlastname", "Clientlogin",
				HashClass.CreateFirstHash("Clientpassword", "Clientlogin"));
			return _clientDataAccess.LogIn("Clientlogin",
				HashClass.CreateFirstHash("Clientpassword", "Clientlogin"));
		}

		#endregion
	}
}
