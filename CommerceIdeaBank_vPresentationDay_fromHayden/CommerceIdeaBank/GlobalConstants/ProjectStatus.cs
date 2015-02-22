using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.GlobalConstants
{
    public class ProjectStatus
    {
        //*** IMPORTANT *** Only store project-specific statuses (not idea statuses)
        // in this class because the Count() function is intended to count all of the
        // statuses. Because Count() relies on reflection, that needs to be the case.        
        static readonly string PROJ_ASSIGNED = "Assigned";
        static readonly string PROJ_IN_PROGRESS = "In-Progress";
        static readonly string PROJ_INTERN = "Intern";
        static readonly string PROJ_PRODUCTION = "Production";
        static readonly string PROJ_ARCHIVED = "Archived";
        
        public static string ASSIGNED { get { return PROJ_ASSIGNED; } }
        public static string IN_PROGRESS { get { return PROJ_IN_PROGRESS; } }
        public static string INTERN { get { return PROJ_INTERN; } }
        public static string PRODUCTION { get { return PROJ_PRODUCTION; } }
        public static string ARCHIVED { get { return PROJ_ARCHIVED; } }

        public static List<string> GetStatusList()
        {
            List<string> status_list = new List<string>();

            //Add each status into the list
            foreach (var property in typeof(ProjectStatus).GetProperties().ToList())
            {
                status_list.Add(property.GetValue(null).ToString());
            }

            return status_list;

        }

    }

}