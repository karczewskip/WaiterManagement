using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using ClassLib;
using ClassLib.DataStructures;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;

namespace WebUI.Controllers
{
	public class CartController : Controller
	{
		private readonly IBaseDataAccess _baseDataAccess;
		private readonly IProcessOrderCommand _orderProcessorCommand;
		private readonly IAuthProvider _authProvider;

		public CartController(IBaseDataAccess baseDataAccess, IProcessOrderCommand orderProcessor, IAuthProvider authProvider)
		{
			_baseDataAccess = baseDataAccess;
			_orderProcessorCommand = orderProcessor;
			_authProvider = authProvider;
		}

		public ViewResult Index(Cart cart, string returnUrl)
		{
			return View(new CartIndexViewModel
			{
				Cart = cart,
				ReturnUrl = returnUrl
			});
		}

		public RedirectToRouteResult AddToCart(Cart cart, int id, string returnUrl)
		{
			var menuItem = _baseDataAccess.GetMenuItems()
			.FirstOrDefault(m => m.Id == id);

			if (menuItem == null)
				throw new ArgumentException("There is no item with this id");

			cart.AddItem(menuItem, 1);

			return RedirectToAction("Index", new { returnUrl });
		}

		public RedirectToRouteResult RemoveFromCart(Cart cart, int menuItemId, string returnUrl)
		{
			var menuItem = _baseDataAccess.GetMenuItems()
			.FirstOrDefault(m => m.Id == menuItemId);
			if (menuItem != null)
			{
				cart.RemoveLine(menuItem);
			}
			return RedirectToAction("Index", new { returnUrl });
		}

		public PartialViewResult Summary(Cart cart)
		{
			return PartialView(cart);
		}

		[HttpPost]
		[Authorize]
		public ViewResult Checkout(Cart cart, OrderDetails orderDetails)
		{
			if (!ModelState.IsValid)
				return View(orderDetails);

			if (CheckCartContent(cart))
				if (!ProcessOrder(cart, orderDetails))
					return View("Error");

			cart.Clear();
			return View("Completed");
		}

		private bool CheckCartContent(Cart cart)
		{
			if (cart.Lines.Any()) return true;

			ModelState.AddModelError("", ApplicationResources.EmptyOrderMessage);
			return false;
		}

		private bool ProcessOrder(Cart cart, OrderDetails orderDetails)
		{
			orderDetails.ClientId = _authProvider.GetClientId();

			if (_orderProcessorCommand.Execute(cart, orderDetails)) return true;

			ModelState.AddModelError("", ApplicationResources.ProcessingErrorMessage);
			return false;
		}

		public ViewResult Checkout()
		{
			return View(new OrderDetails() { Date = DateTime.Now });
		}
	}
}
