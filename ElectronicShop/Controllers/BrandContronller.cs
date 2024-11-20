using ElectronicShop.Models;
using ElectronicShop.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Controllers
{
	public class BrandController : Controller
	{
		private readonly DataContext _dataContext;
		public BrandController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public async Task<IActionResult> Index(string Slug)
		{
			//Lấy CategoryId từ Slug
			BrandModel brand = _dataContext.Brands.Where(c => c.Slug == Slug).FirstOrDefault();

			if (brand == null) { RedirectToAction("Index"); }

			var productByCategogy = _dataContext.Products.Where(p => p.BrandId == brand.Id);
			return View(await productByCategogy.OrderByDescending(p => p.Id).ToListAsync());
		}
	}
}
