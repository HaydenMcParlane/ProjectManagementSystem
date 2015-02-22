using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.Models.ViewModels.Ambassador
{
    //View model for add schools
    public class AmbassSchoolViewModel
    {
        public int SchoolID { get; set; }

        [Display(Name = "School Name")]
        [Required]
        public string SchoolName { get; set; }

        [Display(Name = "School Phone Num")]
        public int Phone { get; set; }

        [Display(Name = "School Email")]
        public string Email { get; set; }

        
        public decimal StreetNumber { get; set; }
        public string StreetName { get; set; }
        public decimal ZipCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}