using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.GlobalConstants
{
    public class PermissionSpace
    {
        /* These constants should exactly match the name of the home controllers
         * for each respective level of access (Admin, Ambassador and Contributor).
         */ 

        public static readonly string PS_CONTRIBUTOR_HOME = "ContributorHome";
        public static readonly string PS_AMBASSADOR_HOME = "AmbassadorHome";
        public static readonly string PS_ADMIN_HOME = "AdminHome";
        public static readonly string PS_DEFAULT_HOME = "Home";
    }
}