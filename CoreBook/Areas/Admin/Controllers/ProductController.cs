using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;
using CoreBook.Models.ViewModels;
using CoreBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostEnvironment; // for uploading images on server in wwwroot folder

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                ProductObj = new Product(),

                CategoryList = _unitOfWork.Category.GetAll().Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.ID.ToString()
                }),

                CoverTypeList = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll).Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.ID.ToString()
                })
            };

            if (id == null)
            {
                // this is for create
                return View(productVM);
            }

            // this is for update
            productVM.ProductObj = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.ProductObj == null)
                return NotFound();

            return View(productVM);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath; // absolute path of wwwroot
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(webRootPath, @"images\products"); // upload path
                    var extension = Path.GetExtension(files[0].FileName);

                    if (productVM.ProductObj.ImageUrl != null)
                    {
                        // this is an edit and we need to delete old image
                        string imagePath = Path.Combine(webRootPath, productVM.ProductObj.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                            System.IO.File.Delete(imagePath);
                    }

                    using (var filesStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }

                    productVM.ProductObj.ImageUrl = @"\images\products\" + fileName + extension;
                }
                else
                {
                    // update when they don't change the image
                    if (productVM.ProductObj.ID != 0)
                    {
                        Product objFromDb = _unitOfWork.Product.Get(productVM.ProductObj.ID);
                        productVM.ProductObj.ImageUrl = objFromDb.ImageUrl;
                    }
                }

                if (productVM.ProductObj.ID == 0) _unitOfWork.Product.Add(productVM.ProductObj);
                else _unitOfWork.Product.Update(productVM.ProductObj);

                _unitOfWork.Save();
                return RedirectToAction(nameof(Index)); // don't like magic strings "Index"
            }
            else
            {
                // if clientValidation not working, ModelState is invalid and productVM has fields null => error => reload fields
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.ID.ToString()
                });

                productVM.CoverTypeList = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll).Select(o => new SelectListItem
                {
                    Text = o.Name,
                    Value = o.ID.ToString()
                });

                if (productVM.ProductObj.ID != 0)
                    productVM.ProductObj = _unitOfWork.Product.Get(productVM.ProductObj.ID);
            }

            return View(productVM);
        }
        


        // WEB METHODS - API CALLS
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.Product.GetAll(includeProperties:"Category,CoverType");
            return Json(new { data = allObj });
        }

        [HttpDelete]
        //[ValidateAntiForgeryToken] // FG WHY: without form goes on bhrugen example
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Product.Get(id);
            if (objFromDb == null)            
                return Json(new { succesuri = false, mesaj = "Error while deleting" });

            // delete image from server
            string webRootPath = _hostEnvironment.WebRootPath; // absolute path of wwwroot
            string imagePath = Path.Combine(webRootPath, objFromDb.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            // remove obj from db
            _unitOfWork.Product.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new { succesuri = true, mesaj = "Delete succesful!" });
        }

        #endregion
    }
}
