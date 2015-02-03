using System.Web.Mvc;
using System.Web.Security;
using WebUI.Infrastructure.Abstract;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    {
        IAuthProvider authProvider;

        public AccountController(IAuthProvider auth)
        {
            authProvider = auth;
        }

        public ViewResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(model.UserName, model.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", "Cart"));
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        public PartialViewResult Summary()
        {
            return PartialView(authProvider);
        }

        public void Logout()
        {
            FormsAuthentication.SignOut();
        }
    }
}