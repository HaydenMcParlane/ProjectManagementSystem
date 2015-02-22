using CommerceIdeaBank.GlobalConstants;
using CommerceIdeaBank.Models;
using CommerceIdeaBank.Models.ViewModels.Admin;
using CommerceIdeaBank.Models.ViewModels.Ambassador;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Helpers;

namespace CommerceIdeaBank.DatabaseInterface.BusinessLogic
{
    public class AmbassadorAccess : ContributorAccess
    {

        public object GetSchool(int? id)
        {
            //Input checks
            if (id == null) { return null; }
            if (id < 0) { return null; }

            using (var context = new MainDBEntities())
            {
                //Read in school with id = id
                object school = context.Schools.Find(id);
                if (school == null) { return null; }

                //Instantiate mapper object
                ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();                                

                //Create application specific mapper and map to view model
                if (this.GetType() == typeof(AdminAccess))
                {
                    AdminSchoolViewModel vm_school = (AdminSchoolViewModel)mapper.Map((School)school, typeof(AdminSchoolViewModel));

                    return (object)vm_school;
                }
                else if (this.GetType() == typeof(AmbassadorAccess))
                {
                    AmbassSchoolViewModel vm_school = (AmbassSchoolViewModel)mapper.Map((School)school, typeof(AmbassSchoolViewModel));                                        

                    return (object)vm_school;
                }
                else
                {
                    //Error : Access object not recognized
                    return null;
                }
            }

            
        }

        public IEnumerable<object> GetAllActiveProjects()
        {
            using (var context = new MainDBEntities())
            {
                if (this.GetType() == typeof(AdminAccess))
                {                    
                    var temp = (from assignment in context.ProjectAssignments
                                    join project in context.Projects on assignment.ProjectID equals project.ProjectID 
                                        into first from first_beg in first.DefaultIfEmpty()
                                    join school in context.Schools on assignment.SchoolID equals school.SchoolID  
                                        into second from second_beg in second.DefaultIfEmpty()
                                    join ambassador in context.Users on second_beg.Username equals ambassador.Username
                                        into third from third_beg in third.DefaultIfEmpty()
                                select new {
                                    ProjectAssignId = assignment.ProjectAssignID,                                                               
                                    ProjectID = assignment.ProjectID, 
                                    SchoolID = assignment.SchoolID, 
                                    ProjectName = first_beg.ProjectName,
                                    SchoolName = second_beg.SchoolName,              
                                    ProgressStatus = assignment.ProgressStatus,
                                    Username = third_beg.Username,                                                               
                                    Email = third_beg.Email                                    
                                        });                                       

                    //Convert into necessary view model
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    IEnumerable<object> assignment_list = (IEnumerable<object>)(mapper.MapAll(temp, typeof(AdminActiveProjectViewModel)));                                                            

                    //Return view models
                    return assignment_list;
                }
                else if (this.GetType() == typeof(AmbassadorAccess))
                {
                    //Modify to make ambassador specific ***

                    var temp = (from assignment in context.ProjectAssignments
                                join project in context.Projects on assignment.ProjectID equals project.ProjectID
                                    into first
                                from first_beg in first.DefaultIfEmpty()
                                join school in context.Schools on assignment.SchoolID equals school.SchoolID
                                    into second
                                from second_beg in second.DefaultIfEmpty()
                                join ambassador in context.Users on second_beg.Username equals ambassador.Username
                                    into third
                                from third_beg in third.DefaultIfEmpty()
                                select new
                                {
                                    ProjectAssignId = assignment.ProjectAssignID,
                                    ProjectID = assignment.ProjectID,
                                    SchoolID = assignment.SchoolID,
                                    ProjectName = first_beg.ProjectName,
                                    SchoolName = second_beg.SchoolName,
                                    ProgressStatus = assignment.ProgressStatus,
                                    Username = third_beg.Username,
                                    Email = third_beg.Email                                    
                                });

                    //Convert into necessary view model
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    IEnumerable<AmbassActiveProjectViewModel> vm_assignment_list = (IEnumerable<AmbassActiveProjectViewModel>)(mapper.MapAll(temp, typeof(AmbassActiveProjectViewModel))).Cast<AmbassActiveProjectViewModel>();

                    //Eliminate archived ideas for ambassador access
                    vm_assignment_list = vm_assignment_list.Where(x => x.ProgressStatus != ProjectStatus.ARCHIVED);

                    //Box items for passage into controller
                    IEnumerable<object> ob_assignment_list = (IEnumerable<object>)vm_assignment_list.Cast<object>();

                    //Return view models
                    return ob_assignment_list;
                }
                else
                {
                    //Error -- Access type of object not recognized
                    return null;
                }

            }            
        }

        public bool AddNote()
        {
            return false;
        }

        public IEnumerable<object> GetAllSchools()
        {
            using (var context = new MainDBEntities())
            {
                if (this.GetType() == typeof(AdminAccess))
                {
                    List<School> schools = context.Schools.ToList();

                    //Map all schools to AdminSchoolViewModel objects
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    //Convert objects from dom model to view model
                    IEnumerable<object> school_enum = mapper.MapAll(schools,
                        typeof(AdminSchoolViewModel)).Cast<object>().AsEnumerable();

                    //return enum of view models
                    return school_enum;
                }
                else if (this.GetType() == typeof(AmbassadorAccess))
                {
                    List<School> schools = context.Schools.ToList();

                    //Map all schools to AdminSchoolViewModel objects
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    //Convert objects from dom model to view model
                    IEnumerable<object> school_enum = mapper.MapAll(schools,
                        typeof(AmbassSchoolViewModel)).Cast<object>().AsEnumerable();

                    //return enum of view models
                    return school_enum;
                }
                else
                {
                    //Gracefully deal with error ***
                    return null;
                }
            }
        }

        public IEnumerable<object> GetAllUsers()
        {
            using (var context = new MainDBEntities())
            {
                if (this.GetType() == typeof(AdminAccess))
                {
                    List<User> users = context.Users.ToList();

                    //Map all schools to AdminSchoolViewModel objects
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    //Convert objects from dom model to view model
                    IEnumerable<object> user_enum = mapper.MapAll(users,
                        typeof(AdminUserViewModel)).Cast<object>().AsEnumerable();

                    //return enum of view models
                    return user_enum;
                }
                else if (this.GetType() == typeof(AmbassadorAccess))
                {
                    List<User> users = context.Users.ToList();

                    //Map all schools to AdminSchoolViewModel objects
                    ApplicationSpecificMapper mapper = new ApplicationSpecificMapper();

                    //Convert objects from dom model to view model
                    IEnumerable<object> user_enum = mapper.MapAll(users,
                        typeof(AmbassUserViewModel)).Cast<object>().AsEnumerable();

                    //return enum of view models
                    return user_enum;
                }
                else
                {
                    //Gracefully deal with error ***
                    return null;
                }
            }
        }

        public bool SendEmail(object email)
        {
            //Input checks
            if(email == null){ return false; }            

            if (this.GetType() == typeof(AdminAccess))
            {
                // *** Update to allow different client to be used
                try
                {
                    //Cast object as appropriate view model
                    AdminEmailViewModel vm_email = new AdminEmailViewModel();

                    vm_email = (AdminEmailViewModel)email;

                    //Configure email for gmail server send. Gmail account is default that was
                    //created specifically for idea bank
                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.SmtpPort = 25;
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "commerce.ideabank.default@gmail.com";
                    WebMail.Password = "ideabank";
                    WebMail.From = "commerce.ideabank.default@gmail.com";

                    //Value checks
                    if (vm_email.SendTo == null || vm_email.SendTo == "") { return false; }
                    if (vm_email.Subject == null)
                    {
                        vm_email.Subject = "Re: Commerce IdeaBank "; //Use default Subject
                    }
                    if (vm_email.EmailBody == null || vm_email.EmailBody == "") { return false; }                    

                    //Send email
                    WebMail.Send(vm_email.SendTo, vm_email.Subject, vm_email.EmailBody, vm_email.SentFrom,
                        vm_email.CCTo, null, true, null, null, null, null, vm_email.Priority, null);

                    //Indicate successful status
                    return true;

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                    //Indicate message send failure
                    return false;
                }
            }
            else if (this.GetType() == typeof(AmbassadorAccess))
            {
                //For ambassador email

                // *** Update to allow different client to be used
                try
                {
                    //Cast object as appropriate view model
                    AmbassEmailViewModel vm_email = new AmbassEmailViewModel();

                    vm_email = (AmbassEmailViewModel)email;

                    //Configure email for gmail server send. Gmail account is default that was
                    //created specifically for idea bank
                    WebMail.SmtpServer = "smtp.gmail.com";
                    WebMail.SmtpPort = 25;
                    WebMail.EnableSsl = true;
                    WebMail.UserName = "commerce.ideabank.default@gmail.com";
                    WebMail.Password = "ideabank";
                    WebMail.From = "commerce.ideabank.default@gmail.com";

                    //Value checks
                    if (vm_email.SendTo == null) { return false; }
                    if (vm_email.Subject == null)
                    {
                        vm_email.Subject = "Re: Commerce IdeaBank "; //Use default Subject
                    }
                    if (vm_email.EmailBody == null || vm_email.EmailBody == "") { return false; }
                    if (vm_email.EmailBody == null || vm_email.EmailBody == "") { return false; }                    

                    //Send email
                    WebMail.Send(vm_email.SendTo, vm_email.Subject, vm_email.EmailBody, vm_email.SentFrom ,
                        vm_email.CCTo, null, true, null, null, null, null, vm_email.Priority, null);

                    //Indicate successful status
                    return true;

                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);

                    //Indicate message send failure
                    return false;
                }
            }
            else
            {
                return false;
            }       
        }

        public bool UpdateActiveStatusToInProg(int? active_project_id)
        {
            //Input checks
            if (active_project_id == null) { return false; }
            if (active_project_id < 0) { return false; }

            using (var context = new MainDBEntities())
            {
                //Access object checks
                if ( (this.GetType() == typeof(AdminAccess)) ||
                     (this.GetType() == typeof(AmbassadorAccess)) )                  
                {
                    ProjectAssignment active_project = new ProjectAssignment();

                    //Read in necessary object
                    active_project = context.ProjectAssignments.Find(active_project_id);

                    //Modify status
                    active_project.ProgressStatus = ProjectStatus.IN_PROGRESS;

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

        //Constructors
        public AmbassadorAccess()
        {

        }
    }
}