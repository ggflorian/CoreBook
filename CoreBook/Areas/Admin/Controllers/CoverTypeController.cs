using CoreBook.DataAccess.Repository.IRepository;
using CoreBook.Models;
using CoreBook.Utility;
using Dapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? id)
        {
            CoverType coverType = new CoverType();

            if (id == null)
            {
                // this is for create
                return View(coverType);
            }

            // this is for update
            var prm = new DynamicParameters();
            prm.Add("@ID", id);
            coverType = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, prm);
            if (coverType == null)
                return NotFound();

            return View(coverType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(CoverType coverType)
        {
            if (ModelState.IsValid)
            {
                var prm = new DynamicParameters();
                prm.Add("@Name", coverType.Name);

                if (coverType.ID == 0)
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Create, prm);
                else
                {
                    prm.Add("@Id", coverType.ID);
                    _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Update, prm);
                }

                //_unitOfWork.Save();
                return RedirectToAction(nameof(Index)); // don't like magic strings "Index"
            }
            return View(coverType);
        }


        // WEB METHODS - API CALLS
        #region API CALLS

        [HttpGet]
        public IActionResult GetAll()
        {
            var allObj = _unitOfWork.SP_Call.List<CoverType>(SD.Proc_CoverType_GetAll);
            return Json(new { data = allObj });
        }

        [HttpDelete]
        //[ValidateAntiForgeryToken] // FG WHY: without form goes on bhrugen example
        public IActionResult Delete(int id)
        {
            var prm = new DynamicParameters();
            prm.Add("@Id", id);

            var objFromDb = _unitOfWork.SP_Call.OneRecord<CoverType>(SD.Proc_CoverType_Get, prm);
            if (objFromDb == null)            
                return Json(new { succesuri = false, mesaj = "Error while deleting" });

            _unitOfWork.SP_Call.Execute(SD.Proc_CoverType_Delete, prm);
            //_unitOfWork.Save();
            return Json(new { succesuri = true, mesaj = "Delete succesful!" });
        }

        #endregion
    }
}
