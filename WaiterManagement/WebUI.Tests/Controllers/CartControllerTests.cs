using System;
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
	public class CartControllerTests
	{
		#region Consts

		const string SampleReturnUrl = "ReturnUrl";

		#endregion

		#region Private fields

		private IBaseDataAccess _baseDataAccess;
		private IProcessOrderCommand _proccessOrderCommand;
		private IAuthProvider _authProvider;

		private Cart _cart;
		private IEnumerable<MenuItemQuantity> _menuItemsInCart;

		#endregion

		#region Tests

		[TestMethod]
		public void Index_check_references()
		{
			//Arrange
			var controller = GetControllerInstance();
			InitializeEmptyCart();

			//Act
			var result = controller.Index(_cart,SampleReturnUrl);
			var resultModel = (CartIndexViewModel) result.ViewData.Model;

			//Assert
			Assert.IsNotNull(result);
			Assert.AreEqual(result.ViewName,"");
			Assert.IsNotNull(resultModel);
			Assert.AreEqual(resultModel.Cart, _cart);
			Assert.AreEqual(resultModel.ReturnUrl, SampleReturnUrl);
		}

		[TestMethod]
		public void AddToCart_add_to_empty_card()
		{
			//Arrange
			InitializeMenuItemsInBaseDataAccess();
			InitializeEmptyCart();
			var controller = GetControllerInstance();
			var idAddingMenuItem = _baseDataAccess.GetMenuItems().First().Id;

			//Act
			var resultRedirect = controller.AddToCart(_cart, idAddingMenuItem, SampleReturnUrl);

			//Assert
			Assert.AreEqual(_menuItemsInCart.Count(), 1);
			Assert.IsNotNull(resultRedirect);
			Assert.AreEqual("", resultRedirect.RouteName);
		}

		[TestMethod]
		[ExpectedException(typeof (ArgumentException))]
		public void AddToCart_no_item_with_this_id()
		{
			//Arrange
			InitializeBaseDataAccessWithNoItem();
			InitializeEmptyCart();

			var controller = GetControllerInstance();

			//Act
			controller.AddToCart(_cart, 0, SampleReturnUrl);
		}

		[TestMethod]
		public void RemoveFromCart_removing_existing_item()
		{
			//Arrange
			InitializeMenuItemsInBaseDataAccess();
			InitializeEmptyCart();
			var testMenuItem = _baseDataAccess.GetMenuItems().First();
			AddItemToMenuItemLists(testMenuItem);

			var controller = GetControllerInstance();

			//Act
			controller.RemoveFromCart(_cart, testMenuItem.Id, SampleReturnUrl);

			//Assert
			Assert.AreEqual(0, _menuItemsInCart.Count());
		}

		[TestMethod]
		[ExpectedException(typeof(ArgumentException))]
		public void RemoveFromCart_removing_not_added_item()
		{
			//Arrang
			InitializeMenuItemsInBaseDataAccess();
			InitializeEmptyCart();
			var testMenuItem = _baseDataAccess.GetMenuItems().First();

			var controller = GetControllerInstance();

			//Act
			controller.RemoveFromCart(_cart, testMenuItem.Id, SampleReturnUrl);
		}

		[TestMethod]
		public void Summary()
		{
			//Arrange
			InitializeEmptyCart();
			var controller = GetControllerInstance();

			//Act
			var partialViewResult = controller.Summary(_cart);

			//Assert
			Assert.IsNotNull(partialViewResult);
			Assert.AreEqual("", partialViewResult.ViewName);
			Assert.AreSame(_cart,partialViewResult.ViewData.Model);
		}

		#endregion

		#region Private methods

		private void AddItemToMenuItemLists(MenuItem testMenuItem)
		{
			var previousMenuItemQuantity = _menuItemsInCart.FirstOrDefault(m => m.MenuItem.Id == testMenuItem.Id);
			var previousMenuItemCount = previousMenuItemQuantity != null ? previousMenuItemQuantity.Quantity : 0;

			_cart.AddItem(testMenuItem, 1);
			
			Assert.AreEqual(previousMenuItemCount + 1, _menuItemsInCart.First( m => m.MenuItem.Id == testMenuItem.Id).Quantity);
		}

		private void InitializeEmptyCart()
		{
			_cart = new Cart();
			_menuItemsInCart = _cart.Lines;
		}

		private CartController GetControllerInstance()
		{
			return new CartController(_baseDataAccess, _proccessOrderCommand, _authProvider);
		}

		private void InitializeMenuItemsInBaseDataAccess()
		{
			var baseDataAccessMock = new Mock<IBaseDataAccess>();
			var sampleMenuItemCategories = new List<MenuItemCategory>()
			{
				new MenuItemCategory() { Description = "CategoryDescription1" , Id = 1, Name = "CategoryName1"},
				new MenuItemCategory() { Description = "CategoryDescription2" , Id = 2, Name = "CategoryName2"},
				new MenuItemCategory() { Description = "CategoryDescription3" , Id = 3, Name = "CategoryName3"},
				new MenuItemCategory() { Description = "CategoryDescription4" , Id = 4, Name = "CategoryName4"}
			};

			var sampleMenuItems = new List<MenuItem>()
			{
				new MenuItem()
				{
					Category = sampleMenuItemCategories[0],
					Description = "MenuItemDescription1",
					Id = 1,
					Name = "MenuItemName1",
					Price = new Money() {Amount = 10, Currency = "PLN"}
				},
				new MenuItem()
				{
					Category = sampleMenuItemCategories[1],
					Description = "MenuItemDescription2",
					Id = 2,
					Name = "MenuItemName2",
					Price = new Money() {Amount = 20, Currency = "PLN"}
				},
				new MenuItem()
				{
					Category = sampleMenuItemCategories[2],
					Description = "MenuItemDescription3",
					Id = 3,
					Name = "MenuItemName3",
					Price = new Money() {Amount = 30, Currency = "PLN"}
				},
			};

			baseDataAccessMock.Setup(b => b.GetMenuItems()).Returns(sampleMenuItems);

			_baseDataAccess = baseDataAccessMock.Object;
		}

		private void InitializeBaseDataAccessWithNoItem()
		{
			var baseDataAccessMock = new Mock<IBaseDataAccess>();
			baseDataAccessMock.Setup(b => b.GetMenuItems()).Returns(new List<MenuItem>());

			_baseDataAccess = baseDataAccessMock.Object;
		}

		#endregion


	}
}