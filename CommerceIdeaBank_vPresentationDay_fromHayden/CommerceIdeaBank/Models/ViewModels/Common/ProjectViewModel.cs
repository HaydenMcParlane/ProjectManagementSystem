using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using CommerceIdeaBank.Models;

namespace CommerceIdeaBank.Models
{
    public class ProjectViewModel
    {
        public int ProjectID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Project Name: ")]
        public string ProjectName { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Project Description: ")]
        public string ProjectDesc { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Business Justification: ")]
        public string BusinessJustification { get; set; }

        public string Username { get; set; }

        public DateTime PostDate { get; set; }

        public bool IsArchived { get; set; } 

    }
}