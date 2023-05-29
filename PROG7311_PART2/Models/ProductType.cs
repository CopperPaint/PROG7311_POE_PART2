using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PROG7311_PART2.Models
{
    public class ProductType
    {
        [Key]
        public int PTypeID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string PTName { get; set; }
        [Required]
        [Display(Name = "Description")]
        public string PTDescription { get; set; }
        [Required]
        [Display(Name = "Cateogry")]
        public string PTCategory { get; set; }
    }
}