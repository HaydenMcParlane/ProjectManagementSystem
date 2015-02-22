using CommerceIdeaBank.Models;
using CommerceIdeaBank.DatabaseInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommerceIdeaBank.Models.ViewModels.Admin;
using CommerceIdeaBank.GlobalConstants;
using CommerceIdeaBank.Models.ViewModels.Ambassador;
using System.Data.Entity;

namespace CommerceIdeaBank.DatabaseInterface.BusinessLogic
{
    //Class containing actions performable by an admin
    public class AdminAccess : AmbassadorAccess
    {
        public bool EditSchool(AdminSchoolViewModel vm_school)
        {
            if (vm_school == null) { return false; }

            try
            {
                using (var context = new MainDBEntities())
                {
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    //Validate input
                    if (mapper == null) { return false; }

                    School school = (School)mapper.Map((AdminSchoolViewModel)vm_school, typeof(School));

                    //Validate input
                    if (school == null) { return false; }

                    //Create the project edit and populate necessary attributes                                             
                    SchoolEdit edit = new SchoolEdit();

                    edit.SchoolID = school.SchoolID;
                    edit.Username = school.Username;
                    edit.SchoolName = school.SchoolName;
                    edit.Phone = (int)school.Phone;
                    edit.Email = school.Email;
                    edit.ContactEmail = school.ContactEmail;
                    edit.ContactName = school.ContactName;
                    edit.ContactPhone = school.ContactPhone;
                    edit.Department = school.Department;
                    edit.Class = school.Class;
                    edit.StreetNumber = school.StreetNumber;
                    edit.StreetName = school.StreetName;
                    edit.ZipCode = school.ZipCode;
                    edit.City = school.City;
                    edit.State = school.State;

                    edit.EditDate = DateTime.Now;

                    context.SchoolEdits.Add(edit);                    

                    //Indicate modification and make changes persistent
                    context.Entry(school).State = EntityState.Modified;
                    context.SaveChanges();

                    //Return true indicating successful project edit
                    return true;
                }
            }
            catch
            {
                //There was an error during school edit
                return false;
            }            
        }

        //Func Desc: Used to delete project idea. Admin function.
        //    Input: The id of the project to delete
        //   Output: Bool indicating deletion status. T = successful deletion, F = failure to delete.
        public bool DeleteProject(int? project_id)
        {

            if (project_id == null) { return false; }

            try
            {
                Project domain_model = new Project();

                using (var context = new MainDBEntities())
                {
                    domain_model = context.Projects.Find(project_id);

                    IEnumerable<ProjectAssignment> dependent_projects = 
                        context.ProjectAssignments.Where( x => x.ProjectID == project_id );

                    if (dependent_projects != null)
                    {
                        foreach (ProjectAssignment assignment in dependent_projects.ToList())
                        {                        
                            //Remove project
                            context.ProjectAssignments.Remove(assignment);                                                                                   
                        }

                        context.SaveChanges();
                    }

                    context.Projects.Remove(domain_model);

                    context.SaveChanges();

                    //Indicate delete success
                    return true;
                }
            }
            catch
            {
                //Indicate deletion failure
                return false;
            }
        }

        //Func Desc: Used to mark the project with primary key id to archived.
        //    Input: Int representing id of project to archive.
        //   Output: A bool indicating whether update was successful or not. T = success, F = failure.
        public bool ArchiveProject(int? id)
        {
            if (id == null) { return false; }

            using (var context = new MainDBEntities())
            {
                //Retrieve project
                Project project = context.Projects.Find(id);

                //Mark the project as archived
                if (project != null)
                {
                    project.IsArchived = true;
                }
                else
                {
                    //Indicate error status
                    return false;
                }

                //Indicate that that value has been modified
                context.Projects.Attach(project);
                var entry = context.Entry(project);

                //Indicate that IsArchived property has been modified (indicates to EF that property
                //needs update
                entry.Property(x => x.IsArchived).IsModified = true;

                //Save update
                context.SaveChanges();

                //Indicate successful archive
                return true;

            }

        }


        //Func Desc: Used to submit school to database
        //    Input: A SchoolViewModel object instance
        //   Output: A bool indicating whether submission succeeded. T = success, F = failure
        public bool SubmitSchool(AdminSchoolViewModel new_school)
        {
            //Input checks
            if (new_school == null) { return false; }

            try
            {
                using (var context = new MainDBEntities())
                {
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    //Create new project instance
                    School school = new School();
                    
                    //Map the view model to the domain model
                    school = (School)mapper.Map(new_school, typeof(School));

                    //Submit the project to the db
                    context.Schools.Add(school);

                    //Save changes
                    context.SaveChanges();

                    //Indicate successful submission
                    return true;
                }
            }
            catch
            {
                //Return false indicating failure to submit project
                return false;
            }
        }        

        public bool AddAmbassToSchool(int? selected_school_id, string new_ambassador_username)
        {
            //Input validation
            if (selected_school_id == null || selected_school_id < 0) { return false; }
            if (new_ambassador_username == null || new_ambassador_username == "") { return false; }

            using (var context = new MainDBEntities())
            {
                //Find project with id = project_id, store in temp_project
                School school = context.Schools.Find(selected_school_id);

                User ambassador = context.Users.Find(new_ambassador_username);

                //Perform input validation
                if (school == null || ambassador == null) { return false; }

                //Ensure user is actually ambassador before assignment and that 
                //user actually exists in database
                if (ambassador.UserRole != UserRole.AMBASSADOR)
                {
                    return false;
                }
                else
                {
                    school.Username = new_ambassador_username;

                    //Verify that valid project was found
                    context.Schools.Add(school);                   

                    //Indicate modification and make changes persistent
                    context.Entry(school).State = EntityState.Modified;
                    context.SaveChanges();

                    //Return true indicating successful project edit
                    return true;
                }
            }                                                
        }

        public bool RemoveAmbassFromSchool(int? selected_school_id)
        {
            //Input validation
            if (selected_school_id == null || selected_school_id < 0) { return false; }

            //Remove ambassador from selected school
            using (var context = new MainDBEntities())
            {
                //Find project with id = project_id, store in temp_project
                School school = context.Schools.Find(selected_school_id);                

                //Ensure there's a point to the update
                if (school.Username == null || school.Username == "") { return true; }

                //Ensure user is actually ambassador before assignment and that 
                //user actually exists in database
                school.Username = null;

                //Verify that valid project was found
                context.Schools.Add(school);

                //Indicate modification and make changes persistent
                context.Entry(school).State = EntityState.Modified;
                context.SaveChanges();

                //Return true indicating successful project edit
                return true;                
            }
        }

        //Func Desc: 
        //    Input: 
        //   Output: 
        public bool AssignProjectToSchool(int? project_id, int? school_id)
        {
            if (project_id == null) { return false; }
            if (school_id == null) { return false; }
            if (school_id < 0) { return false; }
            if (project_id < 0) { return false; }

            using (var context = new MainDBEntities())
            {
                //Find project with id = project_id, store in temp_project
                Project temp_project = context.Projects.Find(project_id);

                //Verify that valid project was found
                if (temp_project == null) { return false; }

                //Create Project Assignment
                ProjectAssignment new_assignment = new ProjectAssignment();

                //Set starting assignment values
                new_assignment.ProgressStatus = ProjectStatus.ASSIGNED;
                new_assignment.ProjectID = (int)project_id;
                new_assignment.SchoolID = (int)school_id;
                new_assignment.DateAssigned = DateTime.Now;

                //Save changes to the database
                context.ProjectAssignments.Add(new_assignment);                

                //Save update
                context.SaveChanges();

                //Indicate successful assignment
                return true;

            }
        }


        public bool AdminUpdateProjectStatus(int project_id, string new_status)
        {
            //Perform input validation
            if (new_status == null || new_status == "") { return false; }
            if( (new_status != ProjectStatus.ASSIGNED) &&
                (new_status != ProjectStatus.IN_PROGRESS) &&
                (new_status != ProjectStatus.INTERN) &&
                (new_status != ProjectStatus.PRODUCTION) &&
                (new_status != ProjectStatus.ARCHIVED) )
            {
                return false;
            }

            using (var context = new MainDBEntities())
            {
                //Access object checks
                if (this.GetType() == typeof(AdminAccess))
                {
                    ProjectAssignment current_project = new ProjectAssignment();

                    //Read in necessary object
                    current_project = context.ProjectAssignments.Find(project_id);

                    //Modify status
                    current_project.ProgressStatus = new_status;

                    //Indicate that that value has been modified
                    context.ProjectAssignments.Attach(current_project);
                    var entry = context.Entry(current_project);

                    //Indicate that IsArchived property has been modified (indicates to EF that property
                    //needs update
                    entry.Property(x => x.ProgressStatus).IsModified = true;

                    //Save update
                    context.SaveChanges();

                    //Indicate successful update
                    return true;
                }
                else
                {
                    //Invalid access object
                    return false;
                }
            }

        }

        public bool UpdateActiveStatusToAssigned(int? active_project_id)
        {
            //Input checks
            if (active_project_id == null) { return false; }
            if (active_project_id < 0) { return false; }

            using (var context = new MainDBEntities())
            {
                //Access object checks
                if (this.GetType() == typeof(AdminAccess))
                {
                    ProjectAssignment active_project = new ProjectAssignment();

                    //Read in necessary object
                    active_project = context.ProjectAssignments.Find(active_project_id);

                    //Modify status
                    active_project.ProgressStatus = ProjectStatus.ASSIGNED;

                    //Indicate that that value has been modified
                    context.ProjectAssignments.Attach(active_project);
                    var entry = context.Entry(active_project);

                    //Indicate that IsArchived property has been modified (indicates to EF that property
                    //needs update
                    entry.Property(x => x.ProgressStatus).IsModified = true;

                    //Save update
                    context.SaveChanges();

                    //Indicate successful archive
                    return true;
                }
                else
                {
                    //Invalid access object
                    return false;
                }
            }
        }

        public bool UpdateActiveStatusToIntern(int? active_project_id)
        {
            //Input checks
            if (active_project_id == null) { return false; }
            if (active_project_id < 0) { return false; }

            using (var context = new MainDBEntities())
            {
                //Access object checks
                if ((this.GetType() == typeof(AdminAccess)) ||
                     (this.GetType() == typeof(AmbassadorAccess)))
                {
                    ProjectAssignment active_project = new ProjectAssignment();

                    //Read in necessary object
                    active_project = context.ProjectAssignments.Find(active_project_id);

                    //Modify status
                    active_project.ProgressStatus = ProjectStatus.INTERN;

                    //Indicate that that value has been modified
                    context.ProjectAssignments.Attach(active_project);
                    var entry = context.Entry(active_project);

                    //Indicate that IsArchived property has been modified (indicates to EF that property
                    //needs update
                    entry.Property(x => x.ProgressStatus).IsModified = true;

                    //Save update
                    context.SaveChanges();

                    //Indicate successful archive
                    return true;
                }
                else
                {
                    //Invalid access object ***
                    return false;
                }
            }
        }

        public bool UpdateActiveStatusToProduction(int? active_project_id)
        {
            //Input checks
            if (active_project_id == null) { return false; }
            if (active_project_id < 0) { return false; }

            using (var context = new MainDBEntities())
            {
                //Access object checks
                if ((this.GetType() == typeof(AdminAccess)) ||
                     (this.GetType() == typeof(AmbassadorAccess)))
                {
                    ProjectAssignment active_project = new ProjectAssignment();

                    //Read in necessary object
                    active_project = context.ProjectAssignments.Find(active_project_id);

                    //Modify status
                    active_project.ProgressStatus = ProjectStatus.PRODUCTION;

                    //Indicate that that value has been modified
                    context.ProjectAssignments.Attach(active_project);
                    var entry = context.Entry(active_project);

                    //Indicate that IsArchived property has been modified (indicates to EF that property
                    //needs update
                    entry.Property(x => x.ProgressStatus).IsModified = true;

                    //Save update
                    context.SaveChanges();

                    //Indicate successful archive
                    return true;
                }
                else
                {
                    //Invalid access object
                    return false;
                }
            }
        }

        public bool UpdateActiveStatusToArchived(int? active_project_id)
        {
            //Input checks
            if (active_project_id == null) { return false; }
            if (active_project_id < 0) { return false; }

            using (var context = new MainDBEntities())
            {
                //Access object checks
                if (this.GetType() == typeof(AdminAccess))                     
                {
                    ProjectAssignment active_project = new ProjectAssignment();

                    //Read in necessary object
                    active_project = context.ProjectAssignments.Find(active_project_id);

                    //Modify status
                    active_project.ProgressStatus = ProjectStatus.ARCHIVED;

                    //Indicate that that value has been modified
                    context.ProjectAssignments.Attach(active_project);
                    var entry = context.Entry(active_project);

                    //Indicate that IsArchived property has been modified (indicates to EF that property
                    //needs update
                    entry.Property(x => x.ProgressStatus).IsModified = true;

                    //Save update
                    context.SaveChanges();

                    //Indicate successful archive
                    return true;
                }
                else
                {
                    //Invalid access object ***
                    return false;
                }
            }
        }

        public bool UpdateUserRole(string username, int? new_role) 
        {
            if (username == null || username == "") { return false; }
            if ( (new_role != UserRole.ADMIN) &&
                 (new_role != UserRole.AMBASSADOR) &&
                 (new_role != UserRole.CONTRIBUTOR))
            {
                return false;
            }

            using (var context = new MainDBEntities())
            {
                User selected_user = context.Users.Find(username);

                if (selected_user == null) { return false; }                                

                //Modify status
                if (new_role != null)
                {
                    selected_user.UserRole = (int)new_role;
                }
                else
                {
                    return false;
                }
                

                //Indicate that that value has been modified
                context.Users.Attach(selected_user);
                var entry = context.Entry(selected_user);

                //Indicate that IsArchived property has been modified (indicates to EF that property
                //needs update
                entry.Property(x => x.UserRole).IsModified = true;

                //Save update
                context.SaveChanges();

                //Indicate successful archive
                return true;
            }
        }

        //Constructors
        public AdminAccess()
        {

        }
    }
}