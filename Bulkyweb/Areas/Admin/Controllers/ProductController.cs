using Microsoft.AspNetCore.Mvc;
using Bulky.DataAcess.Data;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utility;

namespace Bulkyweb.Areas.Admin.Controllers
{
    [Area("Admin")] //This is to specify that this controller is in Admin Area.
    //[Authorize(Roles = SD.Role_Admin)]  //This is to specify that this controller is only for Admin Role.
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;  // This is used to get the path of the wwwroot folder.
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)  //constructor
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties:"Category").ToList();
            
            return View(objProductList);    //objProductList is to display items in UI.
        }

        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = 
            //ViewBag.CategoryList = CategoryList;  // ViewBag is used to pass data from controller to view.

            //ViewData["CategoryList"] = CategoryList;

            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()    // Using Projection to get only required data. very powerful feature.
                }),
            Product = new Product()
            };
            if(id == null || id == 0)
            {
                //create
                return View(productVM);
            }

            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }
        }
        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            

            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;  // This is used to get the path of the wwwroot folder.
                if(file !=null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName) ;
                    string productPath = Path.Combine(wwwRootPath ,@"Images\Product");

                    if(!string .IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete the old image.
                       var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath)) // This is to check if the file exists or not.
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }
                    productVM.Product.ImageUrl = @"\Images\Product\" + fileName;
                }
                if(productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);  //to create category in ui we need to add this. we don't need to add any insert statements.
                }

                else
                {
                    _unitOfWork.Product.Update(productVM.Product);  //to update category in ui we need to add this. we don't need to add any update statements.
                }
                _unitOfWork.Save();  // Here, we need to add savechare then it will save changes in database.
                TempData["success"] = "Product created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()    // Using Projection to get only required data. very powerful feature.
                });
                    
                return View(productVM);
            }
            
        }
       
       
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll(includeProperties: "Category").ToList();
            return Json(new { data = objProductList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var productToBeDeleted = _unitOfWork.Product.Get(u => u.Id == id);
            if(productToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                productToBeDeleted.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(productToBeDeleted);
            _unitOfWork.Save();

            
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
