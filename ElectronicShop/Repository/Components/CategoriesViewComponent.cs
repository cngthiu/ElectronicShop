using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Repository.Component
{
	public class CategoriesViewComponent : ViewComponent 
	{
		private readonly DataContext _dataContext;
		public CategoriesViewComponent(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		//Display List category in side bar
		public async Task<IViewComponentResult> InvokeAsync()
		{
			return View(await _dataContext.Categories.ToListAsync());
		}
	}
}
