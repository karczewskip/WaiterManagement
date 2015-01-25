using System.Linq;
using System.Web.Mvc;
using DataAccess;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class MenuController : Controller
    {
        private readonly IBaseDataAccess _baseDataAccess;
        public int PageSize = 4;

        public MenuController(IBaseDataAccess baseDataAccess)
        {
            _baseDataAccess = baseDataAccess;
        }

        public ViewResult List(int page = 1)
        {
            MenuListViewModel model = new MenuListViewModel
            {
                MenuItems = _baseDataAccess.GetMenuItems()
                    .OrderBy(m => m.Id)
                    .Skip((page - 1)*PageSize)
                    .Take(PageSize),

                PagingInfo = new PagingInfo()
                {
                    CurrentPage = page,
                    ItemsPerPage = PageSize,
                    TotalItems = _baseDataAccess.GetMenuItems().Count()
                }
            };
            
            return View(model);
        }
    }
}