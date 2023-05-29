using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PROG7311_PART2.Models
{
    public class Farmer
    {
        [Key]
        public int FarmerID { get; set; }
        [Required]
        public string UserID { get; set; }
        [Required]
        [Display(Name = "Name")]
        public string FarName { get; set; }
        [Required]
        [Display(Name = "Surname")]
        public string FarSurname { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string FarAdress { get; set; }
    }

    public class CreateFarmerViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class FarmerViewModel
    {
        public int FarmerID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
    }

    public class FarmerDetailModel
    {
        public int FarmerID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        [Display(Name = "Name")]
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Address { get; set; }
    }

    public class FarmerUsers
    {
        public string UserID { get; set; }
        public int FarmerID { get; set; }
        public string Username { get; set; }
    }


}