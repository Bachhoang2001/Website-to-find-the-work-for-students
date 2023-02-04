using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PBL3.Database
{
    public partial class PostSubmit
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int StudentId { get; set; }
        public string StudentCvpath { get; set; }
        public string ShortText { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual CompanyPost Post { get; set; }
        public virtual StudentProfile PostNavigation { get; set; }
    }
}
