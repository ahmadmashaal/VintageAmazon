using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.DataAccess.Repository;
using VintageAmazon.Models;
using VintageAmazon.Models.ViewModels;
using VintageAmazon.Utility;
using Microsoft.AspNet.Identity;
using System.Xml.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;
//using System.Web.Mvc;

namespace VintageAmazonWeb.Areas.Customer.Controllers;
[Area("Customer")]

//when the controller is Home
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }


    //routing
    public IActionResult Index()
    {
        IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

        //when the action is Index
        return View(productList);
    }

    public IActionResult Details(int productId)
    {
        var product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId);
        //var comment = _unitOfWork.Comment.Search(productId);

        var viwModel = new ProductVM
        {
            Product = product,
            //Comment = comment
        };

        ShoppingCart cartObj = new()
        {
            Count = 1,
            ProductId = productId,
            Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == productId, includeProperties: "Category,CoverType"),
        };
        //when the action is Index
        return View(cartObj);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult Details(ShoppingCart shoppingCart)
    {
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        shoppingCart.ApplicationUserId = claim.Value;

        ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(
            u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId);

        if (cartFromDb == null)
        {
            _unitOfWork.ShoppingCart.Add(shoppingCart);
            _unitOfWork.Save();
            HttpContext.Session.SetInt32(SD.SessionCart, _unitOfWork.ShoppingCart.GetAll(x => x.ApplicationUserId == claim.Value).ToList().Count);
        }
        else
        {
            _unitOfWork.ShoppingCart.IncrementCount(cartFromDb, shoppingCart.Count);
            _unitOfWork.Save();

        }

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        //when the action is Privacy
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Review()
    {

        return View();

    }

    [HttpPost]
    public ActionResult Search(string term)
    {
        if (term != null)
        {
            IEnumerable<Product> result = _unitOfWork.Product.Search(term);
            return View(result);
        }
        else
        {
            return RedirectToAction(nameof(Index));
        }
    }

    [HttpPost]
    public IActionResult AdvanceSearch(IFormCollection form)
    {
        var title = form["searchTitle"];
        var author = form["searchAuthor"];
        var category = form["searchCategory"];
        double minPrice = 0;
        if (string.IsNullOrEmpty(form["searchPriceMin"]))
        {
            minPrice = 0;
        }
        else
        {
            minPrice = double.Parse(form["searchPriceMin"]);
        }
        double maxPrice = 1000;
        if (string.IsNullOrEmpty(form["searchPriceMax"]))
        {
            maxPrice = 1000;
        }
        else
        {
            maxPrice = double.Parse(form["searchPriceMax"]);
        }
        var products = _unitOfWork.Product.AdvanceSearch(title, author, category, minPrice, maxPrice);
        return View(products);
    }

    [HttpPost]
    public JsonResult PostRating(int rating, int pid)
    {
        //save data into the database
        StarRating rt = new StarRating();
        rt.Rate = rating;
        rt.ProductId = pid;

        //save into the database
        _unitOfWork.StarRating.Add(rt);
        _unitOfWork.Save();

        return Json("You rated this " + rating.ToString() + " star(s)");
    }

    //[HttpPost]
    //public IActionResult AdvanceSearch(IFormCollection form)
    //{
    //    var searchTitle = form["searchTitle"];
    //    var searchAuthor = form["searchAuthor"];
    //    var searchCategory = form["searchCategory"];
    //    var searchPriceMin = form["searchPriceMin"];
    //    var searchPriceMax = form["searchPriceMax"];

    //    var products = _unitOfWork.Product.GetAll(p => true).ToList();

    //    if (!string.IsNullOrEmpty(searchTitle))
    //    {
    //        products = products.Where(p => p.Title.ToLower().Contains(searchTitle)).ToList();
    //    }

    //    if (!string.IsNullOrEmpty(searchAuthor))
    //    {
    //        products = products.Where(p => p.Author.ToLower().Contains(searchAuthor)).ToList();
    //    }


    //    if (!string.IsNullOrEmpty(searchCategory))
    //    {
    //        products = products.Where(p => p.Category.Name == searchCategory).ToList();
    //    }

    //    if (!string.IsNullOrEmpty(searchPriceMin))
    //    {
    //        double minPrice = double.Parse(searchPriceMin);
    //        products = products.Where(p => p.Price >= minPrice).ToList();
    //    }

    //    if (!string.IsNullOrEmpty(searchPriceMax))
    //    {
    //        double maxPrice = double.Parse(searchPriceMax);
    //        products = products.Where(p => p.Price <= maxPrice).ToList();
    //    }

    //    var categories = _unitOfWork.Category.GetAll().ToList();

    //    var productListItems = products.Select(p => new SelectListItem
    //    {
    //        Text = p.Title,
    //        Value = p.Id.ToString()
    //    }).ToList();

    //    var categoryListItems = categories.Select(c => new SelectListItem
    //    {
    //        Text = c.Name,
    //        Value = c.Id.ToString()
    //    }).ToList();

    //    var vm = new ProductVM
    //    {
    //        ProductList = productListItems,
    //        CategoryList = categoryListItems,
    //    };

    //    return View(vm);
    //}


    //[HttpPost]
    //public ActionResult AddReview(int productId, int rating, string text)
    //{
    //    //var comment = new Comment
    //    {
    //        Rating = rating,
    //        Text = text,
    //        ProductId = productId
    //    };
    //    if (ModelState.IsValid)
    //    {
    //        //_unitOfWork.Comment.Add(comment);
    //        _unitOfWork.Save();

    //        return RedirectToAction("Details", new { id = comment.ProductId });

    //    }
    //    else
    //    {
    //        return RedirectToAction("Details", new { id = comment.ProductId });
    //    }

    //}

    //[HttpPost]
    //    public IActionResult LeaveComment(int? id)
    //    {
    //        if (id == null)
    //        {
    //            return BadRequest();
    //        }
    //        else
    //        {
    //            Product product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id);
    //            CommentVM commentVM = new CommentVM();

    //            if (commentVM == null)
    //            {
    //                return BadRequest();
    //            }

    //            commentVM.ProductId = id.Value;

    //            //var Comments = _unitOfWork.Comment.Search(id);
    //            //commentVM.ListOfComment = Comments.ToList();

    //            //var ratings = _unitOfWork.Comment.Search(id);

    //            if (ratings.Count() > 0)
    //            {
    //                var ratingSum = ratings.Sum(x => x.Rating);
    //                ViewBag.RatingSum = ratingSum;
    //                var ratingCount = ratings.Count();
    //                ViewBag.RatingCount = ratingCount;
    //            }
    //            else
    //            {
    //                ViewBag.RatingSum = 0;
    //                ViewBag.RatingCount = 0;
    //            }

    //            return View(commentVM);
    //        }

    //    }
}
