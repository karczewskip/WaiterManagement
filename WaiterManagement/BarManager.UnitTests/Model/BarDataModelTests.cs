using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BarManager.Abstract;
using BarManager.Model;
using DataAccess;
using ClassLib.DbDataStructures;
using System.Collections.Generic;
using System.Collections;

namespace BarManager.UnitTests
{
    [TestClass]
    public class BarDataModelTests
    {
        #region Categories
        private class MoqMenuItemCategory : MenuItemCategory
        {
            public new int Id { get; set; }
            public MoqMenuItemCategory(int id, string name, string description)
            {
                Id = id;
                Name = name;
                Description = description;
            }
        }

        [TestMethod]
        public void GetAllCategories_ValidExample()
        {
            //Arrange List of Categories
            var Categories = new List<MoqMenuItemCategory>();

            var e1 = new MoqMenuItemCategory(1, "Żarcie", "Schabowe i inne");
            var e2 = new MoqMenuItemCategory(2, "Napoje", "Wódka i inne");
            var e3 = new MoqMenuItemCategory(3, "Przystawki", "Mało tego");

            Categories.Add(e1);
            Categories.Add(e2);
            Categories.Add(e3);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.GetMenuItemCategories()).Returns(Categories);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var ReturnedCategories = BarDataModel.GetAllCategories();
            var ReturnedCategoriesCast = ReturnedCategories.Cast<MoqMenuItemCategory>().ToList();

            //Asserts
            Assert.IsNotNull(ReturnedCategoriesCast);

            Assert.AreEqual(ReturnedCategoriesCast[0].Id, e1.Id);
            Assert.AreEqual(ReturnedCategoriesCast[0].Name, e1.Name);
            Assert.AreEqual(ReturnedCategoriesCast[0].Description, e1.Description);

            Assert.AreEqual(ReturnedCategoriesCast[1].Id, e2.Id);
            Assert.AreEqual(ReturnedCategoriesCast[1].Name, e2.Name);
            Assert.AreEqual(ReturnedCategoriesCast[1].Description, e2.Description);

            Assert.AreEqual(ReturnedCategoriesCast[2].Id, e3.Id);
            Assert.AreEqual(ReturnedCategoriesCast[2].Name, e3.Name);
            Assert.AreEqual(ReturnedCategoriesCast[2].Description, e3.Description);
        }

        [TestMethod]
        public void AddCategoryItem_ValidExample()
        {
            //Arrange Adding Menu item Category
            var category = new MoqMenuItemCategory(1, "Żarcie", "Różne");

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddMenuItemCategory(It.IsAny<string>(), It.IsAny<string>())).Returns(category);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddCategoryItem("Żarcie", "Różne");

            //Assert
            Assert.AreEqual(result, category);
        }

        [TestMethod]
        public void AddCategoryItem_FailedAddinToDataBase()
        {
            //Arrange ManagerDataAccess
            MenuItemCategory category = null;

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddMenuItemCategory(It.IsAny<string>(), It.IsAny<string>())).Returns(category);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddCategoryItem("Żarcie", "Różne");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddCategoryItem_ExceptionFromDataAccess()
        {
            //Arrange Exception
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddMenuItemCategory(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddCategoryItem("Żarcie", "Różne");

            //Assert
            Assert.IsNull(result);
        }

        #endregion

        #region MenuItems

        private class MoqMenuItem : MenuItem
        {
            public new int Id { get; set; }
            public MoqMenuItem(int id, string name, string description, MenuItemCategory category, Money price)
            {
                Id = id;
                Name = name;
                Description = description;
                Category = category;
                Price = price;
            }


        }

        [TestMethod]
        public void GetAllMenuItems_ValidExample()
        {
            //Arrange List of Categories
            var MenuItems = new List<MoqMenuItem>();

            var e1 = new MoqMenuItemCategory(1, "Żarcie", "Schabowe i inne");
            var e2 = new MoqMenuItemCategory(2, "Napoje", "Wódka i inne");
            var e3 = new MoqMenuItemCategory(3, "Przystawki", "Mało tego");

            var f1 = new MoqMenuItem(1, "Schabowy", "Bardzo dobry Schabowy", e1, new Money() { Amount = 20, Currency = "PLN" });
            var f2 = new MoqMenuItem(2, "Wódka", "Bardzo wykwintna wódka", e2, new Money() { Amount = 30, Currency = "PLN" });
            var f3 = new MoqMenuItem(3, "Frytki", "3 frytki na krzyż", e3, new Money() { Amount = 5, Currency = "PLN" });

            MenuItems.Add(f1);
            MenuItems.Add(f2);
            MenuItems.Add(f3);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.GetMenuItems()).Returns(MenuItems);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var ReturnedMenuItems = BarDataModel.GetAllMenuItems();
            var ReturnedMenuItemsCast = ReturnedMenuItems.Cast<MoqMenuItem>().ToList();

            //Asserts
            Assert.IsNotNull(ReturnedMenuItemsCast);

            Assert.AreEqual(ReturnedMenuItemsCast[0].Id, f1.Id);
            Assert.AreEqual(ReturnedMenuItemsCast[0].Name, f1.Name);
            Assert.AreEqual(ReturnedMenuItemsCast[0].Description, f1.Description);
            Assert.AreEqual(ReturnedMenuItemsCast[0].Category, e1);
            Assert.AreEqual(ReturnedMenuItemsCast[0].Price.Amount, 20);
            Assert.AreEqual(ReturnedMenuItemsCast[0].Price.Currency, "PLN");

            Assert.AreEqual(ReturnedMenuItemsCast[1].Id, f2.Id);
            Assert.AreEqual(ReturnedMenuItemsCast[1].Name, f2.Name);
            Assert.AreEqual(ReturnedMenuItemsCast[1].Description, f2.Description);
            Assert.AreEqual(ReturnedMenuItemsCast[1].Category, e2);
            Assert.AreEqual(ReturnedMenuItemsCast[1].Price.Amount, 30);
            Assert.AreEqual(ReturnedMenuItemsCast[1].Price.Currency, "PLN");

            Assert.AreEqual(ReturnedMenuItemsCast[2].Id, f3.Id);
            Assert.AreEqual(ReturnedMenuItemsCast[2].Name, f3.Name);
            Assert.AreEqual(ReturnedMenuItemsCast[2].Description, f3.Description);
            Assert.AreEqual(ReturnedMenuItemsCast[2].Category, e3);
            Assert.AreEqual(ReturnedMenuItemsCast[2].Price.Amount, 5);
            Assert.AreEqual(ReturnedMenuItemsCast[2].Price.Currency, "PLN");
        }

        [TestMethod]
        public void AddMenuItem_ValidExample()
        {
            //Arrange Adding Menu item Category
            var category = new MoqMenuItemCategory(1, "Żarcie", "Różne");

            //Arrange Menu Item
            var menuItem = new MoqMenuItem(1, "Schabowy", "Bardzo dobry schabowy", category, new Money() { Amount = 20, Currency = "PLN" });

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddMenuItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Money>())).Returns(menuItem);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddMenuItem("Schabowy", category, 20, "Bardzo dobry schabowy");

            //Assert
            Assert.AreEqual(result, menuItem);
        }

        [TestMethod]
        public void AddMenuItem_FailedAddinToDataBase()
        {
            //Arrange Adding Menu item Category
            var category = new MoqMenuItemCategory(1, "Żarcie", "Różne");

            //Arrange ManagerDataAccess
            MenuItem menuItem = null;

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddMenuItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Money>())).Returns(menuItem);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddMenuItem("Schabowy", category, 20, "Bardzo dobry schabowy");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddMenuItem_ExceptionFromDataAccess()
        {
            //Arrange Adding Menu item Category
            var category = new MoqMenuItemCategory(1, "Żarcie", "Różne");

            //Arrange Exception
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddMenuItem(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<Money>())).Throws(new Exception());

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddMenuItem("Schabowy", category, 20, "Bardzo dobry schabowy");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteItem_ValidExample()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveMenuItem(It.IsAny<int>())).Returns(true);

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteItem(1);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void DeleteItem_FailedDeletingFromDataAccess()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveMenuItem(It.IsAny<int>())).Returns(false);

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteItem(1);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteItem_ExceptionFromDataAccess()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveMenuItem(It.IsAny<int>())).Throws(new Exception());

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteItem(1);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EditMenuItem_ValidExample()
        {
            //Arrange Menu item Categories
            var category1 = new MoqMenuItemCategory(1, "Żarcie", "Różne");
            var category2 = new MoqMenuItemCategory(2, "Picie", "Owocowe i nie");

            //Arrange other changes
            var newName = "Wódka";
            var newDescription = "Smakowa";
            var newPrice = (double)30;

            //Arrange Menu Item
            var menuItem = new MoqMenuItem(1, "Schabowy", "Bardzo dobry schabowy", category1, new Money() { Amount = 20, Currency = "PLN" });

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditMenuItem(It.IsAny<MenuItem>())).Returns(true);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditMenuItem(menuItem, newName, newPrice, category2, newDescription);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(menuItem.Name, newName);
            Assert.AreEqual(menuItem.Price.Amount, newPrice);
            Assert.AreEqual(menuItem.Category, category2);
            Assert.AreEqual(menuItem.Description, newDescription);

        }

        [TestMethod]
        public void EditMenuItem_FailedExample()
        {
            //Arrange Menu item Categories
            var category1 = new MoqMenuItemCategory(1, "Żarcie", "Różne");
            var category2 = new MoqMenuItemCategory(2, "Picie", "Owocowe i nie");

            //Arrange other changes
            var oldName = "Schabowy";
            var oldDescription = "Bardzo dobry schabowy";
            var oldPrice = 20;

            var newName = "Wódka";
            var newDescription = "Smakowa";
            var newPrice = (double)30;

            //Arrange Menu Item
            var menuItem = new MoqMenuItem(1, oldName, oldDescription, category1, new Money() { Amount = oldPrice, Currency = "PLN" });

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditMenuItem(It.IsAny<MenuItem>())).Returns(false);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditMenuItem(menuItem, newName, newPrice, category2, newDescription);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(menuItem.Name, oldName);
            Assert.AreEqual(menuItem.Price.Amount, oldPrice);
            Assert.AreEqual(menuItem.Category, category1);
            Assert.AreEqual(menuItem.Description, oldDescription);

        }

        [TestMethod]
        public void EditMenuItem_ExceptionFromDataAccess()
        {
            //Arrange Menu item Categories
            var category1 = new MoqMenuItemCategory(1, "Żarcie", "Różne");
            var category2 = new MoqMenuItemCategory(2, "Picie", "Owocowe i nie");

            //Arrange other changes
            var oldName = "Schabowy";
            var oldDescription = "Bardzo dobry schabowy";
            var oldPrice = 20;

            var newName = "Wódka";
            var newDescription = "Smakowa";
            var newPrice = (double)30;

            //Arrange Menu Item
            var menuItem = new MoqMenuItem(1, oldName, oldDescription, category1, new Money() { Amount = oldPrice, Currency = "PLN" });

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditMenuItem(It.IsAny<MenuItem>())).Throws(new Exception());

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditMenuItem(menuItem, newName, newPrice, category2, newDescription);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(menuItem.Name, oldName);
            Assert.AreEqual(menuItem.Price.Amount, oldPrice);
            Assert.AreEqual(menuItem.Category, category1);
            Assert.AreEqual(menuItem.Description, oldDescription);

        }


        #endregion

        #region Tables

        private class MoqTable : Table
        {
            public new int Id { get; set; }
            public MoqTable(int id, int number, string description)
            {
                Id = id;
                Number = number;
                Description = description;
            }
        }

        [TestMethod]
        public void GetAllTables_ValidExample()
        {
            //Arrange List of Categories
            var Tables = new List<MoqTable>();

            var f1 = new MoqTable(1, 1, "Po lewej");
            var f2 = new MoqTable(2, 2, "Po środku");
            var f3 = new MoqTable(3, 3, "Po prawej");

            Tables.Add(f1);
            Tables.Add(f2);
            Tables.Add(f3);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.GetTables()).Returns(Tables);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var ReturnedTables = BarDataModel.GetAllTables();
            var ReturnedTablesCast = ReturnedTables.Cast<MoqTable>().ToList();

            //Asserts
            Assert.IsNotNull(ReturnedTables);

            Assert.AreEqual(ReturnedTablesCast[0].Id, f1.Id);
            Assert.AreEqual(ReturnedTablesCast[0].Number, f1.Number);
            Assert.AreEqual(ReturnedTablesCast[0].Description, f1.Description);

            Assert.AreEqual(ReturnedTablesCast[1].Id, f2.Id);
            Assert.AreEqual(ReturnedTablesCast[1].Number, f2.Number);
            Assert.AreEqual(ReturnedTablesCast[1].Description, f2.Description);

            Assert.AreEqual(ReturnedTablesCast[2].Id, f3.Id);
            Assert.AreEqual(ReturnedTablesCast[2].Number, f3.Number);
            Assert.AreEqual(ReturnedTablesCast[2].Description, f3.Description);
        }

        [TestMethod]
        public void AddTable_ValidExample()
        {
            //Arrange Table
            var table = new MoqTable(1, 1, "Po lewej");

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddTable(It.IsAny<int>(), It.IsAny<string>())).Returns(table);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddTable(table.Number, table.Description);

            //Assert
            Assert.AreEqual(result, table);
        }

        [TestMethod]
        public void AddTable_FailedAddinToDataBase()
        {
            //Arrange Table
            Table table = null;

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddTable(It.IsAny<int>(), It.IsAny<string>())).Returns(table);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddTable(1, "Po lewej");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddTable_ExceptionFromDataAccess()
        {

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddTable(It.IsAny<int>(), It.IsAny<string>())).Throws(new Exception());

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddTable(1, "Po lewej");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteTable_ValidExample()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveTable(It.IsAny<int>())).Returns(true);

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteTable(1);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void DeleteTable_FailedDeletingFromDataAccess()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveTable(It.IsAny<int>())).Returns(false);

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteTable(1);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteTable_ExceptionFromDataAccess()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveTable(It.IsAny<int>())).Throws(new Exception());

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteTable(1);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EditTable_ValidExample()
        {
            //Arrange Changes
            var newNumber = 2;
            var newDescription = "Po prawej";

            //Arrange Table
            var table = new MoqTable(1, 1, "Po lewej");

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditTable(It.IsAny<Table>())).Returns(true);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditTable(table, newNumber, newDescription);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(table.Number, newNumber);
            Assert.AreEqual(table.Description, newDescription);

        }

        [TestMethod]
        public void EditTable_FailedExample()
        {
            //Arrange Changes
            var newNumber = 2;
            var newDescription = "Po prawej";

            //Arrange Table
            var oldNumber = 1;
            var oldDescription = "Po lewej";
            var table = new MoqTable(1, oldNumber, oldDescription);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditTable(It.IsAny<Table>())).Returns(false);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditTable(table, newNumber, newDescription);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(table.Number, oldNumber);
            Assert.AreEqual(table.Description, oldDescription);

        }

        [TestMethod]
        public void EditTable_ExceptionFromDataAccess()
        {
            //Arrange Changes
            var newNumber = 2;
            var newDescription = "Po prawej";

            //Arrange Table
            var oldNumber = 1;
            var oldDescription = "Po lewej";
            var table = new MoqTable(1, oldNumber, oldDescription);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditTable(It.IsAny<Table>())).Throws(new Exception());

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditTable(table, newNumber, newDescription);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(table.Number, oldNumber);
            Assert.AreEqual(table.Description, oldDescription);

        }

        #endregion

        #region Waiters
        private class MoqWaiter : WaiterContext
        {
            public new int Id { get; set; }
            public MoqWaiter(int id, string firstName, string lastName, string login, string password)
            {
                Id = id;
                FirstName = firstName;
                LastName = lastName;
                Login = login;
                Password = password;
            }
        }

        [TestMethod]
        public void GetAllWaiters_ValidExample()
        {
            //Arrange List of Waiters
            var Waiters = new List<MoqWaiter>();

            var f1 = new MoqWaiter(1, "Tom", "Dickens", "tDick", "lala");
            var f2 = new MoqWaiter(2, "Wiliam", "Whatever", "Rockman", "lolo");
            var f3 = new MoqWaiter(3, "Ken", "Hilary", "hili", "lipo");

            Waiters.Add(f1);
            Waiters.Add(f2);
            Waiters.Add(f3);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.GetWaiters()).Returns(Waiters);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var ReturnedWaiters = BarDataModel.GetAllWaiters();
            var ReturnedWaitersCast = ReturnedWaiters.Cast<MoqWaiter>().ToList();

            //Asserts
            Assert.IsNotNull(ReturnedWaiters);

            Assert.AreEqual(ReturnedWaitersCast[0].Id, f1.Id);
            Assert.AreEqual(ReturnedWaitersCast[0].FirstName, f1.FirstName);
            Assert.AreEqual(ReturnedWaitersCast[0].LastName, f1.LastName);
            Assert.AreEqual(ReturnedWaitersCast[0].Login, f1.Login);
            Assert.AreEqual(ReturnedWaitersCast[0].Password, f1.Password);

            Assert.AreEqual(ReturnedWaitersCast[1].Id, f2.Id);
            Assert.AreEqual(ReturnedWaitersCast[1].FirstName, f2.FirstName);
            Assert.AreEqual(ReturnedWaitersCast[1].LastName, f2.LastName);
            Assert.AreEqual(ReturnedWaitersCast[1].Login, f2.Login);
            Assert.AreEqual(ReturnedWaitersCast[1].Password, f2.Password);

            Assert.AreEqual(ReturnedWaitersCast[2].Id, f3.Id);
            Assert.AreEqual(ReturnedWaitersCast[2].FirstName, f3.FirstName);
            Assert.AreEqual(ReturnedWaitersCast[2].LastName, f3.LastName);
            Assert.AreEqual(ReturnedWaitersCast[2].Login, f3.Login);
            Assert.AreEqual(ReturnedWaitersCast[2].Password, f3.Password);
        }

        [TestMethod]
        public void AddWaiter_ValidExample()
        {
            //Arrange Waiter
            var waiter = new MoqWaiter(1, "Tom", "Dickens", "tDick", "lala");

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddWaiter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(waiter);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddWaiter(waiter.FirstName, waiter.LastName, waiter.Login, waiter.Password);

            //Assert
            Assert.AreEqual(result, waiter);
        }

        [TestMethod]
        public void AddWaiter_FailedAddinToDataBase()
        {
            //Arrange Waiter
            WaiterContext waiter = null;

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddWaiter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(waiter);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddWaiter( "Tom", "Dickens", "tDick", "lala");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void AddWaiter_ExceptionFromDataAccess()
        {

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.AddWaiter(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.AddWaiter("Tom", "Dickens", "tDick", "lala");

            //Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void DeleteWaiter_ValidExample()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveWaiter(It.IsAny<int>())).Returns(true);

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteWaiter(1);

            //Assert
            Assert.IsTrue(result);
        }


        [TestMethod]
        public void DeleteWaiter_FailedDeletingFromDataAccess()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveWaiter(It.IsAny<int>())).Returns(false);

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteWaiter(1);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void DeleteWaiter_ExceptionFromDataAccess()
        {
            //Arange 
            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.RemoveWaiter(It.IsAny<int>())).Throws(new Exception());

            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.DeleteWaiter(1);

            //Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EditWaiter_ValidExample()
        {
            //Arrange Changes
            var newFirstName = "Tony";
            var newLastName = "Lombster";
            var newLogin = "Lomb";
            var newPassword = "123";

            //Arrange waiter
            var waiter = new MoqWaiter(1, "Keny", "Wall" , "wally" , "qwerty");

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditWaiter(It.IsAny<WaiterContext>())).Returns(true);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditWaiter(waiter,newLogin, newFirstName, newLastName, newPassword);

            //Assert
            Assert.IsTrue(result);
            Assert.AreEqual(waiter.FirstName, newFirstName);
            Assert.AreEqual(waiter.LastName, newLastName);
            Assert.AreEqual(waiter.Login, newLogin);
            Assert.AreEqual(waiter.Password, newPassword);

        }

        [TestMethod]
        public void EditWaiter_FailedExample()
        {
            //Arrange Changes
            var newFirstName = "Tony";
            var newLastName = "Lombster";
            var newLogin = "Lomb";
            var newPassword = "123";

            //Arrange waiter
            var oldFirstName = "Keny";
            var oldLastName = "Wall";
            var oldLogin = "wally";
            var oldPassword = "qwerty";
            var waiter = new MoqWaiter(1, oldFirstName, oldLastName, oldLogin, oldPassword);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditWaiter(It.IsAny<WaiterContext>())).Returns(false);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditWaiter(waiter, newLogin, newFirstName, newLastName, newPassword);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(waiter.FirstName, oldFirstName);
            Assert.AreEqual(waiter.LastName, oldLastName);
            Assert.AreEqual(waiter.Login, oldLogin);
            Assert.AreEqual(waiter.Password, oldPassword);

        }

        [TestMethod]
        public void EditWaiter_ExceptionFromDataAccess()
        {
            //Arrange Changes
            var newFirstName = "Tony";
            var newLastName = "Lombster";
            var newLogin = "Lomb";
            var newPassword = "123";

            //Arrange waiter
            var oldFirstName = "Keny";
            var oldLastName = "Wall";
            var oldLogin = "wally";
            var oldPassword = "qwerty";
            var waiter = new MoqWaiter(1, oldFirstName, oldLastName, oldLogin, oldPassword);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.EditWaiter(It.IsAny<WaiterContext>())).Throws(new Exception());

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);

            //Act
            var result = BarDataModel.EditWaiter(waiter, newLogin, newFirstName, newLastName, newPassword);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual(waiter.FirstName, oldFirstName);
            Assert.AreEqual(waiter.LastName, oldLastName);
            Assert.AreEqual(waiter.Login, oldLogin);
            Assert.AreEqual(waiter.Password, oldPassword);


        }

        #endregion

    }
}
