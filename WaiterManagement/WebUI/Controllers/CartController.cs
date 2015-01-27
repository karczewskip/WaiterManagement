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

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = GetCart(),
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(int id, string returnUrl)
        {
            var menuItem = _baseDataAccess.GetMenuItems()
            .FirstOrDefault(m => m.Id == id);
            if (menuItem != null)
            {
                GetCart().AddItem(menuItem, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int menuItemId, string returnUrl)
        {
            var menuItem = _baseDataAccess.GetMenuItems()
            .FirstOrDefault(m => m.Id == menuItemId);
            if (menuItem != null)
            {
                GetCart().RemoveLine(menuItem);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCart()
        {
            var cart = (Cart)Session["Cart"];

            if (cart != null) return cart;

            cart = new Cart();
            Session["Cart"] = cart;
            return cart;
        }
    }
}
