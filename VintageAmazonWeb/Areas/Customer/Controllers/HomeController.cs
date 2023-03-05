using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VintageAmazon.DataAccess.Repository.IRepository;
using VintageAmazon.Models;
using VintageAmazon.Models.ViewModels;

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
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");

            //when the action is Index
            return View(productList);
        }

        public IActionResult Details(int id)
        {
            ShoppingCart cartObj = new()
            {
                Count = 1,
                Product = _unitOfWork.Product.GetFirstOrDefault(x => x.Id == id, includeProperties: "Category,CoverType"),
            };
            //when the action is Index
            return View(cartObj);
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
    }
