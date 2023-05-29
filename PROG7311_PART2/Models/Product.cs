using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PROG7311_PART2.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        [Required]
        public int FarmerID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string PName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string PDescription { get; set; }
        [Required]
        public int PTypeID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
        [Required]
        public int Quantity { get; set; }
    }


    public class CreateProductViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Date Added")]
        [DataType(DataType.Date)]
        public DateTime DateAdded { get; set; }
        [Required]
        [Display(Name = "Product Type")]
        public int TypeID { get; set; }
        [Required]
        public int Quantity { get; set; }
    }

    public class ProductDetailsModel
    {
        public int ProductID { get; set; }
        [Display(Name = "Farmer Username")]
        public string Username { get; set; }
        [Display(Name = "Name")]
        public string PName { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }
        public int Quantity { get; set; }
    }

    public class EmpProductIndexModel
    {
        public int ProductID { get; set; }
        [Display(Name = "Name")]
        public string PName { get; set; }
        public string Type { get; set; }
        [Display(Name = "Date Added")]
        public DateTime DateAdded { get; set; }
        public int Quantity { get; set; }

    }
}