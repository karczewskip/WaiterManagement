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

        public CartController(IBaseDataAccess baseDataAccess)
        {
            _baseDataAccess = baseDataAccess;
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
    }
}
