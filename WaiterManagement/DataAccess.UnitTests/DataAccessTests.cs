using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLib.DbDataStructures;
using System.Collections;
using System.Collections.Generic;

namespace DataAccess.UnitTests
{
    [TestClass]
    public class DataAccessTests
    {
        IManagerDataAccess managerDataAccess = new DataAccess();

        [TestMethod]
        public void AddNewWaiterTest()
        {
            string firstName = "Don";
            string lastName = "Guralesko";
            string login = "dge";
            string password = "bang";

            WaiterContext waiterContext = managerDataAccess.AddWaiter(firstName, lastName, login, password);
            Assert.IsNotNull(waiterContext);
            Assert.AreNotEqual(waiterContext.Id, 0);
        }

        [TestMethod]
        public void AddNewCategoryTest()
        {
            string name = "Wykwintne bimbry ziem wschodnich";
            string description = "Najwybitniejsze selekcja trunków wysokoprocentowych pędzonych po lasach w nocy, gwarantujące niesamowite wrażenia oraz ciężki, ale to bardzo ciężki powrót to domu.";

            MenuItemCategory newMenuItemCategory = managerDataAccess.AddMenuItemCategory(name, description);
            Assert.IsNotNull(newMenuItemCategory);
            Assert.AreNotEqual(newMenuItemCategory.Id, 0);
        }

        [TestMethod]
        public void AddNewMenuItemTest()
        {
            string name = "Pędzonka DeLuxe Babci Jadzi";
            string description = "40 ml trunku tak mocnego, że nie sprzedajemy więcej niż jednej porcji każdemu klientowi.";
            Money price = new Money() { Amount = 100.99f, Currency = "PLN" };

            IEnumerable<MenuItemCategory> categories = managerDataAccess.GetMenuItemCategories();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Any());

            MenuItem newMenuItem = managerDataAccess.AddMenuItem(name, description, categories.First(), price);

            Assert.IsNotNull(newMenuItem);
            Assert.AreNotEqual(newMenuItem.Id, 0);

        }

        [TestMethod]
        public void AddNewTableTest()
        {
            int tableNumber = 878;
            string description = "To ta ledwo trzymająca się sterta desek przy kiblu";
            Table newTable = managerDataAccess.AddTable(tableNumber, description);

            Assert.IsNotNull(newTable);
            Assert.AreNotEqual(newTable.Id, 0);
        }

        [TestMethod]
        public void EditMenuItemTest()
        {
            string name = "Pędzonka DeLuxe Babci Jadzi";
            string description = "40 ml trunku tak mocnego, że nie sprzedajemy więcej niż jednej porcji każdemu klientowi.";
            Money price = new Money() { Amount = 100.99f, Currency = "PLN" };

            IEnumerable<MenuItemCategory> categories = managerDataAccess.GetMenuItemCategories();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Any());

            MenuItem newMenuItem = managerDataAccess.AddMenuItem(name, description, categories.First(), price);

            Assert.IsNotNull(newMenuItem);
            Assert.AreNotEqual(newMenuItem.Id, 0);

            string newDescription = "40 ml trunku tak łągodnego, że wypicie mniej niż 10 kielonów jest wstydem i hańbą.";
            
            newMenuItem.Description = newDescription;
            bool result = managerDataAccess.EditMenuItem(newMenuItem);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EditMenuItemCategoryTest()
        {
            string name = "Wykwintne bimbry ziem wschodnich";
            string description = "Najwybitniejsze selekcja trunków wysokoprocentowych pędzonych po lasach w nocy, gwarantujące niesamowite wrażenia oraz ciężki, ale to bardzo ciężki powrót to domu.";

            MenuItemCategory newMenuItemCategory = managerDataAccess.AddMenuItemCategory(name, description);
            Assert.IsNotNull(newMenuItemCategory);
            Assert.AreNotEqual(newMenuItemCategory.Id, 0);

            string newDescription = "Najpodlejsze sikacze i wywary metylowe. Jeżeli chcesz stracić wzrok i wypalić sobie wnętrzności, to są trunki dla Ciebie.";

            newMenuItemCategory.Description = newDescription;
            bool result = managerDataAccess.EditMenuItemCategory(newMenuItemCategory);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void EditTableTest()
        {
            int oldNumber = 999;
            string oldDescription = "oldDescription";

            Table table = managerDataAccess.AddTable(oldNumber, oldDescription);
            Assert.IsNotNull(table);
            Assert.AreNotEqual(table.Id, 0);

            int newNumber = 666;
            string newDescription = "newDescription";

            table.Number = newNumber;
            table.Description = newDescription;

            bool result = managerDataAccess.EditTable(table);
            Assert.IsTrue(result);   
        }

        [TestMethod]
        public void EditWaiterTest()
        {
            string oldFirstName = "oldFirstName";
            string oldLastName = "oldLastName";
            string oldlogin = "oldLogin";
            string oldpassword = "oldpassword";

            WaiterContext waiterContext = managerDataAccess.AddWaiter(oldFirstName, oldLastName, oldlogin, oldpassword);
            Assert.IsNotNull(waiterContext);
            Assert.AreNotEqual(waiterContext.Id, 0);
            
            string newFirstName = "Bonus";
            string newLastName = "BGC";           

            waiterContext.FirstName = newFirstName;
            waiterContext.LastName = newLastName;

            bool result = managerDataAccess.EditWaiter(waiterContext);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void RemoveMenuItemCategoryTest()
        {
            string name = "Wykwintne bimbry ziem wschodnich";
            string description = "Najwybitniejsze selekcja trunków wysokoprocentowych pędzonych po lasach w nocy, gwarantujące niesamowite wrażenia oraz ciężki, ale to bardzo ciężki powrót to domu.";

            MenuItemCategory newMenuItemCategory = managerDataAccess.AddMenuItemCategory(name, description);
            Assert.IsNotNull(newMenuItemCategory);
            Assert.AreNotEqual(newMenuItemCategory.Id, 0);

            bool result = managerDataAccess.RemoveMenuItemCategory(newMenuItemCategory.Id);
            Assert.IsTrue(result);

            IEnumerable<MenuItemCategory> menuItemCategories = managerDataAccess.GetMenuItemCategories();
            IEnumerable<MenuItemCategory> thisShouldBeEmpty = menuItemCategories.Where(m => m.Id == newMenuItemCategory.Id);
            Assert.IsTrue(thisShouldBeEmpty == null || !thisShouldBeEmpty.Any());
        }

        [TestMethod]
        public void RemoveMenuItemTest()
        {
            string name = "Pędzonka DeLuxe Babci Jadzi";
            string description = "40 ml trunku tak mocnego, że nie sprzedajemy więcej niż jednej porcji każdemu klientowi.";
            Money price = new Money() { Amount = 100.99f, Currency = "PLN" };

            IEnumerable<MenuItemCategory> categories = managerDataAccess.GetMenuItemCategories();
            Assert.IsNotNull(categories);
            Assert.IsTrue(categories.Any());

            MenuItem newMenuItem = managerDataAccess.AddMenuItem(name, description, categories.First(), price);

            Assert.IsNotNull(newMenuItem);
            Assert.AreNotEqual(newMenuItem.Id, 0);

            bool result = managerDataAccess.RemoveMenuItem(newMenuItem.Id);
            Assert.IsTrue(result);

            IEnumerable<MenuItem> menuItems = managerDataAccess.GetMenuItems();
            IEnumerable<MenuItem> thisShouldBeEmpty = menuItems.Where(m => m.Id == newMenuItem.Id);
            Assert.IsTrue(thisShouldBeEmpty == null || !thisShouldBeEmpty.Any());
        }

        [TestMethod]
        public void RemoveTableTest()
        {
            int tableNumber = 878;
            string description = "To ta ledwo trzymająca się sterta desek przy kiblu";
            Table newTable = managerDataAccess.AddTable(tableNumber, description);

            Assert.IsNotNull(newTable);
            Assert.AreNotEqual(newTable.Id, 0);

            bool result = managerDataAccess.RemoveTable(newTable.Id);
            Assert.IsTrue(result);

            IEnumerable<Table> tables = managerDataAccess.GetTables();
            IEnumerable<Table> thisShouldBeEmpty = tables.Where(t => t.Id == newTable.Id);
            Assert.IsTrue(thisShouldBeEmpty == null || !thisShouldBeEmpty.Any());
        }

        [TestMethod]
        public void RemoveWaiterTest()
        {
            string firstName = "Don";
            string lastName = "Guralesko";
            string login = "dge";
            string password = "bang";

            WaiterContext waiterContext = managerDataAccess.AddWaiter(firstName, lastName, login, password);
            Assert.IsNotNull(waiterContext);
            Assert.AreNotEqual(waiterContext.Id, 0);

            bool result = managerDataAccess.RemoveWaiter(waiterContext.Id);

            IEnumerable<WaiterContext> waiters = managerDataAccess.GetWaiters();
            IEnumerable<WaiterContext> thisShouldBeEmpty = waiters.Where(w => w.Id == waiterContext.Id);
            Assert.IsTrue(thisShouldBeEmpty == null || !thisShouldBeEmpty.Any());
        }

        [TestMethod]
        public void CleanUpDatabaseTest()
        {
            IEnumerable<MenuItem> menuItems = managerDataAccess.GetMenuItems();
            foreach (MenuItem menuItem in menuItems)
                managerDataAccess.RemoveMenuItem(menuItem.Id);
            IEnumerable<MenuItemCategory> menuItemCategories = managerDataAccess.GetMenuItemCategories();
            foreach (MenuItemCategory category in menuItemCategories)
                managerDataAccess.RemoveMenuItemCategory(category.Id);           
            IEnumerable<WaiterContext> waiters = managerDataAccess.GetWaiters();
            foreach (WaiterContext waiter in waiters)
                managerDataAccess.RemoveWaiter(waiter.Id);
            IEnumerable<Table> tables = managerDataAccess.GetTables();
            foreach (Table table in tables)
                managerDataAccess.RemoveTable(table.Id);

            menuItemCategories = managerDataAccess.GetMenuItemCategories();
            Assert.IsTrue(menuItemCategories == null || !menuItemCategories.Any());
            menuItems = managerDataAccess.GetMenuItems();
            Assert.IsTrue(menuItems == null || !menuItems.Any());
            waiters = managerDataAccess.GetWaiters();
            Assert.IsTrue(waiters == null || !waiters.Any());
            tables = managerDataAccess.GetTables();
            Assert.IsTrue(tables == null || !tables.Any());
        }
    }
}
