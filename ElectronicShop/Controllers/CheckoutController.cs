using Microsoft.AspNetCore.Mvc;

namespace ElectronicShop.Controllers
{
	public class CheckoutController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
