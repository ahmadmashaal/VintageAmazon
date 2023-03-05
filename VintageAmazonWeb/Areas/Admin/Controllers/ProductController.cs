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

public class ProductController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IWebHostEnvironment _hostEnviroment;
    public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnviroment)
    {
        _unitOfWork = unitOfWork;
        _hostEnviroment = hostEnviroment;
    }

    public IActionResult Index()
    {
        
        return View();
    }


    //GET
    public IActionResult Upsert(int? id)
    {
        //Tightly binded view
        ProductVM productVM = new()
        {
            Product = new(),
            CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
            CoverTypeList = _unitOfWork.CoverType.GetAll().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            }),
        };

        if (id == null || id == 0)
        {
            //create product

            //ViewBag.CategoryList = CategoryList;
            //ViewData["CoverTypeList"] = CoverTypeList;
            
            return View(productVM);
        }
        else
        {
            productVM.Product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == id);
            return View(productVM);
            //update product
        }

    }

    //POST
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upsert(ProductVM obj, IFormFile? file)
    {
        if (ModelState.IsValid)
        {
            string wwwRootPath = _hostEnviroment.WebRootPath;
            if(file != null)
            {
                //renaming the image file
                string fileName = Guid.NewGuid().ToString();
                //save to images/products
                var uploads = Path.Combine(wwwRootPath, @"images\products");
                //get extension
                var extension = Path.GetExtension(file.FileName);

                //when update we need to check if image already exists
                if(obj.Product.ImageUrl != null)
                {
                    var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                    //checks if image already exists
                    if(System.IO.File.Exists(oldImagePath))
                    {
                        //deletes the old image
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                //copy to images/product
                using(var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                {
                    file.CopyTo(fileStreams);
                }

                obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
            }
            if(obj.Product.Id == 0)
            {
                _unitOfWork.Product.Add(obj.Product);

            }
            else
            {
                _unitOfWork.Product.Update(obj.Product);
            }
            _unitOfWork.Save();
            TempData["success"] = "Product was created successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }
    
    
    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    { 
        var productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
        return Json(new { data = productList });
    }

    //POST
    [HttpDelete]
    public IActionResult Delete(int? id)
    {
        var obj = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
        if (obj == null)
        {
            return Json(new {success = false, message = "Error while deleting"});
        }

        var oldImagePath = Path.Combine(_hostEnviroment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
        //checks if image already exists
        if (System.IO.File.Exists(oldImagePath))
        {
            //deletes the old image
            System.IO.File.Delete(oldImagePath);
        }

        _unitOfWork.Product.Remove(obj);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Delete Successful" });
    }

    #endregion
}
