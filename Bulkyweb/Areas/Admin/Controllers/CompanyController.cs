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
    /*[Authorize(Roles = SD.Role_Admin)]*/  //This is to specify that this controller is only for Admin Role.
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;  // This is used to get the path of the wwwroot folder.
        public CompanyController(IUnitOfWork unitOfWork)  //constructor
        {
            _unitOfWork = unitOfWork;
            

        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            
            return View(objCompanyList);    //objCompanyList is to display items in UI.
        }

        public IActionResult Upsert(int? id)
        {
            //IEnumerable<SelectListItem> CategoryList = 
            //ViewBag.CategoryList = CategoryList;  // ViewBag is used to pass data from controller to view.

            //ViewData["CategoryList"] = CategoryList;

            
            if(id == null || id == 0)
            {
                //create
                return View(new Company());
            }

            else
            {
                //update
                Company companyObj = _unitOfWork.Company.Get(u => u.Id == id);
                return View(companyObj);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            

            if (ModelState.IsValid)
            {
                
               
                if(CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }

                else
                {
                    _unitOfWork.Company.Update(CompanyObj);  //to update category in ui we need to add this. we don't need to add any update statements.
                }
                _unitOfWork.Save();  // Here, we need to add savechare then it will save changes in database.
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                
                    
                return View(CompanyObj);
            }
            
        }
       
       
        
        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var CompanyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if(CompanyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            _unitOfWork.Company.Remove(CompanyToBeDeleted);
            _unitOfWork.Save();

            
            return Json(new { success = true, message = "Delete successful" });
        }

        #endregion
    }
}
