using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLib.DbDataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using DataAccess.Migrations;

namespace DataAccess.UnitTests
{
    [TestClass]
    public class DataAccessTests
    {
        IManagerDataAccess managerDataAccess = null;
        IWaiterDataAccess waiterDataAccess = null;

        string waiterFirstName1 = "Don";
        string waiterLastName1 = "Juan";
        string waiterEditedLastName1 = "Perignon";
        string waiterLogin1 = "donjuan";
        string waiterPassword1 = "999";

        WaiterContext waiterContext1 = null;

        string waiterFirstName2 = "Charles";
        string waiterLastName2 = "Bukowski";
        string waiterLogin2 = "chinaski";
        string waiterPassword2 = "hollywood";

        WaiterContext waiterContext2 = null;

        string categoryName1 = "Wykwintne bimbry ziem wschodnich";
        string categoryDescription1 = "Najwybitniejsze selekcja trunków wysokoprocentowych pędzonych po lasach w nocy, gwarantujące niesamowite wrażenia oraz ciężki, ale to bardzo ciężki powrót to domu.";
        string categoryEditedDesciption1 = "Najpodlejsze sikacze i wywary metylowe. Jeżeli chcesz stracić wzrok i wypalić sobie wnętrzności, to są to trunki dla Ciebie.";

        MenuItemCategory category1 = null;

        string menuItemName1 = "Pędzonka DeLuxe Babci Jadzi";
        string menuItemDescription1 = "40 ml trunku tak mocnego, że nie sprzedajemy więcej niż jednej porcji każdemu klientowi.";
        string menuItemEditedDescription1 = "40 ml trunku tak łągodnego, że wypicie mniej niż 10 kielonów jest wstydem i hańbą.";

        Money menuItemPrice1 = new Money() { Amount = 100.99f, Currency = "PLN" };

        MenuItem menuItem1 = null;

        int tableNumber1 = 987;
        string tableDescription1 = "To ta ledwo trzymająca się sterta desek przy kiblu";
        string tableEditedDescription1 = "Wypucowana na błysk tytanowa konstrukcja dla vip-ów";

        Table table1 = null;

        [TestInitialize]
        public void TestInitialize()
        {
            //Upewnienie, że baza posiada najnowszy model danych
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>());

            managerDataAccess = new DataAccessClass();
            waiterDataAccess = new DataAccessClass();
        }

        [TestMethod]
        public void AddNewWaiterTest()
        {
            waiterContext1 = managerDataAccess.AddWaiter(waiterFirstName1, waiterLastName1, waiterLogin1, waiterPassword1);

            Assert.IsNotNull(waiterContext1);
            Assert.AreNotEqual(waiterContext1.Id, 0);
            Assert.AreEqual(waiterContext1.FirstName, waiterFirstName1);
            Assert.AreEqual(waiterContext1.LastName, waiterLastName1);
            Assert.AreEqual(waiterContext1.Login, waiterLogin1);
            Assert.AreEqual(waiterContext1.Password, waiterPassword1);
        }

        [TestMethod]
        public void AddRepeatedWaiterTest()
        {
            if (waiterContext1 == null)
                AddNewWaiterTest();

            try
            {
                //Próba dodania drugiego kelnera o już istniejącym loginem
                waiterContext2 = managerDataAccess.AddWaiter(waiterFirstName2, waiterLastName2, waiterLogin1, waiterPassword2);
                Assert.Fail("waiter2Context should not be created!");
            }
            catch(Exception e)
            {
                Assert.IsTrue(e is ArgumentException);
            }
        }

        [TestMethod]
        public void AddNewCategoryTest()
        {
            category1 = managerDataAccess.AddMenuItemCategory(categoryName1, categoryDescription1);

            Assert.IsNotNull(category1);
            Assert.AreNotEqual(category1.Id, 0);
            Assert.AreEqual(category1.Name, categoryName1);
            Assert.AreEqual(category1.Description, categoryDescription1);
        }

        [TestMethod]
        public void AddNewMenuItemTest()
        {
            if (category1 == null)
                AddNewCategoryTest();

            menuItem1 = managerDataAccess.AddMenuItem(menuItemName1, menuItemDescription1, category1.Id, menuItemPrice1);

            Assert.IsNotNull(menuItem1);
            Assert.AreNotEqual(menuItem1.Id, 0);
            Assert.AreEqual(menuItem1.Name, menuItemName1);
            Assert.AreEqual(menuItem1.Description, menuItemDescription1);
            Assert.IsNotNull(menuItem1.Category);
            Assert.AreEqual(menuItem1.Category.Name, categoryName1);
            Assert.AreEqual(menuItem1.Category.Description, categoryDescription1);
        }

        [TestMethod]
        public void AddNewTableTest()
        {
            table1 = managerDataAccess.AddTable(tableNumber1, tableDescription1);

            Assert.IsNotNull(table1);
            Assert.AreNotEqual(table1.Id, 0);
            Assert.AreEqual(table1.Number, tableNumber1);
            Assert.AreEqual(table1.Description, tableDescription1);
        }

        [TestMethod]
        public void EditMenuItemTest()
        {
            if (menuItem1 == null)
                AddNewMenuItemTest();

            menuItem1.Description = menuItemEditedDescription1;

            bool result = managerDataAccess.EditMenuItem(menuItem1);
            Assert.IsTrue(result);

            var menuItems = managerDataAccess.GetMenuItems();
            Assert.IsTrue(menuItems != null && menuItems.Any());

            var editedMenuItem = menuItems.FirstOrDefault(m => m.Id == menuItem1.Id);
            Assert.IsNotNull(editedMenuItem);

            Assert.AreEqual(editedMenuItem.Description, menuItemEditedDescription1);
        }

        [TestMethod]
        public void EditMenuItemCategoryTest()
        {
            if (category1 == null)
                AddNewCategoryTest();

            category1.Description = categoryEditedDesciption1;
            bool result = managerDataAccess.EditMenuItemCategory(category1);
            Assert.IsTrue(result);

            var categories = managerDataAccess.GetMenuItemCategories();
            Assert.IsTrue(categories != null || categories.Any());

            var editedCategory = categories.FirstOrDefault(c => c.Id == category1.Id);
            Assert.IsNotNull(editedCategory);

            Assert.AreEqual(editedCategory.Description, categoryEditedDesciption1);
        }

        [TestMethod]
        public void EditTableTest()
        {
            if (table1 == null)
                AddNewTableTest();

            table1.Description = tableEditedDescription1;
            bool result = managerDataAccess.EditTable(table1);
            Assert.IsTrue(result);

            var tables = managerDataAccess.GetTables();
            Assert.IsTrue(tables != null && tables.Any());

            var editedTable = tables.FirstOrDefault(t => t.Id == table1.Id);
            Assert.IsNotNull(editedTable);

            Assert.AreEqual(editedTable.Description, tableEditedDescription1);
        }

        [TestMethod]
        public void EditWaiterTest()
        {
            if (waiterContext1 == null)
                AddNewWaiterTest();

            waiterContext1.LastName = waiterEditedLastName1;

            bool result = managerDataAccess.EditWaiter(waiterContext1);
            Assert.IsTrue(result);

            var waiters = managerDataAccess.GetWaiters();
            Assert.IsTrue(waiters != null || waiters.Any());

            var editedWaiter = waiters.FirstOrDefault( w => w.Id == waiterContext1.Id);
            Assert.IsNotNull(editedWaiter);

            Assert.AreEqual(editedWaiter.LastName, waiterEditedLastName1);
        }

        [TestMethod]
        public void RemoveMenuItemCategoryTest()
        {
            if (category1 == null)
                AddNewCategoryTest();

            bool result = managerDataAccess.RemoveMenuItemCategory(category1.Id);
            Assert.IsTrue(result);

            var categories = managerDataAccess.GetMenuItemCategories();
            Assert.IsTrue(categories != null && categories.Any());

            var removedCategory = categories.FirstOrDefault(c => c.Id == category1.Id);
            Assert.IsNull(removedCategory);

            result = managerDataAccess.EditMenuItemCategory(category1);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveMenuItemTest()
        {
            if (menuItem1 == null)
                AddNewMenuItemTest();

            bool result = managerDataAccess.RemoveMenuItem(menuItem1.Id);
            Assert.IsTrue(result);

            var menuItems = managerDataAccess.GetMenuItems();
            Assert.IsTrue(menuItems != null && menuItems.Any());

            var removedMenuItem = menuItems.FirstOrDefault(m => m.Id == menuItem1.Id);
            Assert.IsNull(removedMenuItem);

            result = managerDataAccess.EditMenuItem(menuItem1);
            Assert.IsFalse(result);
        }

        //Odtąd w dół testy do przerobienia

        //[TestMethod]
        //public void RemoveTableTest()
        //{
        //    int tableNumber = 878;
        //    string description = "To ta ledwo trzymająca się sterta desek przy kiblu";
        //    Table newTable = managerDataAccess.AddTable(tableNumber, description);

        //    Assert.IsNotNull(newTable);
        //    Assert.AreNotEqual(newTable.Id, 0);

        //    bool result = managerDataAccess.RemoveTable(newTable.Id);
        //    Assert.IsTrue(result);

        //    IEnumerable<Table> tables = managerDataAccess.GetTables();
        //    IEnumerable<Table> thisShouldBeEmpty = tables.Where(t => t.Id == newTable.Id);
        //    Assert.IsTrue(thisShouldBeEmpty == null || !thisShouldBeEmpty.Any());
        //}

        //[TestMethod]
        //public void RemoveWaiterTest()
        //{
        //    string firstName = "Don";
        //    string lastName = "Guralesko";
        //    string login = "dge";
        //    string password = "bang";

        //    WaiterContext waiterContext = managerDataAccess.AddWaiter(firstName, lastName, login, password);
        //    Assert.IsNotNull(waiterContext);
        //    Assert.AreNotEqual(waiterContext.Id, 0);

        //    bool result = managerDataAccess.RemoveWaiter(waiterContext.Id);

        //    IEnumerable<WaiterContext> waiters = managerDataAccess.GetWaiters();
        //    IEnumerable<WaiterContext> thisShouldBeEmpty = waiters.Where(w => w.Id == waiterContext.Id);
        //    Assert.IsTrue(thisShouldBeEmpty == null || !thisShouldBeEmpty.Any());
        //}

        //[TestMethod]
        //public void CleanUpDatabaseTest()
        //{
        //    IEnumerable<MenuItem> menuItems = managerDataAccess.GetMenuItems();
        //    foreach (MenuItem menuItem in menuItems)
        //        managerDataAccess.RemoveMenuItem(menuItem.Id);
        //    IEnumerable<MenuItemCategory> menuItemCategories = managerDataAccess.GetMenuItemCategories();
        //    foreach (MenuItemCategory category in menuItemCategories)
        //        managerDataAccess.RemoveMenuItemCategory(category.Id);           
        //    IEnumerable<WaiterContext> waiters = managerDataAccess.GetWaiters();
        //    foreach (WaiterContext waiter in waiters)
        //        managerDataAccess.RemoveWaiter(waiter.Id);
        //    IEnumerable<Table> tables = managerDataAccess.GetTables();
        //    foreach (Table table in tables)
        //        managerDataAccess.RemoveTable(table.Id);

        //    menuItemCategories = managerDataAccess.GetMenuItemCategories();
        //    Assert.IsTrue(menuItemCategories == null || !menuItemCategories.Any());
        //    menuItems = managerDataAccess.GetMenuItems();
        //    Assert.IsTrue(menuItems == null || !menuItems.Any());
        //    waiters = managerDataAccess.GetWaiters();
        //    Assert.IsTrue(waiters == null || !waiters.Any());
        //    tables = managerDataAccess.GetTables();
        //    Assert.IsTrue(tables == null || !tables.Any());
        //}

        //[TestMethod]
        //public void WaiterLogInTest()
        //{
        //    CleanUpDatabaseTest();
        //    AddNewWaiterTest();
        //    string login = "dge";
        //    string correctPassword = "bang";
        //    string wrongPassword = "bong";

        //    WaiterContext context = waiterDataAccess.LogIn(login, wrongPassword);
        //    Assert.IsNull(context);
        //    context = waiterDataAccess.LogIn(login, correctPassword);
        //    Assert.IsNotNull(context);
        //}

        //[TestMethod]
        //public void WaiterLogOutTest()
        //{
        //    CleanUpDatabaseTest();
        //    AddNewWaiterTest();
        //    string login = "dge";
        //    string correctPassword = "bang";
        //    string wrongPassword = "bong";

        //    WaiterContext context = waiterDataAccess.LogIn(login, wrongPassword);
        //    Assert.IsNull(context);
        //    context = waiterDataAccess.LogIn(login, correctPassword);
        //    Assert.IsNotNull(context);

        //    bool result = waiterDataAccess.LogOut(context.Id);
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void WaiterAddOrderTest()
        //{
        //    //CleanUpDatabaseTest();
        //    AddNewWaiterTest();
        //    AddNewCategoryTest();
        //    AddNewTableTest();
        //    string login = "dge";
        //    string correctPassword = "bang";
        //    string wrongPassword = "bong";

        //    WaiterContext context = waiterDataAccess.LogIn(login, wrongPassword);
        //    Assert.IsNull(context);
        //    context = waiterDataAccess.LogIn(login, correctPassword);
        //    Assert.IsNotNull(context);

        //    string name1 = "Pędzonka DeLuxe Babci Jadzi";
        //    string description1 = "40 ml trunku tak mocnego, że nie sprzedajemy więcej niż jednej porcji każdemu klientowi.";
        //    Money price1 = new Money() { Amount = 100.99f, Currency = "PLN" };

        //    string name2 = "Pędzonka Standard Babci Jadzi";
        //    string description2 = "40 ml trunku mocnego, acz nie za mocnego. Można śmiało częstować się kilkoma głębszymi";
        //    Money price2 = new Money() { Amount = 30f, Currency = "PLN" };

        //    string name3 = "Pędzonka Eco Babci Jadzi";
        //    string description3 = "40 ml trunku, na tyle słabego, że w zasadzie nie warto sobie nim nawet zawracać głowy.";
        //    Money price3 = new Money() { Amount = 10f, Currency = "PLN" };

        //    IEnumerable<MenuItemCategory> categories = managerDataAccess.GetMenuItemCategories();
        //    Assert.IsNotNull(categories);
        //    Assert.IsTrue(categories.Any());

        //    MenuItem newMenuItem1 = managerDataAccess.AddMenuItem(name1, description1, categories.First().Id, price1);

        //    Assert.IsNotNull(newMenuItem1);
        //    Assert.AreNotEqual(newMenuItem1.Id, 0);

        //    MenuItem newMenuItem2 = managerDataAccess.AddMenuItem(name2, description2, categories.First().Id, price2);
        //    Assert.IsNotNull(newMenuItem1);
        //    Assert.AreNotEqual(newMenuItem2.Id, 0);

        //    MenuItem newMenuItem3 = managerDataAccess.AddMenuItem(name3, description3, categories.First().Id, price3);
        //    Assert.IsNotNull(newMenuItem3);
        //    Assert.AreNotEqual(newMenuItem3.Id, 0);

        //    IEnumerable<Table> tables = waiterDataAccess.GetTables();
        //    Assert.IsNotNull(tables);
        //    Assert.IsTrue(tables.Any());

        //    List<Tuple<int, int>> menuItems = new List<Tuple<int, int>>();
        //    menuItems.Add(new Tuple<int,int>(newMenuItem1.Id, 3));
        //    menuItems.Add(new Tuple<int, int>(newMenuItem2.Id, 10));
        //    menuItems.Add(new Tuple<int, int>(newMenuItem3.Id, 30));

        //    Order newOrder = waiterDataAccess.AddOrder(500, tables.First().Id, context.Id, menuItems);
        //    Assert.IsNotNull(newOrder);
        //    Assert.AreNotEqual(newOrder.Id, 0);

        //}

        //[TestMethod]
        //public void GetPastOrdersTest()
        //{
        //    //CleanUpDatabaseTest();
        //    AddNewWaiterTest();
        //    AddNewCategoryTest();
        //    AddNewTableTest();
        //    string login = "dge";
        //    string correctPassword = "bang";
        //    string wrongPassword = "bong";

        //    WaiterContext context = waiterDataAccess.LogIn(login, wrongPassword);
        //    Assert.IsNull(context);
        //    context = waiterDataAccess.LogIn(login, correctPassword);
        //    Assert.IsNotNull(context);

        //    string name1 = "Pędzonka DeLuxe Babci Jadzi";
        //    string description1 = "40 ml trunku tak mocnego, że nie sprzedajemy więcej niż jednej porcji każdemu klientowi.";
        //    Money price1 = new Money() { Amount = 100.99f, Currency = "PLN" };

        //    string name2 = "Pędzonka Standard Babci Jadzi";
        //    string description2 = "40 ml trunku mocnego, acz nie za mocnego. Można śmiało częstować się kilkoma głębszymi";
        //    Money price2 = new Money() { Amount = 30f, Currency = "PLN" };

        //    string name3 = "Pędzonka Eco Babci Jadzi";
        //    string description3 = "40 ml trunku, na tyle słabego, że w zasadzie nie warto sobie nim nawet zawracać głowy.";
        //    Money price3 = new Money() { Amount = 10f, Currency = "PLN" };

        //    IEnumerable<MenuItemCategory> categories = managerDataAccess.GetMenuItemCategories();
        //    Assert.IsNotNull(categories);
        //    Assert.IsTrue(categories.Any());

        //    MenuItem newMenuItem1 = managerDataAccess.AddMenuItem(name1, description1, categories.First().Id, price1);

        //    Assert.IsNotNull(newMenuItem1);
        //    Assert.AreNotEqual(newMenuItem1.Id, 0);

        //    MenuItem newMenuItem2 = managerDataAccess.AddMenuItem(name2, description2, categories.First().Id, price2);
        //    Assert.IsNotNull(newMenuItem1);
        //    Assert.AreNotEqual(newMenuItem2.Id, 0);

        //    MenuItem newMenuItem3 = managerDataAccess.AddMenuItem(name3, description3, categories.First().Id, price3);
        //    Assert.IsNotNull(newMenuItem3);
        //    Assert.AreNotEqual(newMenuItem3.Id, 0);

        //    IEnumerable<Table> tables = waiterDataAccess.GetTables();
        //    Assert.IsNotNull(tables);
        //    Assert.IsTrue(tables.Any());

        //    List<Tuple<int, int>> menuItems = new List<Tuple<int, int>>();
        //    menuItems.Add(new Tuple<int, int>(newMenuItem1.Id, 3));
        //    menuItems.Add(new Tuple<int, int>(newMenuItem2.Id, 10));
        //    menuItems.Add(new Tuple<int, int>(newMenuItem3.Id, 30));

        //    Order newOrder = waiterDataAccess.AddOrder(500, tables.First().Id, context.Id, menuItems);
        //    Assert.IsNotNull(newOrder);
        //    Assert.AreNotEqual(newOrder.Id, 0);

        //    newOrder = waiterDataAccess.AddOrder(500, tables.First().Id, context.Id, menuItems);
        //    Assert.IsNotNull(newOrder);
        //    Assert.AreNotEqual(newOrder.Id, 0);

        //    newOrder = waiterDataAccess.AddOrder(500, tables.First().Id, context.Id, menuItems);
        //    Assert.IsNotNull(newOrder);
        //    Assert.AreNotEqual(newOrder.Id, 0);

        //    IEnumerable<Order> pastOrders = waiterDataAccess.GetPastOrders(context.Id);
        //    Assert.IsNotNull(pastOrders);
        //    Assert.AreEqual(pastOrders.Count(o => true), 3);
        //}

        [TestCleanup]
        public void TestCleanup()
        {
            if (waiterContext1 != null)
                managerDataAccess.RemoveWaiter(waiterContext1.Id);

            if (waiterContext2 != null)
                managerDataAccess.RemoveWaiter(waiterContext2.Id);

            if (menuItem1 != null)
                managerDataAccess.RemoveMenuItem(menuItem1.Id);

            if (category1 != null)
                managerDataAccess.RemoveMenuItemCategory(category1.Id);            

            if (table1 != null)
                managerDataAccess.RemoveTable(table1.Id);

            waiterContext1 = null;
            waiterContext2 = null;
            category1 = null;
            menuItem1 = null;
            table1 = null;
        }
    }
}
