using System.Collections.Generic;
using System.Linq;
using ClassLib.DataStructures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebUI.Controllers;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;

namespace WebUI.Tests.Controllers
{
	[TestClass]
	public class MenuControllerTests
	{
		#region Dependencies

		private IBaseDataAccess _baseDataAccess;

		#endregion

		#region Private Fields

		private MenuController _controller;
		private List<MenuItem> _menuItemList;

		#endregion

		[TestMethod]
		public void List_show_menu_item_list_with_less_number_than_page_size()
		{
			//Arrange
			var testCategory = new MenuItemCategory()
			{
				Description = "MenuITemCategoryDescription1",
				Name = "MenuItemCategoryName1",
				Id = 1
			};

			var menuItemsCount = MenuController.PageSize / 2;

			_menuItemList = new List<MenuItem>();
			for (var i = 0; i < menuItemsCount; i++)
			{
				_menuItemList.Add(new MenuItem()
				{
					Category = testCategory,
					Description = "MenuItemDescription" + (i + 1),
					Id = i + 1,
					Name = "MenuItemName" + (i + 1),
					Price = new Money() { Amount = 10 * (i + 1), Currency = "PLN" }
				});
			}

			InitilizeBaseDataAccessWithList(_menuItemList);
			_controller = GetMenuControllerInstance();

			//Act
			var result = _controller.List(null);
			var model = (MenuListViewModel)result.Model;

			//Assert
			Assert.IsNotNull(model);
			Assert.AreEqual(null, model.CurrentCategory);
			Assert.AreEqual(1, model.PagingInfo.CurrentPage);
			Assert.AreEqual(menuItemsCount, model.PagingInfo.TotalItems);
			Assert.IsNotNull(model.MenuItems);
			Assert.AreEqual(_menuItemList.Count, model.MenuItems.Count());
			foreach (var menuItem in model.MenuItems)
			{
				Assert.IsTrue(_menuItemList.Contains(menuItem));
			}
		}

		[TestMethod]
		public void List_show_menu_items_from_current_category()
		{
			//Arrange
			var testCategory1 = new MenuItemCategory()
			{
				Description = "MenuITemCategoryDescription1",
				Name = "MenuItemCategoryName1",
				Id = 1
			};

			var testCategory2 = new MenuItemCategory()
			{
				Description = "MenuITemCategoryDescription2",
				Name = "MenuItemCategoryName2",
				Id = 2
			};

			_menuItemList = new List<MenuItem>();

			var menuItemsFirstCategoryCount = MenuController.PageSize + 1;

			for (var i = 0; i < menuItemsFirstCategoryCount; i++)
			{
				_menuItemList.Add(new MenuItem()
				{
					Category = testCategory1,
					Description = "MenuItemDescription" + (i + 1),
					Id = i + 1,
					Name = "MenuItemName" + (i + 1),
					Price = new Money() { Amount = 10 * (i + 1), Currency = "PLN" }
				});
			}
			var firstPageCount = MenuController.PageSize;
			var secondPageCount = 1;

			var menuItemsSecondCategoryCount = MenuController.PageSize + 2;

			for (var i = 0; i < menuItemsSecondCategoryCount; i++)
			{
				_menuItemList.Add(new MenuItem()
				{
					Category = testCategory2,
					Description = "MenuItemDescription" + (menuItemsFirstCategoryCount + i + 1),
					Id = menuItemsFirstCategoryCount + i + 1,
					Name = "MenuItemName" + (menuItemsFirstCategoryCount + i + 1),
					Price = new Money() { Amount = 10 * (i + 1), Currency = "PLN" }
				});
			}

			InitilizeBaseDataAccessWithList(_menuItemList);
			_controller = GetMenuControllerInstance();

			var secondCategoryFirstPageCount = MenuController.PageSize;
			var secondCategorySecondPageCount = 2;

			//Act && Assert
			CheckCurrentPageCurrentCategory(testCategory1, menuItemsFirstCategoryCount, firstPageCount, 1);
			CheckCurrentPageCurrentCategory(testCategory1, menuItemsFirstCategoryCount, secondPageCount, 2);
			CheckCurrentPageCurrentCategory(testCategory2, menuItemsSecondCategoryCount, secondCategoryFirstPageCount, 1);
			CheckCurrentPageCurrentCategory(testCategory2, menuItemsSecondCategoryCount, secondCategorySecondPageCount, 2);

		}

		private void CheckCurrentPageCurrentCategory(MenuItemCategory testCategory, int menuItemsCount, int currentPageItemsCount, int currentPageNumber)
		{
			//Act
			var resultCategory = _controller.List(testCategory.Name,currentPageNumber);
			var modelCategory = (MenuListViewModel)resultCategory.Model;

			//Assert
			Assert.IsNotNull(modelCategory);
			Assert.AreEqual(testCategory.Name, modelCategory.CurrentCategory);
			Assert.AreEqual(currentPageNumber, modelCategory.PagingInfo.CurrentPage);
			Assert.AreEqual(menuItemsCount, modelCategory.PagingInfo.TotalItems);
			Assert.IsNotNull(modelCategory.MenuItems);
			Assert.AreEqual(currentPageItemsCount, modelCategory.MenuItems.Count());
			foreach (var menuItem in modelCategory.MenuItems)
			{
				Assert.IsTrue(_menuItemList.Contains(menuItem));
			}
		}

		private MenuController GetMenuControllerInstance()
		{
			return new MenuController(_baseDataAccess);
		}

		private void InitilizeBaseDataAccessWithList(List<MenuItem> menuItems)
		{
			var baseDataAccess_Mock = new Mock<IBaseDataAccess>();

			baseDataAccess_Mock.Setup(b => b.GetMenuItems()).Returns(menuItems);

			_baseDataAccess = baseDataAccess_Mock.Object;
		}
	}
}