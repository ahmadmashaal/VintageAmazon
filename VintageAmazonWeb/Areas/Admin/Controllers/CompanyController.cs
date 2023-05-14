using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using VintageAmazon.DataAccess;
using VintageAmazon.DataAccess.Repository;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.Models;
using VintageAmazon.Models.ViewModels;

namespace VintageAmazonWeb.Areas.Admin.Controllers;
[Area("Admin")]

public class CompanyController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnviroment;
    public CompanyController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public IActionResult Index()
    {
        
        return View();
    }


    //GET
    public IActionResult Upsert(int? id)
    {
        //Tightly binded view
        Company company = new();

        if (id == null || id == 0)
        {
            return View(company);
        }
        else
        {
            company = _unitOfWork.Company.GetFirstOrDefault(i => i.Id == id);
            return View(company);
            //update Company
        }

    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(Company obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            if(obj.Id == 0)
            {
                _unitOfWork.Company.Add(obj);
                TempData["success"] = "Company was updated successfully";
            }
            else
            {
                _unitOfWork.Company.Update(obj);
            }
            _unitOfWork.Save();
            TempData["success"] = "Company was created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    
    
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    { 
        var companyList = _unitOfWork.Company.GetAll();
        return Json(new { data = companyList });
    }

    //POST
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Company.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
        {
            return Json(new {success = false, message = "Error while deleting"});
        }

        _unitOfWork.Company.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successful" });
    }

    #endregion
}
