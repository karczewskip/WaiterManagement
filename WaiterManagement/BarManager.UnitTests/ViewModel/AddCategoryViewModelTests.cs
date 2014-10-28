using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using BarManager.Abstract;
using ClassLib.DbDataStructures;
using System.Collections.Generic;
using BarManager.ViewModel;

namespace BarManager.UnitTests
{
    [TestClass]
    public class AddCategoryViewModelTests
    {
        [TestMethod]
        public void AddCategory_ValidExample()
        {
            //Arrange BarDataModel 
            var AddingCategory = new MenuItemCategory();

            var mockBarDataModel = new Mock<IBarDataModel>();
            mockBarDataModel.Setup(m => m.AddCategoryItem(It.IsAny<string>(), It.IsAny<string>())).Returns(AddingCategory);

            //Arrange MenuManagerViewModel
            var ListOfCategories = new List<MenuItemCategory>();

            var mockMenuManagerViewModel = new Mock<IMenuManagerViewModel>();
            mockMenuManagerViewModel.Setup(m => m.ListOfCategories).Returns(ListOfCategories);

            //Arrange AddCategoryViewModel
            var AddCategoryViewModel = new AddCategoryViewModel(mockBarDataModel.Object, mockMenuManagerViewModel.Object);
            AddCategoryViewModel.CategoryName = "Nowa";
            AddCategoryViewModel.CategoryDescription = "Cudowna";

            //Act
            string resultError;
            var result = AddCategoryViewModel.AddCategory(out resultError);

            //Assert
            Assert.IsTrue(result);
            Assert.IsNotNull(resultError);
            Assert.AreEqual("", resultError);
            Assert.AreEqual(AddingCategory, ListOfCategories[0]);


        }

        [TestMethod]
        public void AddCategory_NoCategoryName()
        {
            //Arrange BarDataModel 
            var AddingCategory = new MenuItemCategory();

            var mockBarDataModel = new Mock<IBarDataModel>();
            mockBarDataModel.Setup(m => m.AddCategoryItem(It.IsAny<string>(), It.IsAny<string>())).Returns(AddingCategory);

            //Arrange MenuManagerViewModel
            var ListOfCategories = new List<MenuItemCategory>();

            var mockMenuManagerViewModel = new Mock<IMenuManagerViewModel>();
            mockMenuManagerViewModel.Setup(m => m.ListOfCategories).Returns(ListOfCategories);

            //Arrange AddCategoryViewModel
            var AddCategoryViewModel = new AddCategoryViewModel(mockBarDataModel.Object, mockMenuManagerViewModel.Object);
            AddCategoryViewModel.CategoryName = "";
            AddCategoryViewModel.CategoryDescription = "Cudowna";

            //Act
            string resultError;
            var result = AddCategoryViewModel.AddCategory(out resultError);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Some Fields are empty", resultError);
            Assert.IsTrue(ListOfCategories.Count == 0);


        }

        [TestMethod]
        public void AddCategory_NoCategoryDescription()
        {
            //Arrange BarDataModel 
            var AddingCategory = new MenuItemCategory();

            var mockBarDataModel = new Mock<IBarDataModel>();
            mockBarDataModel.Setup(m => m.AddCategoryItem(It.IsAny<string>(), It.IsAny<string>())).Returns(AddingCategory);

            //Arrange MenuManagerViewModel
            var ListOfCategories = new List<MenuItemCategory>();

            var mockMenuManagerViewModel = new Mock<IMenuManagerViewModel>();
            mockMenuManagerViewModel.Setup(m => m.ListOfCategories).Returns(ListOfCategories);

            //Arrange AddCategoryViewModel
            var AddCategoryViewModel = new AddCategoryViewModel(mockBarDataModel.Object, mockMenuManagerViewModel.Object);
            AddCategoryViewModel.CategoryName = "Nowa";
            AddCategoryViewModel.CategoryDescription = "";

            //Act
            string resultError;
            var result = AddCategoryViewModel.AddCategory(out resultError);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual("Some Fields are empty", resultError);
            Assert.IsTrue(ListOfCategories.Count == 0);


        }

        [TestMethod]
        public void AddCategory_DoubleName()
        {
            //Arrange BarDataModel 
            var AddingCategory = new MenuItemCategory();
            AddingCategory.Name = "Dublowana";
            AddingCategory.Description = "Doblowana Kategoria";

            var mockBarDataModel = new Mock<IBarDataModel>();
            mockBarDataModel.Setup(m => m.AddCategoryItem(It.IsAny<string>(), It.IsAny<string>())).Returns(AddingCategory);

            //Arrange MenuManagerViewModel
            var ListOfCategories = new List<MenuItemCategory>();
            ListOfCategories.Add(AddingCategory);

            var mockMenuManagerViewModel = new Mock<IMenuManagerViewModel>();
            mockMenuManagerViewModel.Setup(m => m.ListOfCategories).Returns(ListOfCategories);

            //Arrange AddCategoryViewModel
            var AddCategoryViewModel = new AddCategoryViewModel(mockBarDataModel.Object, mockMenuManagerViewModel.Object);
            AddCategoryViewModel.CategoryName = AddingCategory.Name;
            AddCategoryViewModel.CategoryDescription = "Cudowna";

            //Act
            string resultError;
            var result = AddCategoryViewModel.AddCategory(out resultError);

            //Assert
            Assert.IsFalse(result);
            Assert.AreEqual("There is category named: " + AddingCategory.Name, resultError);
            Assert.IsTrue(ListOfCategories.Count == 1);


        }

        [TestMethod]
        public void AddCategory_ErrorAdding()
        {
            //Arrange BarDataModel 
            MenuItemCategory AddingCategory = null;

            var mockBarDataModel = new Mock<IBarDataModel>();
            mockBarDataModel.Setup(m => m.AddCategoryItem(It.IsAny<string>(), It.IsAny<string>())).Returns(AddingCategory);

            //Arrange MenuManagerViewModel
            var ListOfCategories = new List<MenuItemCategory>();

            var mockMenuManagerViewModel = new Mock<IMenuManagerViewModel>();
            mockMenuManagerViewModel.Setup(m => m.ListOfCategories).Returns(ListOfCategories);

            //Arrange AddCategoryViewModel
            var AddCategoryViewModel = new AddCategoryViewModel(mockBarDataModel.Object, mockMenuManagerViewModel.Object);
            AddCategoryViewModel.CategoryName = "Nowa";
            AddCategoryViewModel.CategoryDescription = "Cudowna";

            //Act
            string resultError;
            var result = AddCategoryViewModel.AddCategory(out resultError);

            //Assert
            Assert.IsFalse(result);
            Assert.IsNotNull(resultError);
            Assert.AreEqual("Failed", resultError);
            Assert.IsTrue(ListOfCategories.Count == 0);
        }


    }
}
