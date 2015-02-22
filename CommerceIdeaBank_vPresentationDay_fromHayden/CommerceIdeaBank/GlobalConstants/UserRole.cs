using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.GlobalConstants
{
    public class UserRole
    {
        private static readonly int ROLE_ADMIN = 4;
        private static readonly int ROLE_AMBASSADOR = 3;        
        private static readonly int ROLE_CONTRIBUTOR = 1;
        public static readonly string ROLE_TITLE_ADMIN = "Admin";
        public static readonly string ROLE_TITLE_AMBASSADOR = "Ambassador";
        public static readonly string ROLE_TITLE_CONTRIBUTOR = "Contributor";

        public static int ADMIN { get { return ROLE_ADMIN; } }
        public static int AMBASSADOR { get { return ROLE_AMBASSADOR; } }
        public static int CONTRIBUTOR { get { return ROLE_CONTRIBUTOR; } }  
      


        public static List<int> GetRoleList()
        {
            var properties = typeof(UserRole).GetProperties().ToList();
            List<int> role_list = new List<int>();

            foreach (var property in properties)
            {
                role_list.Add( (int)property.GetMethod.Invoke(null, null) );
            }

            return role_list;
        } 

    }
}