using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.Models.ViewModels.Ambassador
{
    public class AmbassProjectViewModel
    {        
        public int ProjectID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Project Name: ")]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Project Description ")]
        public string ProjectDesc { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Business Justification: ")]
        public string BusinessJustification { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Username ")]
        public string Username { get; set; }
        
        [StringLength(50)]
        [Display(Name = "Contributor Email ")]
        public string Email { get; set; } //For displaying email to admin

        [Required]
        [Display(Name = "Date Posted ")]
        public DateTime PostDate { get; set; }

        [Required]        
        [Display(Name = "Status ")]
        public string Status { get; set; }

        [Required]
        [Display(Name = "Archived ")]
        public bool IsArchived { get; set; }           

    }
}