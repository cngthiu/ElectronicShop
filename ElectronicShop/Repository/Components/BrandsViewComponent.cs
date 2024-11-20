using ElectronicShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Repository.Components
{
    public class BrandsViewComponent:ViewComponent
    {
        private readonly DataContext _datacontext;
		public BrandsViewComponent(DataContext dataContext)
		{
			_datacontext = dataContext;
		}
		public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _datacontext.Brands.ToListAsync());
        }
    }
}
