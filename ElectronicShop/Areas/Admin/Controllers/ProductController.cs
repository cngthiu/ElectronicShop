using ElectronicShop.Models;
using ElectronicShop.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ElectronicShop.Areas.Admin.Controllers
{
    [Area("Admin")]
	[Authorize]
	public class ProductController : Controller
    {
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _IWebHostEnv;
        public ProductController(DataContext dataContext, IWebHostEnvironment IWebHostEnv)
        {
            _dataContext = dataContext;
            _IWebHostEnv = IWebHostEnv;
        }
        public async Task<IActionResult> Index()
        {
            return View( await _dataContext.Products.OrderByDescending(p=> p.Id).Include(p=>p.Category).Include(p=>p.Brand).ToListAsync());
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name");
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name");
            return View();
        }

        #region Create product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Create(ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name",product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);

            //Mếu các dữ liệu đầu vào valid hết thì thêm dữ liệu, không thì in ra lỗi 
            if (ModelState.IsValid)
            {
                product.Slug  = product.Name.Replace(" ","-");
                var slug = _dataContext.Products.FirstOrDefault(p=>p.Slug == product.Slug);
                if (slug != null)
                {
                    ModelState.AddModelError("", "Product has had in database !");
                    return View(product);
                }

                if (product.ImageUpLoad != null) 
                {
                    string uploadDir = Path.Combine(_IWebHostEnv.WebRootPath, "images");
                    //tao ten moi cho Imageupload
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpLoad.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);
                    //coppy file anh vao folder voi ten moi
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpLoad.CopyToAsync(fs);
                    fs.Close();
                    //coppy ten vao cot Image cua bang Prodcut
                    product.Image = imageName;
                }
                else
                {
                    TempData["ErrorMessage"] = "Thêm ảnh không thành công!";
                }
                _dataContext.Products.Add(product);
                await _dataContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Error!!";
                List<string> errors = new List<string>();
                foreach(var value in ModelState.Values)
                {
                    foreach(var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }
            
            return View(product);
        }
        #endregion
        #region Edit product
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, ProductModel product)
        {
            ViewBag.Categories = new SelectList(_dataContext.Categories, "Id", "Name", product.CategoryId);
            ViewBag.Brands = new SelectList(_dataContext.Brands, "Id", "Name", product.BrandId);
            var existed_product =  _dataContext.Products.Find(product.Id);

            //Mếu các dữ liệu đầu vào valid hết thì thêm dữ liệu, không thì in ra lỗi 
            if (ModelState.IsValid)
            {
                product.Slug = product.Name.Replace(" ", "-");

                if (product.ImageUpLoad != null)
                {
                    //upload new image
                    string uploadDir = Path.Combine(_IWebHostEnv.WebRootPath, "images");
                    
                    //tao ten moi cho Imageupload
                    string imageName = Guid.NewGuid().ToString() + "_" + product.ImageUpLoad.FileName;
                    string filePath = Path.Combine(uploadDir, imageName);

                    //delete old image
                    string oldFile = Path.Combine(uploadDir, existed_product.Image);
                    try
                    {
                        if (System.IO.File.Exists(oldFile))
                        {
                            System.IO.File.Delete(oldFile);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("Error", "An error occured while deleting the product image");
                    }

                    //coppy file anh vao folder voi ten moi
                    FileStream fs = new FileStream(filePath, FileMode.Create);
                    await product.ImageUpLoad.CopyToAsync(fs);
                    fs.Close();
                    //coppy ten vao cot Image cua bang Prodcut
                    existed_product.Image = imageName;
                }

                //Update others product properties
                existed_product.Name = product.Name;
                existed_product.Description = product.Description;
                existed_product.Price = product.Price;
                existed_product.BrandId = product.BrandId;
                existed_product.CategoryId = product.CategoryId;

                _dataContext.Products.Update(existed_product);//Update existing prodcut
                await _dataContext.SaveChangesAsync();
                TempData["SuccessMessage"] = "Sửa sản phẩm thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Error!!";
                List<string> errors = new List<string>();
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }
                string errorMessage = string.Join("\n", errors);
                return BadRequest(errorMessage);
            }

            return View(product);
        }
        #endregion
        public async Task<IActionResult> Delete(int Id)
        {
            ProductModel product = await _dataContext.Products.FindAsync(Id);
            if (product == null)
            {
                return NotFound();
            }

            string uploadDir = Path.Combine(_IWebHostEnv.WebRootPath, "images");
            string oldFile = Path.Combine(uploadDir, product.Image);
            try
            {
                if (System.IO.File.Exists(oldFile))
                {
                    System.IO.File.Delete(oldFile);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "An error occured while deleting the product image");
            }

            _dataContext.Products.Remove(product);
            await _dataContext.SaveChangesAsync();
            TempData["ErrorMessage"] = "Đã xóa sản phẩm !!";
            return RedirectToAction("Index");
        }
    }
}
