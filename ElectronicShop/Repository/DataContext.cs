using ElectronicShop.Models;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Repository
{
	public class DataContext : DbContext
	{
		public DataContext(DbContextOptions<DataContext> options):base(options) { }
		public DbSet<ProductModel> Products { get; set; }
		public DbSet<CategoryModel> Categories { get; set; }
		public DbSet<BrandModel> Brands { get; set; }
	}
}
