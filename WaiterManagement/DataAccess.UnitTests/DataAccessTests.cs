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
        IManagerDataAccess managerDataAccess = null;
        IWaiterDataAccess waiterDataAccess = null;
        IDataWipe dataWipe = null;

        string managerFirstName1 = "Mana";
        string managerLastName1 = "Dżer";
        string managerLogin1 = "admin";
        string managerPassword1 = "admin";
        UserContext managerContext1 = null;


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

        /// <summary>
        /// Na pierwszym etapie nie rozróżniamy użytkowników, dlatego wszystie zamówienia są na mockowego użytkownika o Id = 500
        /// </summary>
        int userId1 = 500;

        [TestInitialize]
        public void TestInitialize()
        {
            //Upewnienie, że baza posiada najnowszy model danych
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<DataAccessProvider, Configuration>());

            managerDataAccess = new DataAccessClass();
            waiterDataAccess = new DataAccessClass();
            dataWipe = new DataAccessClass();
        }

        [TestMethod]
        public void AddNewManagerTest()
        {
            managerContext1 = managerDataAccess.AddManager(managerFirstName1, managerLastName1, managerLogin1, HashClass.CreateFirstHash(managerPassword1, managerLogin1));
            Assert.IsNotNull(managerContext1);
            Assert.AreNotEqual(managerContext1.Id, 0);
        }

        //[TestMethod]
        //public void AddNewWaiterTest()
        //{
        //    waiterContext1 = managerDataAccess.AddWaiter(waiterFirstName1, waiterLastName1, waiterLogin1, waiterPassword1);

        //    Assert.IsNotNull(waiterContext1);
        //    Assert.AreNotEqual(waiterContext1.Id, 0);
        //    Assert.AreEqual(waiterContext1.FirstName, waiterFirstName1);
        //    Assert.AreEqual(waiterContext1.LastName, waiterLastName1);
        //    Assert.AreEqual(waiterContext1.Login, waiterLogin1);
        //    Assert.AreEqual(waiterContext1.Password, waiterPassword1);
        //}

        //[TestMethod]
        //public void AddRepeatedWaiterTest()
        //{
        //    if (waiterContext1 == null)
        //        AddNewWaiterTest();

        //    try
        //    {
        //        //Próba dodania drugiego kelnera o już istniejącym loginem
        //        waiterContext2 = managerDataAccess.AddWaiter(waiterFirstName2, waiterLastName2, waiterLogin1, waiterPassword2);
        //        Assert.Fail("waiter2Context should not be created!");
        //    }
        //    catch(Exception e)
        //    {
        //        Assert.IsTrue(e is ArgumentException);
        //    }
        //}

        //[TestMethod]
        //public void AddNewCategoryTest()
        //{
        //    category1 = managerDataAccess.AddMenuItemCategory(categoryName1, categoryDescription1);

        //    Assert.IsNotNull(category1);
        //    Assert.AreNotEqual(category1.Id, 0);
        //    Assert.AreEqual(category1.Name, categoryName1);
        //    Assert.AreEqual(category1.Description, categoryDescription1);
        //}

        //[TestMethod]
        //public void AddNewMenuItemTest()
        //{
        //    if (category1 == null)
        //        AddNewCategoryTest();

        //    menuItem1 = managerDataAccess.AddMenuItem(menuItemName1, menuItemDescription1, category1.Id, menuItemPrice1);
        //    menuItem2 = managerDataAccess.AddMenuItem(menuItemName2, menuItemDescription2, category1.Id, menuItemPrice2);
        //    menuItem3 = managerDataAccess.AddMenuItem(menuItemName3, menuItemDescription3, category1.Id, menuItemPrice3);

        //    Assert.IsNotNull(menuItem1);
        //    Assert.AreNotEqual(menuItem1.Id, 0);
        //    Assert.AreEqual(menuItem1.Name, menuItemName1);
        //    Assert.AreEqual(menuItem1.Description, menuItemDescription1);
        //    Assert.IsNotNull(menuItem1.Category);
        //    Assert.AreEqual(menuItem1.Category.Name, categoryName1);
        //    Assert.AreEqual(menuItem1.Category.Description, categoryDescription1);

        //    Assert.IsNotNull(menuItem2);
        //    Assert.AreNotEqual(menuItem2.Id, 0);
        //    Assert.AreEqual(menuItem2.Name, menuItemName2);
        //    Assert.AreEqual(menuItem2.Description, menuItemDescription2);
        //    Assert.IsNotNull(menuItem2.Category);
        //    Assert.AreEqual(menuItem2.Category.Name, categoryName1);
        //    Assert.AreEqual(menuItem2.Category.Description, categoryDescription1);

        //    Assert.IsNotNull(menuItem3);
        //    Assert.AreNotEqual(menuItem3.Id, 0);
        //    Assert.AreEqual(menuItem3.Name, menuItemName3);
        //    Assert.AreEqual(menuItem3.Description, menuItemDescription3);
        //    Assert.IsNotNull(menuItem3.Category);
        //    Assert.AreEqual(menuItem3.Category.Name, categoryName1);
        //    Assert.AreEqual(menuItem3.Category.Description, categoryDescription1);
        //}

        //[TestMethod]
        //public void AddNewTableTest()
        //{
        //    table1 = managerDataAccess.AddTable(tableNumber1, tableDescription1);

        //    Assert.IsNotNull(table1);
        //    Assert.AreNotEqual(table1.Id, 0);
        //    Assert.AreEqual(table1.Number, tableNumber1);
        //    Assert.AreEqual(table1.Description, tableDescription1);
        //}

        //[TestMethod]
        //public void EditMenuItemTest()
        //{
        //    if (menuItem1 == null)
        //        AddNewMenuItemTest();

        //    menuItem1.Description = menuItemEditedDescription1;

        //    bool result = managerDataAccess.EditMenuItem(menuItem1);
        //    Assert.IsTrue(result);

        //    var menuItems = managerDataAccess.GetMenuItems();
        //    Assert.IsTrue(menuItems != null && menuItems.Any());

        //    var editedMenuItem = menuItems.FirstOrDefault(m => m.Id == menuItem1.Id);
        //    Assert.IsNotNull(editedMenuItem);

        //    Assert.AreEqual(editedMenuItem.Description, menuItemEditedDescription1);
        //}

        //[TestMethod]
        //public void EditMenuItemCategoryTest()
        //{
        //    if (category1 == null)
        //        AddNewCategoryTest();

        //    category1.Description = categoryEditedDesciption1;
        //    bool result = managerDataAccess.EditMenuItemCategory(category1);
        //    Assert.IsTrue(result);

        //    var categories = managerDataAccess.GetMenuItemCategories();
        //    Assert.IsTrue(categories != null || categories.Any());

        //    var editedCategory = categories.FirstOrDefault(c => c.Id == category1.Id);
        //    Assert.IsNotNull(editedCategory);

        //    Assert.AreEqual(editedCategory.Description, categoryEditedDesciption1);
        //}

        //[TestMethod]
        //public void EditTableTest()
        //{
        //    if (table1 == null)
        //        AddNewTableTest();

        //    table1.Description = tableEditedDescription1;
        //    bool result = managerDataAccess.EditTable(table1);
        //    Assert.IsTrue(result);

        //    var tables = managerDataAccess.GetTables();
        //    Assert.IsTrue(tables != null && tables.Any());

        //    var editedTable = tables.FirstOrDefault(t => t.Id == table1.Id);
        //    Assert.IsNotNull(editedTable);

        //    Assert.AreEqual(editedTable.Description, tableEditedDescription1);
        //}

        ////[TestMethod]
        ////public void EditWaiterTest()
        ////{
        ////    if (waiterContext1 == null)
        ////        AddNewWaiterTest();

        ////    waiterContext1.LastName = waiterEditedLastName1;

        ////    bool result = managerDataAccess.EditWaiter(waiterContext1);
        ////    Assert.IsTrue(result);

        ////    var waiters = managerDataAccess.GetWaiters();
        ////    Assert.IsTrue(waiters != null || waiters.Any());

        ////    var editedWaiter = waiters.FirstOrDefault(w => w.Id == waiterContext1.Id);
        ////    Assert.IsNotNull(editedWaiter);

        ////    Assert.AreEqual(editedWaiter.LastName, waiterEditedLastName1);
        ////}

        //[TestMethod]
        //public void RemoveMenuItemCategoryTest()
        //{
        //    if (category1 == null)
        //        AddNewCategoryTest();

        //    bool result = managerDataAccess.RemoveMenuItemCategory(category1.Id);
        //    Assert.IsTrue(result);

        //    var categories = managerDataAccess.GetMenuItemCategories();

        //    if (categories != null && categories.Any())
        //    {
        //        var removedCategory = categories.FirstOrDefault(c => c.Id == category1.Id);
        //        Assert.IsNull(removedCategory);
        //    }

        //    result = managerDataAccess.EditMenuItemCategory(category1);
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void RemoveMenuItemTest()
        //{
        //    if (menuItem1 == null)
        //        AddNewMenuItemTest();

        //    bool result = managerDataAccess.RemoveMenuItem(menuItem1.Id);
        //    Assert.IsTrue(result);

        //    var menuItems = managerDataAccess.GetMenuItems();

        //    if (menuItem1 != null && menuItems.Any())
        //    {
        //        var removedMenuItem = menuItems.FirstOrDefault(m => m.Id == menuItem1.Id);
        //        Assert.IsNull(removedMenuItem);
        //    }

        //    result = managerDataAccess.EditMenuItem(menuItem1);
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void RemoveTableTest()
        //{
        //    if (table1 == null)
        //        AddNewTableTest();

        //    bool result = managerDataAccess.RemoveTable(table1.Id);
        //    Assert.IsTrue(result);

        //    var tables = managerDataAccess.GetTables();

        //    if(tables != null && tables.Any())
        //    {
        //        var removedTable = tables.FirstOrDefault(t => t.Id == table1.Id);
        //        Assert.IsNull(removedTable);
        //    }

        //    result = managerDataAccess.EditTable(table1);
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void RemoveWaiterTest()
        //{
        //    if (waiterContext1 == null)
        //        AddNewWaiterTest();

        //    bool result = managerDataAccess.RemoveWaiter(waiterContext1.Id);
        //    Assert.IsTrue(result);

        //    var waiters = managerDataAccess.GetWaiters();

        //    if(waiters != null && waiters.Any())
        //    {
        //        var removedWaiter = waiters.FirstOrDefault(w => w.Id == waiterContext1.Id);
        //        Assert.IsNull(removedWaiter);
        //    }

        //    result = managerDataAccess.EditWaiter(waiterContext1);
        //    Assert.IsFalse(result);
        //}

        //[TestMethod]
        //public void RemoveOrderTest()
        //{
        //    if (order1 == null)
        //        WaiterAddOrderTest();

        //    bool result = managerDataAccess.RemoveOrder(order1.Id);
        //    Assert.IsTrue(result);

        //    var orders = managerDataAccess.GetOrders();
        //    if(orders != null && orders.Any())
        //    {
        //        var removedOrder = orders.FirstOrDefault(o => o.Id == order1.Id);
        //        Assert.IsNull(removedOrder);
        //    }
        //}

        //[TestMethod]
        //public void WaiterLogInTest()
        //{
        //    if (waiterContext1 == null)
        //        AddNewWaiterTest();
          
        //    WaiterContext context = waiterDataAccess.LogIn(waiterLogin1, waiterPassword1);
        //    Assert.IsNotNull(context);
        //    Assert.AreEqual(context.Login, waiterLogin1);
        //    Assert.AreEqual(context.Password, waiterPassword1);
          
        //}

        //[TestMethod]
        //public void WaiterLogOutTest()
        //{
        //    WaiterLogInTest();

        //    bool result = waiterDataAccess.LogOut(waiterContext1.Id);
        //    Assert.IsTrue(result);
        //}

        //[TestMethod]
        //public void WaiterAddOrderTest()
        //{
        //    WaiterLogInTest();
        //    if (category1 == null)
        //        AddNewCategoryTest();
        //    if (table1 == null)
        //        AddNewTableTest();
        //    if (menuItem1 == null)
        //        AddNewMenuItemTest();           

        //    var menuItems = new List<Tuple<int, int>>();
        //    menuItems.Add(new Tuple<int, int>(menuItem1.Id, menuItemQuantity1));
        //    menuItems.Add(new Tuple<int, int>(menuItem2.Id, menuItemQuantity2));
        //    menuItems.Add(new Tuple<int, int>(menuItem3.Id, menuItemQuantity3));

        //    order1 = waiterDataAccess.AddOrder(userId1, table1.Id, waiterContext1.Id, menuItems);
        //    Assert.IsNotNull(order1);
        //    Assert.AreNotEqual(order1.Id, 0);
        //    Assert.AreEqual(order1.UserId, userId1);
        //    Assert.IsNotNull(order1.Waiter);
        //    Assert.AreEqual(order1.Waiter.Id, waiterContext1.Id);
        //    Assert.AreEqual(order1.State, OrderState.Accepted);
        //    Assert.IsNotNull(order1.Table);
        //    Assert.AreEqual(order1.Table.Id, table1.Id);
        //    Assert.IsNotNull(order1.MenuItems);
        //    Assert.AreEqual(order1.MenuItems.Count, 3);
        //    Assert.AreEqual(order1.MenuItems.ElementAt(0).MenuItem.Id, menuItem1.Id);
        //    Assert.AreEqual(order1.MenuItems.ElementAt(0).Quantity, menuItemQuantity1);
        //    Assert.AreEqual(order1.MenuItems.ElementAt(1).MenuItem.Id, menuItem2.Id);
        //    Assert.AreEqual(order1.MenuItems.ElementAt(1).Quantity, menuItemQuantity2);
        //    Assert.AreEqual(order1.MenuItems.ElementAt(2).MenuItem.Id, menuItem3.Id);
        //    Assert.AreEqual(order1.MenuItems.ElementAt(2).Quantity, menuItemQuantity3);
        //    Assert.IsTrue(order1.PlacingDate < DateTime.Now);
        //    Assert.AreEqual(order1.ClosingDate, DateTime.MaxValue);
        //}

        //[TestMethod]
        //public void WaiterSetOrderStateTest()
        //{
        //   if (order1 == null)
        //        WaiterAddOrderTest();

        //   bool result = waiterDataAccess.SetOrderState(waiterContext1.Id, order1.Id, OrderState.Accepted);
        //   Assert.IsFalse(result);

        //   result = waiterDataAccess.SetOrderState(waiterContext1.Id, order1.Id, OrderState.Realized);
        //   Assert.IsTrue(result);

        //   var orders = managerDataAccess.GetOrders();
        //   Assert.IsTrue(orders != null && orders.Any());

        //   var order = orders.FirstOrDefault(o => o.Id == order1.Id);
        //   Assert.IsNotNull(order);
        //   Assert.AreEqual(order.State, OrderState.Realized);

        //   result = waiterDataAccess.SetOrderState(waiterContext1.Id, order1.Id, OrderState.NotRealized);
        //   Assert.IsFalse(result);            
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

        [TestCleanup]
        public void TestCleanup()
        {
            if (order1 != null)
                dataWipe.WipeOrder(order1.Id);

            if (managerContext1 != null)
                dataWipe.WipeUser(managerContext1.Id);

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
            category1 = null;
            menuItem1 = null;
            menuItem2 = null;
            menuItem3 = null;
            table1 = null;
        }
    }
}
