using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.Models.ViewModels.Admin
{
    //View model for add schools
    public class AdminSchoolViewModel
    {
        public int SchoolID { get; set; }

        [Display(Name = "School Name")]
        [Required]
        public string SchoolName { get; set; }

        [Display(Name = "School Phone")]
        public int Phone { get; set; }

        [Display(Name = "School Email")]
        public string Email { get; set; }

        [Display(Name = "Ambassador Username")]
        public string Username { get; set; }

        //Address
        public decimal StreetNumber { get; set; }
        public string StreetName { get; set; }
        public decimal ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}