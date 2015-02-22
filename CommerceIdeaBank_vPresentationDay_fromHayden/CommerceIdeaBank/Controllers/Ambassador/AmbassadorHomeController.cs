using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CommerceIdeaBank.Models;
using CommerceIdeaBank.DatabaseInterface.BusinessLogic;
using System.Net;
using CommerceIdeaBank.Models.ViewModels.Ambassador;
using System.Web.Helpers;
using System.Windows.Forms;
using CommerceIdeaBank.GlobalConstants;
using PagedList;

namespace CommerceIdeaBank.Controllers.Ambassador
{
    public class AmbassadorHomeController : Controller
    {
        // !!! AMBASSADOR PRIVILAGES !!!        
        AmbassadorAccess ambassador = new AmbassadorAccess();

        public ActionResult Index( string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false )
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is ambassador
            if (role == UserRole.AMBASSADOR)
            {                
                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;  

                //Read in project list
                IEnumerable<AmbassProjectViewModel> ambassador_project_list = 
                    ((IEnumerable<AmbassProjectViewModel>)ambassador.GetProjectList());                

                //Make sure sort is desired
                if (sort_flag == true && ambassador_project_list != null)
                {
                    //Determine sort
                    switch (cur_sort_category)
                    {
                        case SortKey.PROJECT_USERNAME:
                            {
                                ambassador_project_list = ambassador_project_list.OrderBy(x => x.Username);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_NAME:
                            {
                                ambassador_project_list = ambassador_project_list.OrderBy(x => x.ProjectName);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_POST_DATE:
                            {
                                ambassador_project_list = ambassador_project_list.OrderBy(x => x.PostDate);

                                //Leave switch stmt
                                break;
                            }
                        default:
                            {
                                //No ordering
                                break;
                            }
                    }

                    if (prev_sort_category == cur_sort_category)
                    {
                        if (sort_order == SortKey.DESCEND)
                        {
                            //Reverse list
                            ambassador_project_list = ambassador_project_list.Reverse();

                            //Toggle sort order
                            ViewBag.CurrentSortOrder = SortKey.ASCEND;
                        }
                        else
                        {
                            //List is ascending by default. Modify CurrentSortOrder
                            ViewBag.CurrentSortOrder = SortKey.DESCEND;
                        }
                    }
                    else if (sort_order == SortKey.ASCEND)
                    {
                        //If first sort is performed, sort key must be updated

                        //List is ascending by default. Modify CurrentSortOrder
                        ViewBag.CurrentSortOrder = SortKey.DESCEND;
                    }
                    else
                    {
                        ViewBag.CurrentSortOrder = SortKey.ASCEND;
                    }
                }

                //For paging ***
                int pageSize = PageStandard.PAGE_SIZE_PROJECTS;
                int pageNumber = (page ?? 1);

                if (ambassador_project_list != null)
                {
                    return View(ambassador_project_list.ToPagedList(pageNumber, pageSize));
                }
                else
                {                    
                    return View(ambassador_project_list);
                }

            }
            else if (role == UserRole.ADMIN)
            {                
                //Redirect to correct
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }

        public ActionResult Dashboard()
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {
                return View();
            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_DASHBOARD, PermissionSpace.PS_ADMIN_HOME);
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }

        public ActionResult MyContribution(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                IEnumerable<AmbassProjectViewModel> contributed_projects = (IEnumerable<AmbassProjectViewModel>)ambassador.GetProjectList();

                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;

                if (contributed_projects != null)
                {
                    //Filter list for projects that were contributed by current user
                    contributed_projects = contributed_projects.Where(x => x.Username ==
                        System.Web.HttpContext.Current.Session["currentUser"].ToString()
                        ).AsEnumerable();
                }              

                //Make sure sort is desired
                if (sort_flag == true && contributed_projects != null)
                {
                    //Determine sort
                    switch (cur_sort_category)
                    {
                        case SortKey.PROJECT_USERNAME:
                            {
                                contributed_projects = contributed_projects.OrderBy(x => x.Username);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_NAME:
                            {
                                contributed_projects = contributed_projects.OrderBy(x => x.ProjectName);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_POST_DATE:
                            {
                                contributed_projects = contributed_projects.OrderBy(x => x.PostDate);

                                //Leave switch stmt
                                break;
                            }
                        default:
                            {
                                //No ordering
                                break;
                            }
                    }

                    if (prev_sort_category == cur_sort_category)
                    {
                        if (sort_order == SortKey.DESCEND)
                        {
                            //Reverse list
                            contributed_projects = contributed_projects.Reverse();

                            //Toggle sort order
                            ViewBag.CurrentSortOrder = SortKey.ASCEND;
                        }
                        else
                        {
                            //List is ascending by default. Modify CurrentSortOrder
                            ViewBag.CurrentSortOrder = SortKey.DESCEND;
                        }
                    }
                    else if (sort_order == SortKey.ASCEND)
                    {
                        //If first sort is performed, sort key must be updated

                        //List is ascending by default. Modify CurrentSortOrder
                        ViewBag.CurrentSortOrder = SortKey.DESCEND;
                    }
                    else
                    {
                        ViewBag.CurrentSortOrder = SortKey.ASCEND;
                    }
                }


                //For paging
                int pageSize = PageStandard.PAGE_SIZE_PROJECTS;
                int pageNumber = (page ?? 1);

                if (contributed_projects != null)
                {
                    return View(contributed_projects.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(contributed_projects);
                }

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_MY_CONTRIBUTION, PermissionSpace.PS_ADMIN_HOME);
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }


        [HttpGet]
        public ActionResult ContributeIdea()
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {
                //Normal operation
                return View();

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_ADMIN_HOME);
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }


        [HttpPost]
        public ActionResult ContributeIdea(AmbassProjectViewModel idea)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                //Input checks
                if (idea == null) { /* do something *** */}            

                if (ModelState.IsValid)
                {
                    //If commit(idea) is successful it returns true
                    if (ambassador.SubmitProject(idea)) 
                    {
                        return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_AMBASSADOR_HOME);
                    }
                    else
                    {
                        //Gracefully indicate that project submission was unsuccessful ***
                    }
                }
                else
                {
                    //Gracefully indicate error ***
                }

                return View(idea);
            
            }
            else if (role == UserRole.ADMIN)
            {
                //Output error message box
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error in the sytem involving your access permissions. " +
                    "Unfortunately, if you've made a recent submission it'll likely be " +
                    "lost. We'll redirect you to the correct site location after you click " +
                    "OK.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Redirect to correct
                return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_ADMIN_HOME);
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }
        
        public ActionResult ViewIdea(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Retrieve project view model
                AmbassProjectViewModel proj = (AmbassProjectViewModel)ambassador.GetProject(id);

                //If project found is null
                if (proj == null)
                {
                    return HttpNotFound();
                }

                return View(proj);

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_VIEW_IDEA, PermissionSpace.PS_ADMIN_HOME,
                    new { id = id });
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }


        public ActionResult EditIdea(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                AmbassProjectViewModel proj = (AmbassProjectViewModel)ambassador.GetProject(id);
                if (proj == null)
                {
                    return HttpNotFound();
                }

                return View(proj);

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_EDIT_IDEA, PermissionSpace.PS_ADMIN_HOME,
                    new { id = id });
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIdea([Bind(Include = "ProjectID,ProjectName,ProjectDesc,BusinessJustification,Status,PostDate,Username,IsArchived")] AmbassProjectViewModel proj)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                if (ModelState.IsValid)
                {
                    //Edit the project
                    ambassador.EditProject(proj);

                    return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_AMBASSADOR_HOME);
                }
                return View(proj);

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_EDIT_IDEA, PermissionSpace.PS_ADMIN_HOME,
                    new { id = proj.ProjectID });
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }


        public ActionResult ViewSchool(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                //Input check
                if (id == null || id < 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); } //*** Should really output message box and redirect user


                AmbassSchoolViewModel vm_school = (AmbassSchoolViewModel)ambassador.GetSchool(id);
                if (vm_school == null) { return HttpNotFound(); }

                return View(vm_school);

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_VIEW_SCHOOL, PermissionSpace.PS_ADMIN_HOME,
                    new { id = id });
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }

        public ActionResult ViewActiveProjects(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is ambassador
            if (role == UserRole.AMBASSADOR)
            {                             
                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;  

                //Retrieve all active projects in view model form
                try
                {
                    IEnumerable<AmbassActiveProjectViewModel> active_project_list =
                        ambassador.GetAllActiveProjects().Cast<AmbassActiveProjectViewModel>();

                    //Make sure sort is desired
                    if (sort_flag == true && active_project_list != null)
                    {
                        //Determine sort
                        switch (cur_sort_category)
                        {
                            case SortKey.PROJECT_NAME:
                                {
                                    active_project_list = active_project_list.OrderBy(x => x.ProjectName);

                                    //Leave switch stmt
                                    break;
                                }
                            case SortKey.SCHOOL_NAME:
                                {
                                    active_project_list = active_project_list.OrderBy(x => x.SchoolName);

                                    //Leave switch stmt
                                    break;
                                }
                            case SortKey.PROJECT_STATUS:
                                {
                                    active_project_list = active_project_list.OrderBy(x => x.ProgressStatus);

                                    //Leave switch stmt
                                    break;
                                }
                            case SortKey.PROJECT_USERNAME:
                                {
                                    active_project_list = active_project_list.OrderBy(x => x.Username);

                                    //Leave switch stmt
                                    break;
                                }
                            case SortKey.EMAIL:
                                {
                                    active_project_list = active_project_list.OrderBy(x => x.Email);

                                    //Leave switch stmt
                                    break;
                                }
                            default:
                                {
                                    //No ordering
                                    break;
                                }
                        }

                        if (prev_sort_category == cur_sort_category)
                        {
                            if (sort_order == SortKey.DESCEND)
                            {
                                //Reverse list
                                active_project_list = active_project_list.Reverse();

                                //Toggle sort order
                                ViewBag.CurrentSortOrder = SortKey.ASCEND;
                            }
                            else
                            {
                                //List is ascending by default. Modify CurrentSortOrder
                                ViewBag.CurrentSortOrder = SortKey.DESCEND;
                            }
                        }
                        else if (sort_order == SortKey.ASCEND)
                        {
                            //If first sort is performed, sort key must be updated

                            //List is ascending by default. Modify CurrentSortOrder
                            ViewBag.CurrentSortOrder = SortKey.DESCEND;
                        }
                        else
                        {
                            ViewBag.CurrentSortOrder = SortKey.ASCEND;
                        }
                    }

                    //For paging
                    int pageSize = PageStandard.PAGE_SIZE_PROJECTS;
                    int pageNumber = (page ?? 1);

                    if (active_project_list != null)
                    {
                        //Send view models to view
                        return View(active_project_list.ToPagedList(pageNumber, pageSize));
                    }
                    else
                    {
                        return View(active_project_list);
                    }

                }
                catch
                {
                    //Gracefully indicate error
                }
                return View();

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }

        [HttpPost, ActionName("AmbassUpdateProjectStatus")]
        public ActionResult UpdateProjectStatus(int? assignment_id, string new_project_status)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                //Input checks and constraints
                if (assignment_id == null || assignment_id < 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                if (new_project_status == null || new_project_status == "") { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }


                try
                {
                    ambassador.UpdateActiveStatusToInProg(assignment_id);

                    //Output message box
                    MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK);

                    return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_AMBASSADOR_HOME);
                }
                catch
                {
                    //Gracefully indicate error ***
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

            }
            else if (role == UserRole.ADMIN)
            {

                //Redirect to correct
                return RedirectToAction(ActionName.AN_UPDATE_PROJECT_STATUS, PermissionSpace.PS_ADMIN_HOME,
                    new { assignment_id = assignment_id, new_project_status = new_project_status });
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }


        [HttpGet]
        public ActionResult ConstructEmail(string user_email)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                //Input checks
                if (user_email == null || user_email == "") 
                { 
                    return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_AMBASSADOR_HOME); 
                }

                AmbassEmailViewModel email = new AmbassEmailViewModel();

                //Assign user email to necessary email view model field
                email.SendTo = user_email;

                return View(email);

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_CONSTRUCT_EMAIL, PermissionSpace.PS_ADMIN_HOME,
                    new { user_email = user_email });
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }


        [ActionName("ReconstructEmail")]
        public ActionResult ReconstructEmail(AmbassEmailViewModel user_email)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                //Input checks
                if (user_email == null)
                {
                    return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_AMBASSADOR_HOME);
                }

                AmbassEmailViewModel email = new AmbassEmailViewModel();

                //Assign user_email fields to necessary email view model field
                email.SendTo = user_email.SendTo;
                email.SentFrom = user_email.SentFrom;
                email.Subject = user_email.Subject;
                email.Priority = user_email.Priority;
                email.EmailBody = user_email.EmailBody;
                email.CCTo = user_email.CCTo;                

                return View(email);

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_CONSTRUCT_EMAIL, PermissionSpace.PS_ADMIN_HOME,
                    new { user_email = user_email.SendTo });
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
        }

        [HttpPost, ActionName("CreateEmail")]
        public ActionResult ConstructEmail(AmbassEmailViewModel user_email)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.AMBASSADOR)
            {

                //Input checks
                if (user_email == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                if (ModelState.IsValid)
                {
                    //Assign user email to necessary email view model field            
                    if (ambassador.SendEmail(user_email))
                    {
                        //Email was sent.
                        //Output confirmation message for user ***
                        MessageBox.Show("Your email is on its way!", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        //Return to original view
                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_AMBASSADOR_HOME);
                    }
                    else
                    {
                        //Gracefully indicate error                
                        MessageBox.Show("Oh no! An error occurred while sending your email. " +
                            "We'll return you to email construction and reset the values with " +
                            "your entries.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                             MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        //Redirect to reconstruct email *** won't actually re-route value correctly
                        return RedirectToAction(ActionName.AN_RECONSTRUCT_EMAIL, PermissionSpace.PS_AMBASSADOR_HOME,
                        new { Value = (AmbassEmailViewModel)user_email });
                    }
                }

                return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_AMBASSADOR_HOME);

            }
            else if (role == UserRole.ADMIN)
            {
                //Output error message box
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error in the sytem with your permissions which will " +
                    "require that you re-write your previous email. We'll redirect you " +
                    "to the correct email page.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Delete email and redirect to recovery point
                return RedirectToAction(ActionName.AN_CONSTRUCT_EMAIL, PermissionSpace.PS_ADMIN_HOME,
                    new { user_email = user_email.SendTo});
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_AMBASSADOR_HOME);
            }
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
