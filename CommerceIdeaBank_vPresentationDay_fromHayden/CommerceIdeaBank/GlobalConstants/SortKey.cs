using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.GlobalConstants
{
    public class SortKey
    {
        /* These keys are used in conjunction with the permission space Sort() functions
         * to sort lists
         */

        /* For Sort Order */
        public const string ASCEND = "ascend";
        public const string DESCEND = "descend";

        /* Sort Categories */

        /* IDEAS & PROJECTS */
        public const string PROJECT_NAME = "project_name";
        public const string PROJECT_DESC = "project_desc";
        public const string PROJECT_POST_DATE = "project_post_date";
        public const string PROJECT_USERNAME = "project_username";
        public const string PROJECT_STATUS = "project_status";        
        public const string PROJECT_AMBASSADOR = "project_ambassador";
        public const string EMAIL = "email";
        public const string PROJECT_ARCHIVED = "project_archived";

        /* SCHOOL */
        public const string SCHOOL_NAME = "school_name";
        public const string USER_ROLE = "user_role";
        public const string USER_STATUS = "user_status";
        public const string SCHOOL_AMBASSADOR = "school_ambassador";
        public const string SCHOOL_PHONE = "school_phone";

       
    }
}