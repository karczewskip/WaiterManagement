using System.Web.Mvc;
using DataAccess;

namespace WebUI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IBaseDataAccess _baseDataAccess;

        public MenuController(IBaseDataAccess baseDataAccess)
        {
            _baseDataAccess = baseDataAccess;
        }

        public ViewResult List()
        {
            return View(_baseDataAccess.GetMenuItems());
        }
    }
}