//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CommerceIdeaBank.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AssignmentMetaData
    {
        public int AssignMetaDataID { get; set; }
        public int ProjectAssignID { get; set; }
        public Nullable<int> NumAmbassEmailsSent { get; set; }
        public Nullable<int> NumAdminEmailsSent { get; set; }
        public Nullable<int> NumAssociatedFiles { get; set; }
        public Nullable<int> NumMeetingsHeldWithSchool { get; set; }
        public Nullable<int> ProjectPerformanceRating { get; set; }
        public Nullable<int> SchoolPerformanceRating { get; set; }
        public Nullable<int> NumPerspectiveInterns { get; set; }
        public Nullable<int> NumInternsHired { get; set; }
        public Nullable<int> QualityOfCompletedProject { get; set; }
        public Nullable<int> StudentPerformanceRating { get; set; }
        public Nullable<int> EstDifficultyRating { get; set; }
    
        public virtual ProjectAssignment ProjectAssignment { get; set; }
    }
}
