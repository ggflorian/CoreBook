using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace CoreBook.Models
{
    public class Category
    {
        public int ID { get; set; }

        [Display(Name="Category Name")]
        [Required]
        [MaxLength(32)]
        public string Name { get; set; }
    }
}
