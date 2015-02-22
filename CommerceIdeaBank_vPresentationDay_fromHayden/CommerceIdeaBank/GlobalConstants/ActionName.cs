using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.GlobalConstants
{
    public class ActionName
    {
        /* This class contains constants used to name each view */

        /* Login / Logout */
        public static readonly string AN_LOGIN = "Login";
        public static readonly string AN_LOGOUT = "Logout";

        /* Default+ Actions */
        public static readonly string AN_INDEX = "Index";
        public static readonly string AN_VIEW_IDEA = "ViewIdea";
        public static readonly string AN_PERMISSIONS_INVALID = "PermissionsInvalid";

        /* Contributor+ Actions */        
        public static readonly string AN_DASHBOARD = "Dashboard";
        public static readonly string AN_MY_CONTRIBUTION = "MyContribution";
        public static readonly string AN_CONTRIBUTE_IDEA = "ContributeIdea";
        public static readonly string AN_EDIT_IDEA = "EditIdea";
        public static readonly string AN_ADD_FILE = "AddFile";

        /* Ambassador+ Actions */
        public static readonly string AN_CREATE_EMAIL = "CreateEmail";
        public static readonly string AN_CONSTRUCT_EMAIL = "ConstructEmail";
        public static readonly string AN_VIEW_ACTIVE_PROJECTS = "ViewActiveProjects";
        public static readonly string AN_UPDATE_PROJECT_STATUS = "UpdateProjectStatus";
        public static readonly string AN_VIEW_SCHOOL = "ViewSchool";
        public static readonly string AN_RECONSTRUCT_EMAIL = "ReconstructEmail";
        public static readonly string AN_VIEW_ALL_USERS = "ViewAllUsers";
        public static readonly string AN_VIEW_ALL_SCHOOLS = "ViewAllSchools";

        /* Admin Actions */
        public static readonly string AN_CREATE_SCHOOL = "CreateSchool";
        public static readonly string AN_VIEW_ARCHIVED_IDEAS = "ArchivedIdeas";
        public static readonly string AN_FINALIZE_ASSIGNMENT = "FinalizeAssignment";
        public static readonly string AN_SELECT_SCHOOL = "SelectSchool";
        public static readonly string AN_ARCHIVE_IDEA = "ArchiveIdea";
        public static readonly string AN_DELETE_IDEA = "DeleteIdea";
        public static readonly string AN_EDIT_SCHOOL = "EditSchool";
    }
}