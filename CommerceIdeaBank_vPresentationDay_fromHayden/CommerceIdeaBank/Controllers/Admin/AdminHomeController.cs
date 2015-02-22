using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using CommerceIdeaBank.Models;
using CommerceIdeaBank.DatabaseInterface.BusinessLogic;
using System.Net;
using CommerceIdeaBank.Models.ViewModels.Admin;
using System.Web.Helpers;
using System.Windows.Forms;
using CommerceIdeaBank.GlobalConstants;
using PagedList;

namespace CommerceIdeaBank.Controllers.Admin
{
    public class AdminHomeController : Controller
    {
        // !!! ADMINISTRATIVE PRIVILAGES !!!        
        AdminAccess admin = new AdminAccess();

        public ActionResult Index(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {
                //Gather idea list and eliminate archived ideas
                IEnumerable<AdminProjectViewModel> admin_project_list = 
                    (IEnumerable<AdminProjectViewModel>)admin.GetProjectList();
                
                if(admin_project_list != null )
                {
                    admin_project_list = 
                        admin_project_list.Cast<AdminProjectViewModel>().Where(x => x.IsArchived == false);
                }
                else
                {
                    return View(admin_project_list);
                }                

                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;

                //Make sure sort is desired
                if (sort_flag == true && admin_project_list != null)
                {
                    //Determine sort
                    switch (cur_sort_category)
                    {
                        case SortKey.PROJECT_USERNAME:
                            {
                                admin_project_list = admin_project_list.OrderBy(x => x.Username);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_NAME:
                            {
                                admin_project_list = admin_project_list.OrderBy(x => x.ProjectName);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_POST_DATE:
                            {
                                admin_project_list = admin_project_list.OrderBy(x => x.PostDate);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_STATUS:
                            {
                                admin_project_list = admin_project_list.OrderBy(x => x.Status);

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
                            admin_project_list = admin_project_list.Reverse();

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

                if (admin_project_list != null)
                {
                    //Return project list without archived projects
                    return View(admin_project_list.ToPagedList(pageNumber, pageSize));
                }
                else 
                {
                    return View(admin_project_list);
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        public ActionResult Dashboard()
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.ADMIN)
            {
                return View();
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        public ActionResult MyContribution(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Gather idea list
                IEnumerable<AdminProjectViewModel> contributed_projects = 
                    (IEnumerable<AdminProjectViewModel>)admin.GetProjectList();
                
                if (contributed_projects != null) 
                {
                    contributed_projects = contributed_projects.Cast<AdminProjectViewModel>();

                    //Filter list for projects that were contributed by current user
                    //and eliminate archived ideas
                    contributed_projects = contributed_projects.Where(x => x.Username ==
                        System.Web.HttpContext.Current.Session["currentUser"].ToString()
                        ).Where(x => x.IsArchived == false).AsEnumerable();
                }                

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
                        case SortKey.PROJECT_STATUS:
                            {
                                contributed_projects = contributed_projects.OrderBy(x => x.Status);

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
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        public ActionResult ArchivedIdeas(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                IEnumerable<AdminProjectViewModel> archived_projects = 
                    (IEnumerable<AdminProjectViewModel>)admin.GetProjectList();

                if (archived_projects != null)
                {
                    //Filter list for projects that are archived
                    archived_projects = archived_projects.ToList().Where(x => x.IsArchived
                        == true).AsEnumerable();
                }

                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;                

                //Make sure sort is desired
                if (sort_flag == true && archived_projects != null)
                {
                    //Determine sort
                    switch (cur_sort_category)
                    {
                        case SortKey.PROJECT_USERNAME:
                            {
                                archived_projects = archived_projects.OrderBy(x => x.Username);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_NAME:
                            {
                                archived_projects = archived_projects.OrderBy(x => x.ProjectName);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_POST_DATE:
                            {
                                archived_projects = archived_projects.OrderBy(x => x.PostDate);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_STATUS:
                            {
                                archived_projects = archived_projects.OrderBy(x => x.Status);

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
                            archived_projects = archived_projects.Reverse();

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

                if (archived_projects != null)
                {
                    return View(archived_projects.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(archived_projects);
                }                

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        [HttpGet]
        public ActionResult ContributeIdea()
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                return View();

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        [HttpPost]
        public ActionResult ContributeIdea(AdminProjectViewModel idea)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if (ModelState.IsValid)
                {                   
                    //If commit(idea) is successful it returns true
                    if ( admin.SubmitProject(idea) )
                    {
                        //Output error message box
                        MessageBox.Show("Project submission successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                        MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);
                    }
                    else
                    {
                        //Output error message box
                        MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                            "An error occurred during project submission. " +
                        "We'll redirect you back to \'Submit Idea\'.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        //Redirect to correct *** Repopulate fields!
                        return RedirectToAction(ActionName.AN_CONTRIBUTE_IDEA, PermissionSpace.PS_ADMIN_HOME);
                    }                 
                }            
                         
                return View(idea);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpGet]
        public ActionResult SelectSchool(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {
                //Input validation
                if (id == null || id < 0) 
                {
                    //Output error message box
                    MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                        "An error occurred while selecting the school. We'll redirect you to " +
                        "\'View All Schools\' to establish a fresh state.",
                        Popups.POP_UP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, 
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                    //Redirect to correct
                    return RedirectToAction( ActionName.AN_VIEW_ALL_SCHOOLS, PermissionSpace.PS_DEFAULT_HOME);
                }

                TempData["ProjectID"] = id;
           
                IEnumerable<AdminSchoolViewModel> school_list;
                IEnumerable<object> temp;

                try
                {
                    temp = admin.GetAllSchools();

                    school_list = temp.Cast<AdminSchoolViewModel>();

                    return View(school_list);
                }
                catch
                {
                    //Output error message box
                    MessageBox.Show(" :-( We're extremely sorry about the inconvenience! " +
                        "An error occurred while selecting the school. We'll redirect you to " +
                        "\'View All Schools\' to establish a fresh state.",
                        Popups.POP_UP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                    //Redirect to correct
                    return RedirectToAction(ActionName.AN_LOGOUT, PermissionSpace.PS_DEFAULT_HOME);
                }                
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }
        
        [HttpPost, ActionName("FinalizeAssignment")]
        public ActionResult SelectSchool()
        {            
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                int school_id = Convert.ToInt32(Request.Form["SchoolID"]);
                int project_id = (int)TempData["ProjectID"];                

                //If either id < 0, deal with error
                if ( (school_id < 0) || (project_id < 0) )
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }                

                admin.AssignProjectToSchool(project_id, school_id); 

                return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        [HttpGet]
        public ActionResult ViewAllUsers(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {                
                //Retrieve list of users
                IEnumerable<object> users_as_object = admin.GetAllUsers();

                IEnumerable<AdminUserViewModel> users;

                if (users_as_object != null)
                {
                    users = users_as_object.ToList().Cast<AdminUserViewModel>().AsEnumerable();
                }
                else
                {
                    users = null;
                }                

                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;

                //Make sure sort is desired
                if (sort_flag == true && users != null)
                {
                    //Determine sort
                    switch (cur_sort_category)
                    {
                        case SortKey.PROJECT_USERNAME:
                            {
                                users = users.OrderBy(x => x.Username);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.EMAIL:
                            {
                                users = users.OrderBy(x => x.Email);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.USER_ROLE:
                            {
                                users = users.OrderBy(x => x.UserRole);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.USER_STATUS:
                            {
                                users = users.OrderBy(x => x.IsActive);

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
                            users = users.Reverse();

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
                int pageSize = PageStandard.PAGE_SIZE_USERS;
                int pageNumber = (page ?? 1);

                if (users != null)
                {
                    return View(users.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(users);
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpPost , ActionName("AdminViewAllUsers")]
        public ActionResult ViewAllUsers(string selected_username, int? new_user_role)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Input validation
                if (selected_username == null || selected_username == "") { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                if (new_user_role < 0 || new_user_role == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                //Perform update
                if (admin.UpdateUserRole(selected_username, new_user_role))
                {
                    //If update successful

                    //Output necessary message to user
                    MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK );

                    return RedirectToAction(ActionName.AN_VIEW_ALL_USERS, PermissionSpace.PS_ADMIN_HOME);
                }
                else
                {
                    //Output error message box
                    MessageBox.Show(" :-( An error seems to have occurred while " +
                    "updating the selected user's role. We apologize about any " +
                    "inconvenience! We're going to redirect you to \'View All Users\'.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                    return RedirectToAction(ActionName.AN_VIEW_ALL_USERS, PermissionSpace.PS_ADMIN_HOME);
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        [HttpGet]
        public ActionResult CreateSchool()
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Create a school            
                return View();

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpPost]
        public ActionResult CreateSchool(AdminSchoolViewModel new_school)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Add the school
                if (admin.SubmitSchool(new_school))
                {
                    return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);
                }
                else
                {
                    //Gracefully indicate error status
                    //Output error message box
                    MessageBox.Show(" :-( An error seems to have occurred while " +
                    "submitting the school. We apologize about any " +
                    "inconvenience! We're going to redirect you to \'Create School\'.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                    MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                    // *** Repopulate values
                    return RedirectToAction(ActionName.AN_CREATE_SCHOOL, PermissionSpace.PS_ADMIN_HOME);

                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpGet]
        public ActionResult ViewAllSchools(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {
                IEnumerable<object> temp = admin.GetAllSchools();
                IEnumerable<AdminSchoolViewModel> vm_schools;

                if (temp != null)
                {
                    vm_schools = 
                        (IEnumerable<AdminSchoolViewModel>)temp.ToList().Cast<AdminSchoolViewModel>().AsEnumerable();
                }
                else
                {
                    vm_schools = null;
                }                         

                //For paging
                ViewBag.PreviousSortCategory = prev_sort_category;
                ViewBag.CurrentSortCategory = cur_sort_category;
                ViewBag.CurrentSortOrder = sort_order;

                //Make sure sort is desired
                if (sort_flag == true && vm_schools != null)
                {
                    //Determine sort
                    switch (cur_sort_category)
                    {
                        case SortKey.SCHOOL_NAME:
                            {
                                vm_schools = vm_schools.OrderBy(x => x.SchoolName);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.PROJECT_USERNAME:
                            {
                                vm_schools = vm_schools.OrderBy(x => x.Username);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.EMAIL:
                            {
                                vm_schools = vm_schools.OrderBy(x => x.Email);

                                //Leave switch stmt
                                break;
                            }
                        case SortKey.SCHOOL_PHONE:
                            {
                                vm_schools = vm_schools.OrderBy(x => x.Phone);

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
                            vm_schools = vm_schools.Reverse();

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
                int pageSize = PageStandard.PAGE_SIZE_SCHOOLS;
                int pageNumber = (page ?? 1);

                if (vm_schools != null)
                {
                    return View(vm_schools.ToPagedList(pageNumber, pageSize));
                }
                else
                {
                    return View(vm_schools);
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpPost, ActionName("SelectAmbassador")]
        public ActionResult SelectAmbassador(int? selected_school_id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {
                //Information validation ***

                //Input validation
                if (selected_school_id == null || selected_school_id < 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            
                //Store selected school id
                TempData["selected_school_id"] = selected_school_id;

                //Read in all users
                IEnumerable<object> user_object_enum = admin.GetAllUsers();

                //Cast objects to users
                IEnumerable<AdminUserViewModel> user_enum = user_object_enum.Cast<AdminUserViewModel>();

                //Isolate only ambassadors
                user_enum = user_enum.Where(x => x.UserRole == UserRole.AMBASSADOR);

                //Pass users into view
                return View(user_enum);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
       }

        [HttpPost, ActionName("AdminAddAmbassadorToSchool")]
        public ActionResult ViewAllSchools(string new_ambassador_username)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Input validation            
                if (new_ambassador_username == null || new_ambassador_username == "") { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
            
                //Input cached result
                int selected_school_id = (int)TempData["selected_school_id"];
            
                //Input validation
                if (selected_school_id < 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                //Add ambassador to school
                if (admin.AddAmbassToSchool(selected_school_id, new_ambassador_username))
                {
                    //If addition of ambassador successful
                    MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                         MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                    //Reload original view
                    return RedirectToAction(ActionName.AN_VIEW_ALL_SCHOOLS, PermissionSpace.PS_ADMIN_HOME);
                }
                else
                {
                    //Error has occurred
                    MessageBox.Show(" :-( Oh no! An error occurred while adding the ambassador " +
                        "to the school. Please try this operation again later.", Popups.POP_UP_TITLE,
                        MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0x40000);

                    //reload original view
                    return RedirectToAction(ActionName.AN_VIEW_ALL_SCHOOLS, PermissionSpace.PS_ADMIN_HOME);
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpPost, ActionName("AdminRemoveAmbassador")]
        public ActionResult AdminRemoveAmbassador(int? selected_school_id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Input checks
                if (selected_school_id == null || selected_school_id < 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                //Remove Ambassador from school
                if (admin.RemoveAmbassFromSchool(selected_school_id))
                {
                    //If removal successful
                    return RedirectToAction(ActionName.AN_VIEW_ALL_SCHOOLS, PermissionSpace.PS_ADMIN_HOME);
                }
                else 
                {
                    // *** Gracefully deal with error
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest); 
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [ActionName("ViewIdea")]
        public ActionResult ViewIdea(int? id)
        {          
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            
                //Retrieve project view model
                AdminProjectViewModel proj = (AdminProjectViewModel)admin.GetProject(id);

                //If project found is null
                if (proj == null)
                {
                    return HttpNotFound();
                }
            
                return View(proj);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        public ActionResult EditIdea(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Read project in
                AdminProjectViewModel proj = (AdminProjectViewModel)admin.GetProject(id);

                if (proj == null)
                {
                    return HttpNotFound();
                }

                return View(proj);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public ActionResult EditIdea([Bind(Include = "ProjectID,ProjectName,ProjectDesc,BusinessJustification,Status,PostDate,Username,IsArchived")] AdminProjectViewModel proj)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {
                
                if (ModelState.IsValid)
                {
                    //Edit the project
                    admin.EditProject(proj);

                    return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);
                }

                return View(proj);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        } 


        public ActionResult DeleteIdea(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Retrieve project view model
                AdminProjectViewModel proj = (AdminProjectViewModel)admin.GetProject(id);

                if (proj == null)
                {
                    return HttpNotFound();
                }

                return View(proj);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }
 

        [HttpPost, ActionName("DeleteIdea")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if( admin.DeleteProject(id) )
                {
                    return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);
                }
                else
                {
                    //Gracefully handle deletion error ***
                    return RedirectToAction("Index", "AdminHome");
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        public ActionResult ArchiveIdea(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                AdminProjectViewModel proj = (AdminProjectViewModel)admin.GetProject(id);

                if (proj == null)
                {
                    return HttpNotFound();
                }

                return View(proj);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveIdea([Bind(Include = "ProjectID,ProjectName,ProjectDesc,BusinessJustification,Status,PostDate,Username,IsArchived")] AdminProjectViewModel proj)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                admin.ArchiveProject(proj.ProjectID);
                return RedirectToAction(ActionName.AN_VIEW_ARCHIVED_IDEAS, "AdminHome");

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        public ActionResult ViewSchool(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Input check
                if (id == null || id < 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); } 

            
                AdminSchoolViewModel vm_school = (AdminSchoolViewModel)admin.GetSchool(id);
                if (vm_school == null) { return HttpNotFound(); } 
            
                return View(vm_school);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        public ActionResult EditSchool(int? id)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if (id == null || id < 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                //Read school in
                AdminSchoolViewModel school = (AdminSchoolViewModel)admin.GetSchool(id);

                if (school == null)
                {
                    return HttpNotFound();
                }

                return View(school);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSchool([Bind(Include = "SchoolID,SchoolName,Phone,Email,Username,ContactName, ContactPhone, ContactEmail,Department,Class,StreetNumber,StreetName,ZipCode, City, State")] AdminSchoolViewModel school)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                if (ModelState.IsValid)
                {
                    //Edit the project
                    if (admin.EditSchool(school))
                    {
                        //If edit successful, do following
                        return RedirectToAction(ActionName.AN_VIEW_SCHOOL, PermissionSpace.PS_ADMIN_HOME,
                        new { id = school.SchoolID });
                    }
                    else
                    {
                        //Output error message box
                        MessageBox.Show(" :-( There was an error that occurred during submission of " +
                            "your school edit. We'll redirect you to the IdeaBank.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                        MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        //Redirect to correct
                        return RedirectToAction(ActionName.AN_INDEX, PermissionSpace.PS_ADMIN_HOME);
                    }
                }

                return View(school);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        public ActionResult ViewActiveProjects(string sort_order = SortKey.ASCEND, string prev_sort_category = "",
            string cur_sort_category = "", string current_filter = "",
            int? page = 1, bool sort_flag = false)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Retrieve all active projects in view model form
                try
                {
                    IEnumerable<object> temp = admin.GetAllActiveProjects();

                    IEnumerable<AdminActiveProjectViewModel> active_project_list;

                    if (temp != null)
                    {
                        active_project_list = temp.Cast<AdminActiveProjectViewModel>();
                    }
                    else
                    {
                        active_project_list = null;
                    }                 

                    //For paging
                    ViewBag.PreviousSortCategory = prev_sort_category;
                    ViewBag.CurrentSortCategory = cur_sort_category;
                    ViewBag.CurrentSortOrder = sort_order;

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
                            case SortKey.SCHOOL_AMBASSADOR:
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
                    //Output error message
                    DialogResult status_update_error_box = MessageBox.Show(":-( Oh no! An error occurred during retrieval of all " +
                     " projects!", Popups.POP_UP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                     (MessageBoxOptions)0x40000);

                    //Gracefully deal with error
                    return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                }                            
            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpPost, ActionName("AdminUpdateProjectStatus")]
        public ActionResult UpdateProjectStatus(int? assignment_id, string current_project_status, string new_project_status)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Input checks and constraints
                if (assignment_id == null || assignment_id < 0) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                if (current_project_status == null || current_project_status == "") { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }
                if (new_project_status == null || new_project_status == "") { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                //Check to make sure that project update sequence is occurring in usual order
                //If not, output message box before changes are made permanent
                if (current_project_status == ProjectStatus.ASSIGNED && new_project_status == ProjectStatus.IN_PROGRESS)
                {                
                    try
                    {
                        //Update to In-Progress
                        admin.UpdateActiveStatusToInProg(assignment_id);

                        //Output message box signifying successful update to admin user
                        MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK, 
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                    catch
                    {
                        //Output error message
                        DialogResult status_update_error_box = MessageBox.Show(":-( Oh no! An error occurred during status update! " +
                         "We'll have to try this update again later.", Popups.POP_UP_TITLE,
                         MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                         (MessageBoxOptions)0x40000);

                        //Gracefully deal with error
                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                }
                else if (current_project_status == ProjectStatus.IN_PROGRESS && new_project_status == ProjectStatus.INTERN)
                {
                    //Update to Intern
                    try
                    {
                        //Update to In-Progress
                        admin.UpdateActiveStatusToIntern(assignment_id);

                        //Output message box signifying successful update to admin user
                        MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK, 
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                    catch
                    {
                        //Output error message
                        DialogResult status_update_error_box = MessageBox.Show(":-( Oh no! An error occurred during status update! " +
                         "We'll have to try this update again later.", Popups.POP_UP_TITLE,
                         MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                         (MessageBoxOptions)0x40000);

                        //Gracefully deal with error
                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                }
                else if (current_project_status == ProjectStatus.INTERN && new_project_status == ProjectStatus.PRODUCTION)
                {
                    //Update to Production
                    try
                    {
                        //Update to In-Progress
                        admin.UpdateActiveStatusToProduction(assignment_id);

                        //Output message box signifying successful update to admin user
                        MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK, 
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                    catch
                    {
                        //Output error message
                        DialogResult status_update_error_box = MessageBox.Show(":-( Oh no! An error occurred during status update! " +
                         "We'll have to try this update again later.", Popups.POP_UP_TITLE,
                         MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                         (MessageBoxOptions)0x40000);

                        //Gracefully deal with error
                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                }
                else if (new_project_status == ProjectStatus.ARCHIVED)
                {
                    //Archive the assignment
                    try
                    {                                        
                        //Check to ensure that user really would like to make the change                    
                        DialogResult result = MessageBox.Show("Are you sure you'd like to archive this project?", 
                            Popups.POP_UP_TITLE , MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1, 
                            (MessageBoxOptions)0x40000);

                        //Update to In-Progress
                        if (result == DialogResult.Yes)
                        {
                            admin.UpdateActiveStatusToArchived(assignment_id);

                            //Output message box signifying successful update to admin user
                            MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK, 
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                            return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);

                        }else if (result == DialogResult.No)
                        {
                            //Output message indicating termination of archive
                            MessageBox.Show("The status change will be stopped. ", Popups.POP_UP_TITLE, MessageBoxButtons.OK, 
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                            return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                        }                        

                    }
                    catch
                    {
                        //Output error message
                        DialogResult status_update_error_box = MessageBox.Show(":-( Oh no! An error occurred during status update! " +
                         "We'll have to try this update again later.", Popups.POP_UP_TITLE,
                         MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                         (MessageBoxOptions)0x40000);

                        //Gracefully deal with error
                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                }
                else
                {
                    //If in this else, admin status update in unusual sequence
                    //Output special dialog to ensure update is intended

                    //Caution user
                    DialogResult result = MessageBox.Show("You're updating this project out of normal sequence! " +
                        "Are you sure you want to make this status change?", Popups.POP_UP_TITLE,
                        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, 
                        (MessageBoxOptions)0x40000);

                    if (result == DialogResult.Yes)
                    {
                        //Update status as requested                    
                        try
                        {
                            admin.AdminUpdateProjectStatus((int)assignment_id, new_project_status);

                            MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                            return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                        }
                        catch
                        {
                            DialogResult status_update_error_box = MessageBox.Show(":-( Oh no! An error occurred during status update! " +
                                "We'll have to try this update again later.", Popups.POP_UP_TITLE,
                                MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, 
                                (MessageBoxOptions)0x40000);

                            if (status_update_error_box == DialogResult.OK)
                            {
                                return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);                    
                            }                        
                        }                    

                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);                    
                    }
                    else if (result == DialogResult.No)
                    {
                        //Print friendly confirmation
                        DialogResult update_status_check = MessageBox.Show("No problem! No changes were made to the projects status.",
                            Popups.POP_UP_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, 
                            (MessageBoxOptions)0x40000);

                        //When user clicks OK button, reload the page
                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    
                    }                
                }

                try
                {
                    admin.UpdateActiveStatusToInProg(assignment_id);

                    //Output message box
                    MessageBox.Show("Update Successful!", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                    return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                }
                catch
                {
                    //Output error message
                    DialogResult status_update_error_box = MessageBox.Show(":-( Oh no! An error occurred during status update! " +
                     "We'll have to try this update again later.", Popups.POP_UP_TITLE,
                     MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1,
                     (MessageBoxOptions)0x40000);

                    //Gracefully deal with error
                    return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                }

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }

        [HttpGet]
        public ActionResult ConstructEmail(string user_email)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Input checks
                if (user_email == null || user_email == "") 
                { return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME); }

                AdminEmailViewModel email = new AdminEmailViewModel();

                //Assign user email to necessary email view model field
                email.SendTo = user_email;

                return View(email);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }
        
        [HttpPost, ActionName("CreateEmail")]
        public ActionResult ConstructEmail(AdminEmailViewModel user_email)
        {   
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is admin
            if (role == UserRole.ADMIN)
            {

                //Input checks
                if (user_email == null) { return new HttpStatusCodeResult(HttpStatusCode.BadRequest); }

                if (ModelState.IsValid)
                {
                    //Assign user email to necessary email view model field            
                    if (admin.SendEmail(user_email))
                    {
                        //Output confirmation message for user
                        MessageBox.Show("Your email is on its way!", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                             MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        //Return to original view
                        return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                    }
                    else
                    {
                        //Gracefully indicate error                
                        MessageBox.Show(":-( Oh no! An error occurred while sending your email. " +
                            "We'll return you to email construction and reset the values with " +
                            "your entries.", Popups.POP_UP_TITLE, MessageBoxButtons.OK,
                             MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);

                        //Redirect to reconstruct email *** won't actually re-route value correctly
                        return RedirectToAction(ActionName.AN_RECONSTRUCT_EMAIL, PermissionSpace.PS_ADMIN_HOME,
                        new { Value = (AdminEmailViewModel)user_email });
                    }
                }

                return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
            }
        }


        [ActionName("ReconstructEmail")]
        public ActionResult ReconstructEmail(AdminEmailViewModel user_email)
        {
            //User role security check
            int role = (int)HttpContext.Session["userRole"];

            //Ensure user is contributor
            if (role == UserRole.ADMIN)
            {

                //Input checks *** Add message boxes to all alike
                if (user_email == null)
                {
                    return RedirectToAction(ActionName.AN_VIEW_ACTIVE_PROJECTS, PermissionSpace.PS_ADMIN_HOME);
                }

                AdminEmailViewModel email = new AdminEmailViewModel();

                //Assign user_email fields to necessary email view model field
                email.SendTo = user_email.SendTo;
                email.SentFrom = user_email.SentFrom;
                email.Subject = user_email.Subject;
                email.Priority = user_email.Priority;
                email.EmailBody = user_email.EmailBody;
                email.CCTo = user_email.CCTo;

                return View(email);

            }
            else
            {
                //Output error message box
                return RedirectToAction(ActionName.AN_PERMISSIONS_INVALID, PermissionSpace.PS_ADMIN_HOME);
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
