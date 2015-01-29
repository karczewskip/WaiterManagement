using System.Web.Mvc;

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