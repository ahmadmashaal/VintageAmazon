using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VintageAmazon.DataAccess;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.Models;

namespace VintageAmazonWeb.Areas.Admin.Controllers;
[Area("Admin")]


public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }

        //GET
        public IActionResult Create()
        {

            return View();
        }

        //POST
        [HttpPost]
        //To prevent the cross site request forgery attack (Highly recommended)
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "The Display Order cannot exactly match the Name.");
            }
            //Checks for the validity of the information entered from the user
            if (ModelState.IsValid)
            {
                //Adds the information to the Database
                _unitOfWork.Category.Add(obj);
                //Saves the changes in the Database
                _unitOfWork.Save();
                TempData["success"] = "The category was created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        //GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var catergoryFromDb = _db.Categories.Find(id);

            //Other ways to find from DB
            var catergoryFromDbSingle = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);
            //var catergoryFromDbFirst = _db.Categories.FirstOrDefault(x => x.Id == id);

            if (catergoryFromDbSingle == null) { return NotFound(); }


            return View(catergoryFromDbSingle);
        }

        //POST
        [HttpPost]
        //To prevent the cross site request forgery attack (Highly recommended)
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("DisplayOrder", "The Display Order cannot exactly match the Name.");
            }
            //Checks for the validity of the information entered from the user
            if (ModelState.IsValid)
            {
                //Updates the information to the Database
                _unitOfWork.Category.Update(obj);
                //Saves the changes in the Database
                _unitOfWork.Save();
                TempData["success"] = "The category was updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var catergoryFromDb = _db.Categories.Find(id);

            //Other ways to find from DB
            //var catergoryFromDbSingle = _db.Categories.SingleOrDefault(x => x.Id == id);
            var catergoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            if (catergoryFromDbFirst == null) { return NotFound(); }


            return View(catergoryFromDbFirst);
        }

        //POST
        [HttpPost, ActionName("Delete")]
        //To prevent the cross site request forgery attack (Highly recommended)
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == id);

            //Checks for the validity of the information
            if (obj == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "The category was removed successfully";

            return RedirectToAction("Index");
        }
    }
