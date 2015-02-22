using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.Models.ViewModels.Admin
{
    public class AdminUserViewModel
    {
        //Metadata
        public int UserID { get; set; }

        //Visible data
        [Display(Name = "Username")]        
        public string Username { get; set; }

        [Display(Name = "User Email")]     
        public string Email { get; set; }

        [Display(Name = "User Role")]     
        public int UserRole { get; set; }

        [Display(Name = "User Status")]     
        public bool? IsActive { get; set; }
    }
}