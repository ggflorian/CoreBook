using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;
using CoreBook.Models.ViewModels;
using CoreBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CoreBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<IdentityUser> _userManager;

        public ShoppingCartVM ShoppingCartVM { get; set; }

        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _emailSender = emailSender;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            ShoppingCartVM = new ShoppingCartVM()
            {
                OrderHeader = new OrderHeader(),
                ListCart = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value, includeProperties: "Product")
            };
            ShoppingCartVM.OrderHeader.OrderTotal = 0;
            ShoppingCartVM.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                                                            .GetFirstOrDefault(u => u.Id == claim.Value, includeProperties: "Company");                                                            

            foreach(var li in ShoppingCartVM.ListCart)
            {
                li.Price = SD.GetPriceBasedOnQuantity(li.Count, li.Product.Price, li.Product.Price50, li.Product.Price100);

                ShoppingCartVM.OrderHeader.OrderTotal += li.Price * li.Count;
                li.Product.Description = SD.ConvertToRawHtml(li.Product.Description);

                if (li.Product.Description.Length > 16)
                    li.Product.Description = li.Product.Description.Substring(0, 15) + "...";
            }

            return View(ShoppingCartVM);
        }
    }
}
