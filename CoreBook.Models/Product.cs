using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace CoreBook.Models
{
    public class Product
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category Category { get; set; }

        [Required]
        public int CoverTypeID { get; set; }
        [ForeignKey("CoverTypeID")]
        public CoverType CoverType { get; set; }

        [Required]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        public string Authors { get; set; }

        [Required]
        [Range(1, 160)]
        public double ListPrice { get; set; }
        [Required]
        [Range(1, 160)]
        public double Price { get; set; }
        [Required]
        [Range(1, 160)]
        public double Price50 { get; set; }
        [Required]
        [Range(1, 160)]
        public double Price100 { get; set; }

        public string ImageUrl { get; set; }
    }
}
