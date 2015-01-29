using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace WebUI.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/

        public ViewResult Index()
        {
            return View();
        }

        public ViewResult Contact()
        {
            return View();
        }

    }
}
