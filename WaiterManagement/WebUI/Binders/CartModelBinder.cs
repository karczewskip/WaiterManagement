using System.Web.Mvc;
using ClassLib.DataStructures;

namespace WebUI.Binders
{
    public class CartModelBinder : IModelBinder
    {
        private const string sessionKey = "Cart";
        public object BindModel(ControllerContext controllerContext,
        ModelBindingContext bindingContext)
        {
            var cart = (Cart)controllerContext.HttpContext.Session[sessionKey];

            if (cart != null) return cart;

            cart = new Cart();
            controllerContext.HttpContext.Session[sessionKey] = cart;
            // return the cart
            return cart;
        }
    }
}