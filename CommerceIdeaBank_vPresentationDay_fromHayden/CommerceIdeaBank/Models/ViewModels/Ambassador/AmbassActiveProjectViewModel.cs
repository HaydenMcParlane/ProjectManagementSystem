using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.Models.ViewModels.Ambassador
{
    public class AmbassActiveProjectViewModel
    {

        public int ProjectAssignID { get; set; }
        public int ProjectID { get; set; }
        public int SchoolID { get; set; }
        public bool IsArchived { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "School Name")]
        public string SchoolName { get; set; }

        [Display(Name = "Assignment Status")]
        public string ProgressStatus { get; set; }

        [Display(Name = "Ambassador Username")]
        public string Username { get; set; }

        [Display(Name = "Ambassador Email")]
        public string Email { get; set; }

        [Display(Name = "Project Notes")]
        public string Notes { get; set; }

        //Constructors
        public AmbassActiveProjectViewModel()
        {

        }
    }
}