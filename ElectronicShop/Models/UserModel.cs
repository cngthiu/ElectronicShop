using System.ComponentModel.DataAnnotations;

namespace ElectronicShop.Models
{
	public class UserModel
	{
		public int Id { get; set; }

		[Required(ErrorMessage="Nhập Username")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Nhập Email"), EmailAddress]
		public string Email { get; set; }

		[DataType(DataType.Password), Required(ErrorMessage ="Nhap Password")]
		public string Password { get; set; }
	}
}
