using System;
using System.Collections.Generic;

#nullable disable

namespace PBL3.Database
{
    public partial class Faculty
    {
        public Faculty()
        {
            StudentProfiles = new HashSet<StudentProfile>();
        }

        public int Id { get; set; }
        public string FacultyName { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual ICollection<StudentProfile> StudentProfiles { get; set; }
    }
}
