using ElectronicShop.Models;
using ElectronicShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Controllers
{
	
	public class CategoryController : Controller
	{
		private readonly DataContext _dataContext;
		public CategoryController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index(string Slug="")
		{
			//Lấy CategoryId từ Slug
			CategoryModel category = _dataContext.Categories.Where(c => c.Slug == Slug).FirstOrDefault();

			if (category == null) { RedirectToAction("Index"); }

			var productByCategogy = _dataContext.Products.Where(p => p.CategoryId == category.Id);
			return View(await productByCategogy.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
