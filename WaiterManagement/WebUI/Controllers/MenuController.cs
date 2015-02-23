using System.Linq;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.Infrastructure.Abstract;

namespace WebUI.Controllers
{
	public class MenuController : Controller
	{
		#region Consts

		public static int PageSize
		{
			get { return 4; }
		}

		#endregion

		#region Dependencies

		private readonly IBaseDataAccess _baseDataAccess;

		#endregion

		#region Constructors

		public MenuController(IBaseDataAccess baseDataAccess)
		{
			_baseDataAccess = baseDataAccess;
		}

		#endregion

		#region Actions

		public ViewResult List(string category, int page = 1)
		{
			var model = new MenuListViewModel
			{
				MenuItems = _baseDataAccess.GetMenuItems()
					.Where(m => category == null || m.Category.Name == category)
					.OrderBy(m => m.Id)
					.Skip((page - 1) * PageSize)
					.Take(PageSize),

				PagingInfo = new PagingInfo()
				{
					CurrentPage = page,
					ItemsPerPage = PageSize,
					TotalItems = category == null ?
						_baseDataAccess.GetMenuItems().Count() :
						_baseDataAccess.GetMenuItems().Count(e => e.Category.Name == category)
				},
				CurrentCategory = category
			};

			ViewBag.SelectedCategory = category;

			return View(model);
		}

		#endregion
	}
}