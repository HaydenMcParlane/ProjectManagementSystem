using CommerceIdeaBank.Models;
using CommerceIdeaBank.DatabaseInterface.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CommerceIdeaBank.Models.ViewModels.Contributor;
using CommerceIdeaBank.GlobalConstants;
using System.Windows.Forms;
using System.IO;
using PagedList;

//Included by author


namespace CommerceIdeaBank.Controllers.Contributor
{
    //The visitors home controller
    public class ContributorHomeController : Controller
    {
        //
        // GET: /Home/                        
        ContributorAccess contributor_access = new ContributorAccess();

        public ActionResult Index( string sort_order = SortKey.ASCEND, string prev_sort_category = "", 
            string cur_sort_category = "", string current_filter = "", 
            int? page = 1, bool sort_flag = false ) 
        {            
            //User role security check
            int role = (int)HttpContext.Session["userRole"];                       

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {
                //Read in project list
                IEnumerable<ContributorProjectViewModel> contributor_project_list = 
                    (IEnumerable<ContributorProjectViewModel>)contributor_access.GetProjectList();                

                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;                           
                ViewBag.CurrentSortCategory = cur_sort_category;                
                ViewBag.CurrentSortOrder = sort_order;                 

                //Make sure sort is desired
                if (sort_flag == true && contributor_project_list != null)
                {
                    //Determine sort
                    switch (cur_sort_category)
                    {
                        case SortKey.PROJECT_USERNAME:
                            {
                                contributor_project_list = contributor_project_list.OrderBy(x => x.Username);                               

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_NAME:
                            {
                                contributor_project_list = contributor_project_list.OrderBy(x => x.ProjectName);                                

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_POST_DATE:
                            {
                                contributor_project_list = contributor_project_list.OrderBy(x => x.PostDate);                                

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
                            contributor_project_list = contributor_project_list.Reverse();

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

                if (contributor_project_list != null)
                {
                    return View(contributor_project_list.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    contributor_project_list = null;

                    return View(contributor_project_list);
                }
                                
            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_AMBASSADOR_HOME);
            }
            else 
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            }            
        }

        public ActionResult Dashboard()
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {
                return View();
            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_DASHBOARD, PermissionSpace.PS_ADMIN_HOME);
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_DASHBOARD, PermissionSpace.PS_AMBASSADOR_HOME);
            }
            else 
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            } 
        }

        public ActionResult MyContribution( string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false )
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {                

                IEnumerable<ContributorProjectViewModel> contributed_projects = 
                    (IEnumerable<ContributorProjectViewModel>)contributor_access.GetProjectList();

                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;

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
                    //Filter list for projects that were contributed by current user
                    contributed_projects = contributed_projects.Where(x => x.Username ==
                        System.Web.HttpContext.Current.Session["currentUser"].ToString()
                        ).AsEnumerable();


                    return View(contributed_projects.ToPagedList(pageNumber, pageSize));
                }
                else
                {                    
                    return View(contributed_projects);
                }                

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_MY_CONTRIBUTION, PermissionSpace.PS_ADMIN_HOME);
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_MY_CONTRIBUTION, PermissionSpace.PS_AMBASSADOR_HOME);
            }
            else
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            }
        }


        [HttpGet]
        public ActionResult ContributeIdea()
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {

                return View();

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_ADMIN_HOME);
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_AMBASSADOR_HOME);
            }
            else
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            }
        }


        [HttpPost]
        public ActionResult ContributeIdea(ContributorProjectViewModel idea)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {

                if (ModelState.IsValid)
                {
                    //If commit(idea) is successful it returns true
                    if (contributor_access.SubmitProject(idea))
                    {
                        return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_CONTRIBUTOR_HOME);
                    }
                    else
                    {
                        //Gracefully indicate that project submission was unsuccessful ***
                    }
                }

                return View(idea);

            }
            else if (role == UserRole.ADMIN)
            {
                //Output error message box
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error while processing your " + 
                "submission and, unfortunately, your idea will be lost. " +
                "Please re-submit your idea after we redirect you. " , Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Redirect to correct index
                return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_ADMIN_HOME);
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Output error message box
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error while processing your " +
                "submission and, unfortunately, your idea will be lost. " +
                "Please re-submit your idea after we redirect you. ", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Redirect to correct index
                return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_AMBASSADOR_HOME);
            }
            else
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            }
        }


        public ActionResult ViewIdea(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Retrieve project view model
                ContributorProjectViewModel proj = (ContributorProjectViewModel)contributor_access.GetProject(id);

                //If project found is null
                if (proj == null)
                {
                    return HttpNotFound();
                }

                return View(proj);

            }
            else if (role == UserRole.ADMIN)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_VIEW_IDEA, PermissionSpace.PS_ADMIN_HOME, 
                    new { id = id });
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_VIEW_IDEA, PermissionSpace.PS_AMBASSADOR_HOME,
                    new { id = id });
            }
            else
            {
                //Redirect to correct index
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            }
        }


        public ActionResult EditIdea(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                ContributorProjectViewModel proj = (ContributorProjectViewModel)contributor_access.GetProject(id);

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
            else if (role == UserRole.AMBASSADOR)
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_EDIT_IDEA, PermissionSpace.PS_AMBASSADOR_HOME,
                    new { id = id });
            }
            else
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditIdea([Bind(Include = "ProjectID,ProjectName,ProjectDesc,BusinessJustification,PostDate,Username,IsArchived")] ContributorProjectViewModel proj)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {

                if (ModelState.IsValid)
                {
                    //Edit the project
                    contributor_access.EditProject(proj);

                    return RedirectToAction("Index", "ContributorHome");
                }
                return View(proj);

            }
            else if (role == UserRole.ADMIN)
            {
                //Output error message box
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error while processing your " +
                "edit and, unfortunately, your edit will be lost. " +
                "Please re-submit your edit after we redirect you. ", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Redirect to correct
                return RedirectToAction(ActionName.AN_EDIT_IDEA, PermissionSpace.PS_ADMIN_HOME,
                    new { id = proj.ProjectID});
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Output error message box
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error while processing your " +
                "edit and, unfortunately, your edit will be lost. " +
                "Please re-submit your edit after we redirect you. ", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Redirect to correct
                return RedirectToAction(ActionName.AN_EDIT_IDEA, PermissionSpace.PS_AMBASSADOR_HOME,
                    new { id = proj.ProjectID });
            }
            else
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
            }
        }

        [HttpPost]
        public ActionResult AddFile( ContributorProjectViewModel cur_project, HttpPostedFileBase new_file )
        {            
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.CONTRIBUTOR)
            {
                //Function body
                foreach (string file_name in Request.Files)
                {
                    if ( (Request.Files[file_name] != null) &&
                                (Request.Files[file_name].ContentLength > 0) )
                    {
                        string path = AppDomain.CurrentDomain.BaseDirectory + "/uploads/";
                        string filename = Path.GetFileName(Request.Files[file_name].FileName);
                        Request.Files[file_name].SaveAs(Path.Combine(path, filename));

                        return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_CONTRIBUTOR_HOME);
                    }
                    else
                    {
                        // *** Handle issue gracefully
                        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                    }
                }

                return View();
            }
            else if (role == UserRole.ADMIN)
            {
                //Output error message box *** REWRITE ERROR MESSAGES ***
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error while processing your " +
                "edit and, unfortunately, your edit will be lost. " +
                "Please re-submit your edit after we redirect you. ", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Redirect to correct
                return RedirectToAction(ActionName.AN_ADD_FILE, PermissionSpace.PS_ADMIN_HOME);
            }
            else if (role == UserRole.AMBASSADOR)
            {
                //Output error message box
                MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                    "There was an error while processing your " +
                "edit and, unfortunately, your edit will be lost. " +
                "Please re-submit your edit after we redirect you. ", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                //Redirect to correct
                return RedirectToAction(ActionName.AN_ADD_FILE, PermissionSpace.PS_AMBASSADOR_HOME);
            }
            else
            {
                //Redirect to correct
                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_DEFAULT_HOME);
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
