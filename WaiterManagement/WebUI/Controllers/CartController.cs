using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLib.DataStructures;
using DataAccess;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private readonly IBaseDataAccess _baseDataAccess;
        private readonly IOrderProcessor _orderProcessor;

        public CartController(IBaseDataAccess baseDataAccess, IOrderProcessor orderProcessor)
        {
            _baseDataAccess = baseDataAccess;
            _orderProcessor = orderProcessor;
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
            if (menuItem != null)
            {
                cart.AddItem(menuItem, 1);
            }
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
        public ViewResult Checkout(Cart cart, OrderDetails orderDetails)
        {
            if (cart.Lines.Count() == 0)
            {
                ModelState.AddModelError("", "Sorry, your order is empty!");
            }
            if (ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, orderDetails);
                cart.Clear();
                return View("Completed");
            }
            else
            {
                return View(orderDetails);
            }
        }

        public ViewResult Checkout()
        {
            return View(new OrderDetails());
        }
    }
}
