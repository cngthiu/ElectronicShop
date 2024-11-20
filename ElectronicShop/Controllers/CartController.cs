using ElectronicShop.Repository;
using Microsoft.AspNetCore.Mvc;
using ElectronicShop.Models;
using ElectronicShop.Models.ViewModel;

namespace ElectronicShop.Controllers
{

	public class CartController : Controller
	{
		private readonly DataContext _dataContext;
		public CartController(DataContext dataContext)
		{
			_dataContext = dataContext;
		}
		public IActionResult Index()
		{
			List<CartItemModel> cartItems = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
			CartItemViewModel cartItemViewModel = new ()
			{
				CartItems = cartItems,
				GrandTotal = cartItems.Sum(x => x.Quantity * x.Price)
			};
			return View(cartItemViewModel);
		}
		//Thêm sản phẩm vào giỏ hàng 
		public async Task<IActionResult> Add(int Id)
		{
			try
			{
				ProductModel product = await _dataContext.Products.FindAsync(Id);
				List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new List<CartItemModel>();
				CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
				if (cartItem == null)
				{
					cart.Add(new CartItemModel(product));
				}
				else
				{
					cartItem.Quantity++;
				}
				HttpContext.Session.SetJson("Cart", cart);
				TempData["SuccessMessage"] = "Dữ liệu đã được tạo thành công!";
			}
			catch (Exception ex)
			{
				TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
			}
			return Redirect(Request.Headers["Referer"].ToString());
		}
		//Giam so luong san pham 
		public async Task<IActionResult> Descrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity > 1)
			{
				--cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			//Nếu không còn sp nào thì xóa luôn Cart
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			//Nếu còn sp khác thì set lại session
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			return Redirect(Request.Headers["Referer"].ToString());
		}
		#region Inscrease the product	
		public async Task<IActionResult> Inscrease(int Id)
		{
			List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
			CartItemModel cartItem = cart.Where(c => c.ProductId == Id).FirstOrDefault();
			if (cartItem.Quantity >= 1)
			{
				++cartItem.Quantity;
			}
			else
			{
				cart.RemoveAll(p => p.ProductId == Id);
			}
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove("Cart");
			}
			else
			{
				HttpContext.Session.SetJson("Cart", cart);
			}
			return RedirectToAction("Index");
		}
		#endregion
		public async Task<IActionResult> Remove(int Id)
		{
            try
            {
                List<CartItemModel> cart = HttpContext.Session.GetJson<List<CartItemModel>>("Cart");
                cart.RemoveAll(p => p.ProductId == Id);
                //Nếu không còn sp nào thì xóa luôn Cart
                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                //Nếu còn sp khác thì set lại session
                else
                {
                    HttpContext.Session.SetJson("Cart", cart);
                }
                TempData["SuccessMessage"] = "Xóa thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Có lỗi xảy ra: " + ex.Message;
            }
            return RedirectToAction("Index");
		}


	}
}
