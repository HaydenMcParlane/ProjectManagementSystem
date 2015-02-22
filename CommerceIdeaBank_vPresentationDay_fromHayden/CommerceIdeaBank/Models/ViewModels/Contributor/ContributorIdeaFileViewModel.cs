using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommerceIdeaBank.Models.ViewModels.Contributor
{
    public class ContributorIdeaFileViewModel
    {       
        // IdeaFile table attributes
        public int IdeaFileID { get; set; }
        public int ProjectID { get; set; }
        public int FileID { get; set; }

        // File table attributes
        public byte[] FileContent { get; set; }
        public string MimeType { get; set; }
        public string FileName { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public System.DateTime UploadDate { get; set; }
    }
}