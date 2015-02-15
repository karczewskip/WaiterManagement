using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        private readonly IBaseDataAccess _baseDataAccess;

        public NavController(IBaseDataAccess baseDataAccess)
        {
            _baseDataAccess = baseDataAccess;
        }

        //
        // GET: /Nav/

        public PartialViewResult Menu(string category = null)
        {
            ViewBag.SelectedCategory = category;

            IEnumerable<string> categories = _baseDataAccess.GetMenuItems()
                .Select(x => x.Category.Name)
                .Distinct()
                .OrderBy(x => x);

            return PartialView(categories);
        }

    }
}
