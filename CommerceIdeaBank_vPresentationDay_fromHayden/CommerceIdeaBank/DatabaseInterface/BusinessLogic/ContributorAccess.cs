using CommerceIdeaBank.GlobalConstants;
using CommerceIdeaBank.Models;
using CommerceIdeaBank.Models.ViewModels.Admin;
using CommerceIdeaBank.Models.ViewModels.Ambassador;
using CommerceIdeaBank.Models.ViewModels.Contributor;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.DatabaseInterface.BusinessLogic
{

    //Class containing actions performable by a standard user
    public class ContributorAccess : DefaultAccess
    {

        //Func Desc: Used to submit project idea
        //    Input: A ProjectView instance
        //   Output: Bool indicating submission status. T = successful submission, F = failure to submit.
        public bool SubmitProject(object new_project)
        {
            //Input checks
            if (new_project == null) { return false; }

            using(var context = new MainDBEntities())
            {
                //Determine response based on access privilages
                if (this.GetType() == typeof(AdminAccess)) // IF ADMINISTRATOR
                {
                    try
                    {
                        //Cast object to admin project view model type
                        AdminProjectViewModel admin_project =
                            (AdminProjectViewModel)new_project;

                        //Create new project instance
                        Project proj = new Project();

                        //Transfer necessary values
                        proj.ProjectName = admin_project.ProjectName;
                        proj.ProjectDesc = admin_project.ProjectDesc;
                        proj.BusinessJustification = admin_project.BusinessJustification;
                        proj.Username = admin_project.Username;
                        proj.Status = IdeaStatus.SUBMITTED;
                        proj.IsArchived = false; //Not yet archived
                        proj.PostDate = DateTime.Now;
                        proj.AssignDate = null;
                        proj.FinishDate = null;

                        //Submit the project to the db
                        context.Projects.Add(proj);

                        //Save changes
                        context.SaveChanges();

                        //Indicate successful submission
                        return true;
                    }                
                    catch
                    {
                        //Return false indicating failure to submit project
                        return false;
                    }
                }
                else if (this.GetType() == typeof(AmbassadorAccess)) 
                {
                    try
                    {
                        //Cast object to contributor project view model type
                        AmbassProjectViewModel contributor_project =
                            (AmbassProjectViewModel)new_project;

                        //Create new project instance
                        Project proj = new Project();

                        //Transfer necessary values
                        proj.ProjectName = contributor_project.ProjectName;
                        proj.ProjectDesc = contributor_project.ProjectDesc;
                        proj.BusinessJustification = contributor_project.BusinessJustification;
                        proj.Username = contributor_project.Username;
                        proj.Status = IdeaStatus.SUBMITTED;
                        proj.IsArchived = false; //Not yet archived
                        proj.PostDate = DateTime.Now;
                        proj.AssignDate = null;
                        proj.FinishDate = null;

                        //Submit the project to the db
                        context.Projects.Add(proj);

                        //Save changes
                        context.SaveChanges();

                        //Indicate successful submission
                        return true;
                    }                
                    catch
                    {
                        //Return false indicating failure to submit project
                        return false;
                    }
                }
                else if (this.GetType() == typeof(ContributorAccess))
                {
                    try
                    {
                        //
                        ContributorProjectViewModel contributor_project = (ContributorProjectViewModel)new_project;

                        //Create new project instance
                        Project proj = new Project();

                        //Transfer necessary values
                        proj.ProjectName = contributor_project.ProjectName;
                        proj.ProjectDesc = contributor_project.ProjectDesc;
                        proj.BusinessJustification = contributor_project.BusinessJustification;
                        proj.Username = contributor_project.Username;
                        proj.Status = IdeaStatus.SUBMITTED;
                        proj.IsArchived = false; //Not yet archived
                        proj.PostDate = DateTime.Now;
                        proj.AssignDate = null;
                        proj.FinishDate = null;

                        //Submit the project to the db
                        context.Projects.Add(proj);

                        //Save changes
                        context.SaveChanges();

                        //Indicate successful submission
                        return true;
                    }                
                    catch
                    {
                        //Return false indicating failure to submit project
                        return false;
                    } 
  
                }
                else
                {
                    //Access object not recognized
                    Debug.WriteLine("\n\n***** " +
                        "Access object type wasn't recognized during SubmitProject(). " +
                        "ERROR IN: CommerceIdeaBank.DatabaseInterface.BusinessLogic.ContributorAccess SubmitProject()" +
                        "*****\n\n");

                    //Indicate error
                    return false;
                }
        }


    }        


        //Func Desc: Used to return a project from its id.
        //    Input: Int representing id of project to locate.
        //   Output: An instance of the project that has the specified id, or null
        public bool EditProject(object vm_project)
        {
            //Input checks
            if (vm_project == null) { return false; }
            if ( (vm_project.GetType() != typeof(AdminProjectViewModel)) &&
                 (vm_project.GetType() != typeof(AmbassProjectViewModel)) &&
                 (vm_project.GetType() != typeof(ContributorProjectViewModel)))
            {
                //Invalid view model
                Debug.WriteLine("\n\n***** " + 
                    "View model input into EditProject of ContributorAccess is invalid in type. " +
                    "ERROR IN: CommerceIdeaBank.DatabaseInterface.BusinessLogic.ContributorAccess EditProject()" +
                    "*****\n\n" );

                //Indicate failure in status
                return false;
            }

            using (var context = new MainDBEntities()) 
            {
                ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();                                                

                if (this.GetType() == typeof(AdminAccess))
                {
                    Project proj = (Project)mapper.Map((AdminProjectViewModel)vm_project, typeof(Project));

                    //Create the project edit and populate necessary attributes                                             
                    ProjectEdit edit = new ProjectEdit();
                    edit.Username = proj.Username;
                    edit.ProjectID = proj.ProjectID;
                    edit.EditDate = DateTime.Now;
                    context.ProjectEdits.Add(edit);

                    //Indicate modification and make changes persistent
                    context.Entry(proj).State = EntityState.Modified;
                    context.SaveChanges();

                    //Return true indicating successful project edit
                    return true;

                }
                else if (this.GetType() == typeof(AmbassadorAccess))
                {
                    Project proj = (Project)mapper.Map((AmbassProjectViewModel)vm_project, typeof(Project));

                    //Create the project edit and populate necessary attributes                                             
                    ProjectEdit edit = new ProjectEdit();
                    edit.Username = proj.Username;
                    edit.ProjectID = proj.ProjectID;
                    edit.EditDate = DateTime.Now;
                    context.ProjectEdits.Add(edit);

                    //Indicate modification and make changes persistent
                    context.Entry(proj).State = EntityState.Modified;
                    context.SaveChanges();

                    //Return true indicating successful project edit
                    return true;
                }
                else if (this.GetType() == typeof(ContributorAccess))
                {
                    Project proj = (Project)mapper.Map((ContributorProjectViewModel)vm_project, typeof(Project));

                    //Create the project edit and populate necessary attributes                                             
                    ProjectEdit edit = new ProjectEdit();
                    edit.Username = proj.Username;
                    edit.ProjectID = proj.ProjectID;
                    edit.EditDate = DateTime.Now;
                    context.ProjectEdits.Add(edit);

                    //Indicate modification and make changes persistent
                    context.Entry(proj).State = EntityState.Modified;
                    context.SaveChanges();

                    //Return true indicating successful project edit
                    return true;
                }
                else
                {
                    //Access type not recognized

                    Debug.WriteLine("\n\n***** " +
                        "Access object type wasn't recognized during EditProject(). " +
                        "ERROR IN: CommerceIdeaBank.DatabaseInterface.BusinessLogic.ContributorAccess EditProject()" +
                        "*****\n\n");

                    //Indicate error status
                    return false;
                }
              
            }
        }                


    }
}