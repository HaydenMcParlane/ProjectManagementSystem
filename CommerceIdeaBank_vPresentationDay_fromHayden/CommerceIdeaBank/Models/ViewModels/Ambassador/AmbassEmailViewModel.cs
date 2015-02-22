using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.Models.ViewModels.Ambassador
{
    public class AmbassEmailViewModel
    {
        [Required]
        [Display(Name = "To: ")]
        public string SendTo { get; set; }

        [Display(Name = "CC: ")]
        public string CCTo { get; set; }

        [Required]
        [Display(Name = "Subject: ")]
        public string Subject { get; set; }
        
        [Required]
        public string EmailBody { get; set; }        

        /* [Display(Name = "Attached File(s): ")]
        public List<string> FilesToAttach { get; set; } */

        [Display(Name = "Priority: ")]
        public string Priority { get; set; }
       
        [Required]
        [Display(Name = "Sent From: ")]
        public string SentFrom { get; set; }        

        //Constructor
        public AmbassEmailViewModel()
        {
            //Defaults

        }
    }
}