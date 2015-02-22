using CommerceIdeaBank.Models;
using CommerceIdeaBank.DatabaseInterface.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;
using CommerceIdeaBank.GlobalConstants;

//Included by author


namespace CommerceIdeaBank.Controllers
{
    //The visitors home controller
    public class HomeController : Controller
    {        
        //
        // GET: /Home/                 
        DefaultAccess default_access = new DefaultAccess();                

        public ActionResult Index()
        {                        
            return View( default_access.GetProjectList() );                       
        }


        public ActionResult ViewIdea(int? id)
        {            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            //Retrieve project view model
            ProjectViewModel proj = (ProjectViewModel)default_access.GetProject(id);

            //If project found is null
            if (proj == null)
            {
                return HttpNotFound();
            }
            
            return View(proj);
        }
        
        public ActionResult PermissionsInvalid()
        {
            //Output error message
            MessageBox.Show("Your permissions are invalid. We're going to have to log you " +
                "off. You're welcome to login again and continue afterwards, though!", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

            //Redirect to log out
            return RedirectToAction("Logout", "Account");
        }

        
    }
}
