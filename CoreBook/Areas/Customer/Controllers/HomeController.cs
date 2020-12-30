using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreBook.Models;
using CoreBook.Models.ViewModels;
using CoreBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using CoreBook.Utility;
using Microsoft.AspNetCore.Http;

namespace CoreBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        // FG ANNOYING WARNING: Message	IDE0052	Private member 'HomeController._logger' can be removed as the value assigned to it is never read
        /*
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        */

        private readonly IUnitOfWork _unitOfWork;
        public HomeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");

            var claimIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if (claim != null)
            {
                var count = _unitOfWork.ShoppingCart.GetAll(s => s.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);
            }

            return View(productList);
        }

        public IActionResult Details(int id)
        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(p => p.ID == id, includeProperties: "Category,CoverType");
            ShoppingCart shoppingCart = new ShoppingCart()
            {
                Product = productFromDb,
                ProductId = productFromDb.ID
            };

            return View(shoppingCart);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart cartObject)
        {
            cartObject.Id = 0;
            if (ModelState.IsValid)
            {
                // FG Add to cart, after we get user id
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                cartObject.ApplicationUserId = claim.Value;

                ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.GetFirstOrDefault(u =>
                   u.ApplicationUserId == cartObject.ApplicationUserId &&
                   u.ProductId == cartObject.ProductId,
                    includeProperties: "Product");

                if (cartFromDb == null)
                {
                    // FG no records exists in db for that product for that user
                    _unitOfWork.ShoppingCart.Add(cartObject);
                }
                else
                {
                    // FG update count - which is available thanks to asp-for in the form
                    //cartObject.Count += cartFromDb.Count; 
                    //_unitOfWork.ShoppingCart.Update(cartObject); // FG on update cartObject will have Id = 0 and will insert a new record

                    cartFromDb.Count += cartObject.Count;
                    // _unitOfWork.ShoppingCart.Update(cartFromDb); // FG no need because EntityFramework keeps tracking of the objects from db
                }
                _unitOfWork.Save();

                var count = _unitOfWork.ShoppingCart
                        .GetAll(s => s.ApplicationUserId == cartObject.ApplicationUserId)
                        .ToList().Count();

                // FG In Session we will store number of items in the cart
                HttpContext.Session.SetInt32(SD.ssShoppingCart, count);

                //HttpContext.Session.SetObject(SD.ssShoppingCart, cartObject); // we can store object not just int or string
                //var cartOjb = HttpContext.Session.GetObject<ShoppingCart>(SD.ssShoppingCart);

                return RedirectToAction(nameof(Index));
            }
            else
            {
                // FG re-populate fields
                var productFromDb = _unitOfWork.Product.GetFirstOrDefault(p => p.ID == cartObject.ProductId, includeProperties: "Category,CoverType");
                ShoppingCart shoppingCart = new ShoppingCart()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.ID
                };

                return View(shoppingCart);
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
