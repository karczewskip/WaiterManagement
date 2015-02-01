using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ClassLib.DataStructures;
using DataAccess;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IManagerDataAccess _managerDataAccess;
        private UserContext admin;

        public AdminController(IManagerDataAccess managerDataAccess)
        {
            _managerDataAccess = managerDataAccess;
            admin = _managerDataAccess.LogIn("lamb", ClassLib.DataStructures.HashClass.CreateFirstHash("lamb", "lamb"));
        }

        //
        // GET: /Admin/

        public ViewResult Index()
        {
            return View(new List<Order>()); // _managerDataAccess.GetOrders(admin.Id));
        }

    }
}
