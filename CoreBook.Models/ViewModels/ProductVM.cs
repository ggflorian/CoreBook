using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.AspNetCore.Mvc.Rendering; // install MVC.ViewFeatures


namespace CoreBook.Models.ViewModels
{
    public class ProductVM
    {
        public Product ProductObj { get; set; }

        public IEnumerable<SelectListItem> CategoryList { get; set; }
        public IEnumerable<SelectListItem> CoverTypeList { get; set; }
    }
}
