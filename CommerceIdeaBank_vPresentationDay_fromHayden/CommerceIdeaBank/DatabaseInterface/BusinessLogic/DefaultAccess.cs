using CommerceIdeaBank.Models;
using CommerceIdeaBank.Models.ViewModels.Admin;
using CommerceIdeaBank.Models.ViewModels.Ambassador;
using CommerceIdeaBank.Models.ViewModels.Contributor;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.DatabaseInterface.BusinessLogic
{
    public class DefaultAccess
    {

        //Func Desc: Used to return a project from its id.
        //    Input: Int representing id of project to locate.
        //   Output: An instance of the project that has the specified id, or null
        public object GetProject(int? project_id) //Should this be nullable? ***
        {
            //Input checks
            if (project_id == null) { return null; }
            if (project_id < 0) { return null; }

            using (var context = new MainDBEntities())
            {
                ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                if (this.GetType() == typeof(AdminAccess))
                {
                    //Map located project to project view model                    
                    AdminProjectViewModel vm_project = (AdminProjectViewModel)mapper.Map(context.Projects.Find(project_id), typeof(AdminProjectViewModel));

                    //Return project with given id or null
                    return vm_project;
                }
                else if (this.GetType() == typeof(AmbassadorAccess))
                {
                    //Map located project to project view model                    
                    AmbassProjectViewModel vm_project = (AmbassProjectViewModel)mapper.Map(context.Projects.Find(project_id), typeof(AmbassProjectViewModel));

                    //Make sure project isn't archived
                    if (vm_project.IsArchived == true) { return null; }

                    //Return project with given id or null
                    return vm_project;
                }
                else if (this.GetType() == typeof(ContributorAccess))
                {
                    //Map located project to project view model                    
                    ContributorProjectViewModel vm_project = (ContributorProjectViewModel)mapper.Map(context.Projects.Find(project_id), typeof(ContributorProjectViewModel));

                    //Make sure project isn't archived
                    if (vm_project.IsArchived == true) { return null; }

                    //Return project with given id or null
                    return vm_project;
                }
                else if (this.GetType() == typeof(DefaultAccess))
                {
                    //Map located project to project view model                    
                    ProjectViewModel vm_project = (ProjectViewModel)mapper.Map(context.Projects.Find(project_id), typeof(ProjectViewModel));

                    //Make sure project isn't archived
                    if (vm_project.IsArchived == true) { return null; }

                    //Return project with given id or null
                    return vm_project;
                }
                else
                {
                    //Invalid access object
                    Debug.WriteLine("\n\n***** " +
                        "Access object type wasn't recognized during GetProject(). " +
                        "ERROR IN: CommerceIdeaBank.DatabaseInterface.BusinessLogic.ContributorAccess GetProject()" +
                        "*****\n\n");

                    return null;
                }
            }
        }


        //Func Desc: Used to get the current list of projects.
        //    Input: None.
        //   Output: An IEnumerable< T = appropriate view model >, or null
        public IEnumerable<object> GetProjectList()
        {
            using (var context = new MainDBEntities())
            {
                //Create mapper
                ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();
                IEnumerable<object> project_list;
                IEnumerable<object> view_model_list;

                //Check the type of the calling object and vary reaction
                if (this.GetType() == typeof(AdminAccess))
                {
                    //If admin, use admin project view model

                    //Get project list
                    project_list = (IEnumerable<object>)context.Projects.ToList().Cast<object>();

                    try
                    {
                        //Map list to corresponding view model
                        view_model_list = mapper.MapAll(project_list, typeof(AdminProjectViewModel)).Cast<AdminProjectViewModel>();
                    }
                    catch
                    {
                        view_model_list = null;
                    }                    

                    if (view_model_list == null) { return null; }
                    
                    //Return list of view models
                    return (IEnumerable<object>)view_model_list.Cast<AdminProjectViewModel>().GetEnumerator();

                }
                else if (this.GetType() == typeof(AmbassadorAccess))
                {
                    //If ambassador, use ambass project view model

                    //Get project list
                    project_list = (IEnumerable<object>)context.Projects.ToList().Cast<object>();
                    IEnumerable<AmbassProjectViewModel> temp;

                    try
                    {
                        //Map list to corresponding view model
                        temp = (IEnumerable<AmbassProjectViewModel>)mapper.MapAll(project_list,
                        typeof(AmbassProjectViewModel)).Cast<AmbassProjectViewModel>();
                    }
                    catch
                    {
                        temp = null;
                    }                 

                    if (temp == null) { return null; }

                    //Eliminate archived ideas for ambassador
                    view_model_list = temp.Where(x => x.IsArchived == false).Cast<object>();

                    //Return list of view models
                    return (IEnumerable<object>)view_model_list.Cast<AmbassProjectViewModel>().GetEnumerator();
                }
                else if (this.GetType() == typeof(ContributorAccess))
                {
                    //If contributor, use contributor project view model

                    //Get project list
                    project_list = (IEnumerable<object>)context.Projects.ToList().Cast<object>();
                    IEnumerable<ContributorProjectViewModel> temp;

                    try
                    {
                        //Map list to corresponding view model
                        temp = (IEnumerable<ContributorProjectViewModel>)mapper.MapAll(project_list,
                            typeof(ContributorProjectViewModel)).Cast<ContributorProjectViewModel>();
                    }
                    catch
                    {
                        temp = null;
                    }

                    if (temp == null) { return null; }

                    //Eliminate archived ideas for contributor
                    view_model_list = temp.Where(x => x.IsArchived == false).Cast<object>();

                    //Return list of view models
                    return (IEnumerable<object>)view_model_list.Cast<ContributorProjectViewModel>().GetEnumerator();

                }
                else if (this.GetType() == typeof(DefaultAccess))
                {
                    //If default, use default project view model

                    //Get project list
                    project_list = (IEnumerable<object>)context.Projects.ToList().Cast<object>();

                    //Map list to corresponding view model
                    IEnumerable<ProjectViewModel> temp;

                    try
                    {
                        temp = (IEnumerable<ProjectViewModel>)mapper.MapAll(project_list, typeof(ProjectViewModel)).Cast<ProjectViewModel>();
                    }
                    catch
                    {
                        temp = null; 
                    }

                    if (temp == null) { return null; }

                    //Eliminate archived ideas for default
                    view_model_list = temp.Where(x => x.IsArchived == false).Cast<object>();

                    //Return list of view models
                    return (IEnumerable<object>)view_model_list.Cast<ProjectViewModel>().GetEnumerator();
                }
                else
                {
                    //Invalid access type
                    Debug.WriteLine("\n\n***** " +
                        "Access object type wasn't recognized during GetProject(). " +
                        "ERROR IN: CommerceIdeaBank.DatabaseInterface.BusinessLogic.ContributorAccess GetProject()" +
                        "*****\n\n");

                    //Gracefully indicate error
                    return null;
                }
            }
        }
    }
}