using System;
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
            public MoqMenuItemCategory(int id, string name, string description) : base(id, name, description) { }
        }

        [TestMethod]
        public void GetAllCategories_ValidExample()
        {
            //Arrange List of Categories
            var Categories = new List<MenuItemCategory>();

            var e1 = new MoqMenuItemCategory( 1, "Żarcie", "Schabowe i inne" );
            var e2 = new MoqMenuItemCategory( 2, "Napoje", "Wódka i inne" );
            var e3 = new MoqMenuItemCategory( 3, "Przystawki", "Mało tego" );

            Categories.Add(e1);
            Categories.Add(e2);
            Categories.Add(e3);

            var mock = new Mock<IManagerDataAccess>();
            mock.Setup(m => m.GetMenuItemCategories()).Returns(Categories);

            //Arrange Bar Data Model
            var BarDataModel = new BarDataModel(mock.Object);
            
            //Act
            var ReturnedCategories = BarDataModel.GetAllCategories();

            //Asserts
            Assert.IsNotNull(ReturnedCategories);

            Assert.AreEqual(ReturnedCategories[0].Id, e1.Id);
            Assert.AreEqual(ReturnedCategories[0].Name, e1.Name);
            Assert.AreEqual(ReturnedCategories[0].Description, e1.Description);

            Assert.AreEqual(ReturnedCategories[1].Id, e2.Id);
            Assert.AreEqual(ReturnedCategories[1].Name, e2.Name);
            Assert.AreEqual(ReturnedCategories[1].Description, e2.Description);

            Assert.AreEqual(ReturnedCategories[2].Id, e3.Id);
            Assert.AreEqual(ReturnedCategories[2].Name, e3.Name);
            Assert.AreEqual(ReturnedCategories[2].Description, e3.Description);
        }

        [TestMethod]
        public void AddCategoryItem_ValidExample()
        {
            //Arrange Adding Menu item Category
            var category = new MoqMenuItemCategory(1,"Żarcie", "Różne");

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
            public MoqMenuItem(int id, string name, string description, MenuItemCategory category, Money price) : base(id, name, description, category, price) { }
        }

        [TestMethod]
        public void GetAllMenuItems_ValidExample()
        {
            //Arrange List of Categories
            var MenuItems = new List<MenuItem>();

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
            var ReturnedCategories = BarDataModel.GetAllMenuItems();

            //Asserts
            Assert.IsNotNull(ReturnedCategories);

            Assert.AreEqual(ReturnedCategories[0].Id, f1.Id);
            Assert.AreEqual(ReturnedCategories[0].Name, f1.Name);
            Assert.AreEqual(ReturnedCategories[0].Description, f1.Description);
            Assert.AreEqual(ReturnedCategories[0].Category, e1);
            Assert.AreEqual(ReturnedCategories[0].Price.Amount, 20);
            Assert.AreEqual(ReturnedCategories[0].Price.Currency, "PLN");

            Assert.AreEqual(ReturnedCategories[1].Id, f2.Id);
            Assert.AreEqual(ReturnedCategories[1].Name, f2.Name);
            Assert.AreEqual(ReturnedCategories[1].Description, f2.Description);
            Assert.AreEqual(ReturnedCategories[1].Category, e2);
            Assert.AreEqual(ReturnedCategories[1].Price.Amount, 30);
            Assert.AreEqual(ReturnedCategories[1].Price.Currency, "PLN");

            Assert.AreEqual(ReturnedCategories[2].Id, f3.Id);
            Assert.AreEqual(ReturnedCategories[2].Name, f3.Name);
            Assert.AreEqual(ReturnedCategories[2].Description, f3.Description);
            Assert.AreEqual(ReturnedCategories[2].Category, e3);
            Assert.AreEqual(ReturnedCategories[2].Price.Amount, 5);
            Assert.AreEqual(ReturnedCategories[2].Price.Currency, "PLN");
        }

        #endregion

    }
}
