using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Models.ViewModel
{
    public class LoginViewModel
    {
		public int Id { get; set; }

		[Required(ErrorMessage = "Nhập Username")]
		public string Username { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage = "Nhap Password")]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }
	}
}
