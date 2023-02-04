using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using PBL3.Models.Common;
using PBL3.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models.Home
{
    public class IndexModel : SearchModel
    {
        public int SelectedFaculty {get;set;}
        public int MaxPostPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int SelectedPost { get; set; }
        public List<CompanyPost> Posts;
        public IndexModel()
        {
            Posts = new List<CompanyPost>();
            MaxPostPerPage = 6;
            CurrentPage = 1;
            CheckIsApproved = true;
            CheckIsDeleted = false;
        }
    }
}
