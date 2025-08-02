using Microsoft.AspNetCore.Mvc;
using Bulky.DataAcess.Data;
using Bulky.Models;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Bulky.Utility;

namespace Bulkyweb.Areas.Admin.Controllers
{
    [Area("Admin")] //This is to specify that this controller is in Admin Area.
    /*[Authorize(Roles = SD.Role_Admin)]*/  //This is to specify that this controller is only for Admin Role.
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)  //constructor
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            List<Category> objCategories = _unitOfWork.Category.GetAll().ToList();
            return View(objCategories);    //objCategoryList is to display items in UI.
        }

        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name.");
            }

            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);  //to create category in ui we need to add this. we don't need to add any insert statements.
                _unitOfWork.Save();  // Here, we need to add savechare then it will save changes in database.
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);   //Method 1
            //Category? categoryFromdb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);  //Method 2 link operation try to find out if not it will return nullable
            //Category? categoryFromdb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();  //Method 3
            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);  //to Update a record in  category in ui we need to add this. we don't need to Add any update statements.
                _unitOfWork.Save();  // Here, we need to add savechare then it will save changes in database.
                TempData["success"] = "Category Updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? categoryFromdb = _unitOfWork.Category.Get(u => u.Id == id);   //Method 1
            //Category? CategoryFromdb1 = _db.Categories.FirstOrDefault(u=>u.Id==id);  //Method 2 link operation try to find out if not it will return nullable
            //Category? CategoryFromdb2 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();  //Method 3
            if (categoryFromdb == null)
            {
                return NotFound();
            }
            return View(categoryFromdb);
        }
        [HttpPost, ActionName("Delete")]  // Added code ActionName("Delete")] .
        public IActionResult DeletePost(int? id)  // Here, Delete is a method name. In Delete and Delete in Post method should not be same.
        {
            Category? obj = _unitOfWork.Category.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();  // Here, we need to delete and savechanges then it will save changes in database.
            TempData["success"] = "Category Deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
