using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ClassLib.DbDataStructures;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using DataAccess.Migrations;
using System.Security;
using ClassLib.DataStructures;

namespace DataAccess.UnitTests
{
    [TestClass]
    public class DataAccessTests
    {
        #region Private Fields
        private IManagerDataAccess managerDataAccess = null;
        private IWaiterDataAccess waiterDataAccess = null;
        private IClientDataAccess clientDataAccess = null;
        private IDataWipe dataWipe = null;

        private const string ManagerFirstName1 = "Mana";
        private const string ManagerLastName1 = "Dżer";
        private const string ManagerLogin1 = "admini";
        private const string ManagerPassword1 = "admini";
        private UserContext managerContext1 = null;

        private const string ClientFirstName1 = "Anonimowa";
        private const string ClientLastName1 = "Kapibara";
        private const string ClientLogin1 = "anonKap";
        private const string ClientPassword1 = "kapPass";
        private UserContext clientContext1 = null;

        string waiterFirstName1 = "Don";
        string waiterLastName1 = "Juan";
        string waiterEditedLastName1 = "Perignon";
        string waiterLogin1 = "donjuan";
        string waiterPassword1 = "999";
        UserContext waiterContext1 = null;

        string waiterFirstName2 = "Charles";
        string waiterLastName2 = "Bukowski";
        string waiterLogin2 = "chinaski";
        string waiterPassword2 = "hollywood";
        UserContext waiterContext2 = null;

        string categoryName1 = "Wykwintne bimbry ziem wschodnich";
        string categoryDescription1 = "Najwybitniejsze selekcja trunków wysokoprocentowych pędzonych po lasach w nocy, gwarantujące niesamowite wrażenia oraz ciężki, ale to bardzo ciężki powrót to domu.";
        string categoryEditedDesciption1 = "Najpodlejsze sikacze i wywary metylowe. Jeżeli chcesz stracić wzrok i wypalić sobie wnętrzności, to są to trunki dla Ciebie.";
        MenuItemCategory category1 = null;

        string menuItemName1 = "Pędzonka DeLuxe Babci Jadzi";
        string menuItemDescription1 = "40 ml trunku tak mocnego, że nie sprzedajemy więcej niż jednej porcji każdemu klientowi.";
        string menuItemEditedDescription1 = "40 ml trunku tak łagodnego, że wypicie mniej niż 10 kielonów jest wstydem i hańbą.";
        Money menuItemPrice1 = new Money() { Amount = 100.99f, Currency = "PLN" };
        int menuItemQuantity1 = 30;
        MenuItem menuItem1 = null;

        string menuItemName2 = "Pędzonka Standard Babci Jadzi";
        string menuItemDescription2 = "40 ml trunku mocnego, acz nie za mocnego. Można śmiało częstować się kilkoma głębszymi";
        Money menuItemPrice2 = new Money() { Amount = 30f, Currency = "PLN" };
        int menuItemQuantity2 = 15;
        MenuItem menuItem2 = null;

        string menuItemName3 = "Pędzonka Eco Babci Jadzi";
        string menuItemDescription3 = "40 ml trunku, na tyle słabego, że w zasadzie nie warto sobie nim nawet zawracać głowy.";
        Money menuItemPrice3 = new Money() { Amount = 10f, Currency = "PLN" };
        int menuItemQuantity3 = 10;
        MenuItem menuItem3 = null;

        int tableNumber1 = 987;
        string tableDescription1 = "To ta ledwo trzymająca się sterta desek przy kiblu";
        string tableEditedDescription1 = "Wypucowana na błysk tytanowa konstrukcja dla vip-ów";
        Table table1 = null;

        Order order1 = null;
        #endregion

        #region Test Initialization & CleanUp

        [TestInitialize]
        public void TestInitialize()
        {
            //Upewnienie, że baza posiada najnowszy model danych
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>());

            var dataAccessClass = new DataAccessClass();
            managerDataAccess = dataAccessClass;
            waiterDataAccess = dataAccessClass;
            clientDataAccess = dataAccessClass;
            dataWipe = dataAccessClass;
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (order1 != null)
                dataWipe.WipeOrder(order1.Id);

            if (managerContext1 != null)
                dataWipe.WipeUser(managerContext1.Id);

            if (clientContext1 != null)
                dataWipe.WipeUser(clientContext1.Id);

            if (waiterContext1 != null)
                dataWipe.WipeUser(waiterContext1.Id);

            if (waiterContext2 != null)
                dataWipe.WipeUser(waiterContext2.Id);

            if (menuItem1 != null)
                dataWipe.WipeMenuItem(menuItem1.Id);

            if (menuItem2 != null)
                dataWipe.WipeMenuItem(menuItem2.Id);

            if (menuItem3 != null)
                dataWipe.WipeMenuItem(menuItem3.Id);

            if (category1 != null)
                dataWipe.WipeMenuItemCategory(category1.Id);

            if (table1 != null)
                dataWipe.WipeTable(table1.Id);

            order1 = null;
            managerContext1 = null;
            waiterContext1 = null;
            waiterContext2 = null;
            clientContext1 = null;
            category1 = null;
            menuItem1 = null;
            menuItem2 = null;
            menuItem3 = null;
            table1 = null;
        }

        #endregion

        #region Manager Method Tests

        [TestMethod]
        public void AddNewManagerTest()
        {
            managerContext1 = managerDataAccess.AddManager(ManagerFirstName1, ManagerLastName1, ManagerLogin1,
                HashClass.CreateFirstHash(ManagerPassword1, ManagerLogin1));
            Assert.IsNotNull(managerContext1);
            Assert.AreNotEqual(managerContext1.Id, 0);
            Assert.AreEqual(managerContext1.FirstName, ManagerFirstName1);
            Assert.AreEqual(managerContext1.LastName, ManagerLastName1);
            Assert.AreEqual(managerContext1.Role, UserRole.Manager);
        }

        [TestMethod]
        public void ManagerLogInTest()
        {
            if (managerContext1 == null)
                AddNewManagerTest();

            UserContext context = managerDataAccess.LogIn(ManagerLogin1,
                HashClass.CreateFirstHash(ManagerPassword1, ManagerLogin1));
            Assert.IsNotNull(context);
            Assert.AreEqual(context.Login, ManagerLogin1);
            Assert.AreEqual(context.Role, UserRole.Manager);

        }

        [TestMethod]
        public void ManagerLogOutTest()
        {
            if (managerContext1 != null)
            {
                bool result = managerDataAccess.LogOut(managerContext1.Id);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void AddNewWaiterTest()
        {
            ManagerLogInTest();

            waiterContext1 = managerDataAccess.AddWaiter(managerContext1.Id, waiterFirstName1, waiterLastName1,
                waiterLogin1, waiterPassword1);

            Assert.IsNotNull(waiterContext1);
            Assert.AreNotEqual(waiterContext1.Id, 0);
            Assert.AreEqual(waiterContext1.FirstName, waiterFirstName1);
            Assert.AreEqual(waiterContext1.LastName, waiterLastName1);
            Assert.AreEqual(waiterContext1.Login, waiterLogin1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void AddRepeatedWaiterTest()
        {
            if (waiterContext1 == null)
                AddNewWaiterTest();
            ManagerLogInTest();

            try
            {
                //Próba dodania drugiego kelnera o już istniejącym loginem
                waiterContext2 = managerDataAccess.AddWaiter(managerContext1.Id, waiterFirstName2, waiterLastName2,
                    waiterLogin1, waiterPassword2);
                Assert.Fail("waiter2Context should not be created!");
            }
            catch (Exception e)
            {
                Assert.IsTrue(e is ArgumentException);
            }

            ManagerLogOutTest();
        }

        [TestMethod]
        public void AddNewCategoryTest()
        {
            ManagerLogInTest();

            category1 = managerDataAccess.AddMenuItemCategory(managerContext1.Id, categoryName1, categoryDescription1);

            Assert.IsNotNull(category1);
            Assert.AreNotEqual(category1.Id, 0);
            Assert.AreEqual(category1.Name, categoryName1);
            Assert.AreEqual(category1.Description, categoryDescription1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void AddNewMenuItemTest()
        {
            if (category1 == null)
                AddNewCategoryTest();

            ManagerLogInTest();

            menuItem1 = managerDataAccess.AddMenuItem(managerContext1.Id, menuItemName1, menuItemDescription1,
                category1.Id, menuItemPrice1);
            menuItem2 = managerDataAccess.AddMenuItem(managerContext1.Id, menuItemName2, menuItemDescription2,
                category1.Id, menuItemPrice2);
            menuItem3 = managerDataAccess.AddMenuItem(managerContext1.Id, menuItemName3, menuItemDescription3,
                category1.Id, menuItemPrice3);

            Assert.IsNotNull(menuItem1);
            Assert.AreNotEqual(menuItem1.Id, 0);
            Assert.AreEqual(menuItem1.Name, menuItemName1);
            Assert.AreEqual(menuItem1.Description, menuItemDescription1);
            Assert.IsNotNull(menuItem1.Category);
            Assert.AreEqual(menuItem1.Category.Name, categoryName1);
            Assert.AreEqual(menuItem1.Category.Description, categoryDescription1);

            Assert.IsNotNull(menuItem2);
            Assert.AreNotEqual(menuItem2.Id, 0);
            Assert.AreEqual(menuItem2.Name, menuItemName2);
            Assert.AreEqual(menuItem2.Description, menuItemDescription2);
            Assert.IsNotNull(menuItem2.Category);
            Assert.AreEqual(menuItem2.Category.Name, categoryName1);
            Assert.AreEqual(menuItem2.Category.Description, categoryDescription1);

            Assert.IsNotNull(menuItem3);
            Assert.AreNotEqual(menuItem3.Id, 0);
            Assert.AreEqual(menuItem3.Name, menuItemName3);
            Assert.AreEqual(menuItem3.Description, menuItemDescription3);
            Assert.IsNotNull(menuItem3.Category);
            Assert.AreEqual(menuItem3.Category.Name, categoryName1);
            Assert.AreEqual(menuItem3.Category.Description, categoryDescription1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void AddNewTableTest()
        {
            ManagerLogInTest();

            table1 = managerDataAccess.AddTable(managerContext1.Id, tableNumber1, tableDescription1);

            Assert.IsNotNull(table1);
            Assert.AreNotEqual(table1.Id, 0);
            Assert.AreEqual(table1.Number, tableNumber1);
            Assert.AreEqual(table1.Description, tableDescription1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void EditMenuItemTest()
        {
            if (menuItem1 == null)
                AddNewMenuItemTest();

            ManagerLogInTest();

            menuItem1.Description = menuItemEditedDescription1;

            bool result = managerDataAccess.EditMenuItem(managerContext1.Id, menuItem1);
            Assert.IsTrue(result);

            var menuItems = managerDataAccess.GetMenuItems(managerContext1.Id);
            Assert.IsTrue(menuItems != null && menuItems.Any());

            var editedMenuItem = menuItems.FirstOrDefault(m => m.Id == menuItem1.Id);
            Assert.IsNotNull(editedMenuItem);

            Assert.AreEqual(editedMenuItem.Description, menuItemEditedDescription1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void EditMenuItemCategoryTest()
        {
            if (category1 == null)
                AddNewCategoryTest();

            ManagerLogInTest();

            category1.Description = categoryEditedDesciption1;
            bool result = managerDataAccess.EditMenuItemCategory(managerContext1.Id, category1);
            Assert.IsTrue(result);

            var categories = managerDataAccess.GetMenuItemCategories(managerContext1.Id);
            Assert.IsTrue(categories != null || categories.Any());

            var editedCategory = categories.FirstOrDefault(c => c.Id == category1.Id);
            Assert.IsNotNull(editedCategory);

            Assert.AreEqual(editedCategory.Description, categoryEditedDesciption1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void EditTableTest()
        {
            if (table1 == null)
                AddNewTableTest();

            ManagerLogInTest();

            table1.Description = tableEditedDescription1;
            bool result = managerDataAccess.EditTable(managerContext1.Id, table1);
            Assert.IsTrue(result);

            var tables = managerDataAccess.GetTables(managerContext1.Id);
            Assert.IsTrue(tables != null && tables.Any());

            var editedTable = tables.FirstOrDefault(t => t.Id == table1.Id);
            Assert.IsNotNull(editedTable);

            Assert.AreEqual(editedTable.Description, tableEditedDescription1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void EditWaiterTest()
        {
            if (waiterContext1 == null)
                AddNewWaiterTest();

            ManagerLogInTest();

            waiterContext1.LastName = waiterEditedLastName1;

            bool result = managerDataAccess.EditWaiter(managerContext1.Id, waiterContext1);
            Assert.IsTrue(result);

            var waiters = managerDataAccess.GetWaiters(managerContext1.Id);
            Assert.IsTrue(waiters != null || waiters.Any());

            var editedWaiter = waiters.FirstOrDefault(w => w.Id == waiterContext1.Id);
            Assert.IsNotNull(editedWaiter);

            Assert.AreEqual(editedWaiter.LastName, waiterEditedLastName1);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void RemoveMenuItemCategoryTest()
        {
            if (category1 == null)
                AddNewCategoryTest();

            ManagerLogInTest();

            bool result = managerDataAccess.RemoveMenuItemCategory(managerContext1.Id, category1.Id);
            Assert.IsTrue(result);

            var categories = managerDataAccess.GetMenuItemCategories(managerContext1.Id);

            if (categories != null && categories.Any())
            {
                var removedCategory = categories.FirstOrDefault(c => c.Id == category1.Id);
                Assert.IsNull(removedCategory);
            }

            result = managerDataAccess.EditMenuItemCategory(managerContext1.Id, category1);
            Assert.IsFalse(result);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void RemoveMenuItemTest()
        {
            if (menuItem1 == null)
                AddNewMenuItemTest();

            ManagerLogInTest();

            bool result = managerDataAccess.RemoveMenuItem(managerContext1.Id, menuItem1.Id);
            Assert.IsTrue(result);

            var menuItems = managerDataAccess.GetMenuItems(managerContext1.Id);

            if (menuItem1 != null && menuItems.Any())
            {
                var removedMenuItem = menuItems.FirstOrDefault(m => m.Id == menuItem1.Id);
                Assert.IsNull(removedMenuItem);
            }

            result = managerDataAccess.EditMenuItem(managerContext1.Id, menuItem1);
            Assert.IsFalse(result);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void RemoveTableTest()
        {
            if (table1 == null)
                AddNewTableTest();

            ManagerLogInTest();

            bool result = managerDataAccess.RemoveTable(managerContext1.Id, table1.Id);
            Assert.IsTrue(result);

            var tables = managerDataAccess.GetTables(managerContext1.Id);

            if (tables != null && tables.Any())
            {
                var removedTable = tables.FirstOrDefault(t => t.Id == table1.Id);
                Assert.IsNull(removedTable);
            }

            result = managerDataAccess.EditTable(managerContext1.Id, table1);
            Assert.IsFalse(result);

            ManagerLogOutTest();
        }

        [TestMethod]
        public void RemoveWaiterTest()
        {
            if (waiterContext1 == null)
                AddNewWaiterTest();

            ManagerLogInTest();

            bool result = managerDataAccess.RemoveWaiter(managerContext1.Id, waiterContext1.Id);
            Assert.IsTrue(result);

            var waiters = managerDataAccess.GetWaiters(managerContext1.Id);

            if (waiters != null && waiters.Any())
            {
                var removedWaiter = waiters.FirstOrDefault(w => w.Id == waiterContext1.Id);
                Assert.IsNull(removedWaiter);
            }

            result = managerDataAccess.EditWaiter(managerContext1.Id, waiterContext1);
            Assert.IsFalse(result);
        }

        #endregion

        #region Waiter Method Tests

        [TestMethod]
        public void WaiterLogInTest()
        {
            if (waiterContext1 == null)
                AddNewWaiterTest();

            UserContext context = waiterDataAccess.LogIn(waiterLogin1, waiterPassword1);
            Assert.IsNotNull(context);
            Assert.AreEqual(context.Login, waiterLogin1);
        }

        [TestMethod]
        public void WaiterLogOutTest()
        {
            WaiterLogInTest();

            bool result = waiterDataAccess.LogOut(waiterContext1.Id);
            Assert.IsTrue(result);
        }

        #endregion

        #region Client Method Tests

        [TestMethod]
        public void AddNewClientTest()
        {
            clientContext1 = clientDataAccess.AddClient(ClientFirstName1, ClientLastName1, ClientLogin1,
                HashClass.CreateFirstHash(ClientPassword1, ClientLogin1));

            Assert.IsNotNull(clientContext1);
            Assert.AreNotEqual(clientContext1.Id, 0);
            Assert.AreEqual(clientContext1.FirstName, ClientFirstName1);
            Assert.AreEqual(clientContext1.LastName, ClientLastName1);
            Assert.AreEqual(clientContext1.Role, UserRole.Client);
        }

        [TestMethod]
        public void ClientLogInTest()
        {
            if(clientContext1 == null)
                AddNewClientTest();

            var context = clientDataAccess.LogIn(ClientLogin1, HashClass.CreateFirstHash(ClientPassword1, ClientLogin1));
            Assert.IsNotNull(context);
            Assert.AreEqual(context.Login, ClientLogin1);
            Assert.AreEqual(context.Role, UserRole.Client);

        }

        [TestMethod]
        public void ClientLogOutTest()
        {
            if (clientContext1 != null)
            {
                bool result = clientDataAccess.LogOut(clientContext1.Id);
                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ClientAddOrderTest()
        {
            ClientLogInTest();

            if (category1 == null)
                AddNewCategoryTest();
            if (table1 == null)
                AddNewTableTest();
            if (menuItem1 == null)
                AddNewMenuItemTest();

            var menuItems = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(menuItem1.Id, menuItemQuantity1),
                new Tuple<int, int>(menuItem2.Id, menuItemQuantity2),
                new Tuple<int, int>(menuItem3.Id, menuItemQuantity3)
            };

            order1 = clientDataAccess.AddOrder(clientContext1.Id, table1.Id, menuItems);
            Assert.IsNotNull(order1);
            Assert.AreNotEqual(order1.Id, 0);
            Assert.AreEqual(order1.UserId, clientContext1.Id);
            //Assert.IsNotNull(order1.Waiter);
            //Assert.AreEqual(order1.Waiter.Id, waiterContext1.Id);
            Assert.AreEqual(order1.State, OrderState.Placed);
            Assert.IsNotNull(order1.Table);
            Assert.AreEqual(order1.Table.Id, table1.Id);
            Assert.IsNotNull(order1.MenuItems);
            Assert.AreEqual(order1.MenuItems.Count, 3);
            Assert.AreEqual(order1.MenuItems.ElementAt(0).MenuItem.Id, menuItem1.Id);
            Assert.AreEqual(order1.MenuItems.ElementAt(0).Quantity, menuItemQuantity1);
            Assert.AreEqual(order1.MenuItems.ElementAt(1).MenuItem.Id, menuItem2.Id);
            Assert.AreEqual(order1.MenuItems.ElementAt(1).Quantity, menuItemQuantity2);
            Assert.AreEqual(order1.MenuItems.ElementAt(2).MenuItem.Id, menuItem3.Id);
            Assert.AreEqual(order1.MenuItems.ElementAt(2).Quantity, menuItemQuantity3);
            Assert.IsTrue(order1.PlacingDate < DateTime.Now);
            Assert.AreEqual(order1.ClosingDate, DateTime.MaxValue);

            ClientLogOutTest();
        }

        #endregion

        [TestMethod]
        public void RemoveOrderTest()
        {
           if (order1 == null)
                ClientAddOrderTest();

            ManagerLogInTest();

            bool result = managerDataAccess.RemoveOrder(managerContext1.Id, order1.Id);
            Assert.IsTrue(result);

            var orders = managerDataAccess.GetOrders(managerContext1.Id);
            if (orders != null && orders.Any())
            {
                var removedOrder = orders.FirstOrDefault(o => o.Id == order1.Id);
                Assert.IsNull(removedOrder);
            }

            ManagerLogOutTest();
        }

       

        //[TestMethod]
        //public void WaiterSetOrderStateTest()
        //{
        //    if (order1 == null)
        //        WaiterAddOrderTest();

        //    bool result = waiterDataAccess.SetOrderState(waiterContext1.Id, order1.Id, OrderState.Accepted);
        //    Assert.IsFalse(result);

        //    result = waiterDataAccess.SetOrderState(waiterContext1.Id, order1.Id, OrderState.Realized);
        //    Assert.IsTrue(result);

        //    var orders = managerDataAccess.GetOrders();
        //    Assert.IsTrue(orders != null && orders.Any());

        //    var order = orders.FirstOrDefault(o => o.Id == order1.Id);
        //    Assert.IsNotNull(order);
        //    Assert.AreEqual(order.State, OrderState.Realized);

        //    result = waiterDataAccess.SetOrderState(waiterContext1.Id, order1.Id, OrderState.NotRealized);
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void WaiterGetActiveOrdersTest()
        //{
        //    if (order1 == null)
        //        WaiterAddOrderTest();

        //    var activeOrders = waiterDataAccess.GetActiveOrders(waiterContext1.Id);
        //    Assert.IsTrue(activeOrders != null && activeOrders.Any());

        //    WaiterSetOrderStateTest();

        //    activeOrders = waiterDataAccess.GetActiveOrders(waiterContext1.Id);
        //    Assert.IsTrue(activeOrders == null || !activeOrders.Any());
        //}

        //[TestMethod]
        //public void WaiterGetPastOrdersTest()
        //{
        //    WaiterSetOrderStateTest();

        //    var pastOrders = waiterDataAccess.GetPastOrders(waiterContext1.Id);
        //    Assert.IsTrue(pastOrders != null && pastOrders.Any());

        //    int orderCount = pastOrders.ToList().Count;

        //    pastOrders = waiterDataAccess.GetPastOrders(waiterContext1.Id, orderCount - 1, orderCount - 1);
        //    Assert.IsTrue(pastOrders != null && pastOrders.ToList().Count == 1);

        //    pastOrders = waiterDataAccess.GetPastOrders(waiterContext1.Id, 0, orderCount + 10);
        //    Assert.IsTrue(pastOrders != null && pastOrders.ToList().Count == orderCount);

        //    pastOrders = waiterDataAccess.GetPastOrders(waiterContext1.Id, orderCount, orderCount + 10);
        //    Assert.IsTrue(pastOrders == null || !pastOrders.Any());
        //}


    }
}
