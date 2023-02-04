using System;
using System.Collections.Generic;

#nullable disable

namespace PBL3.Database
{
    public partial class CompanyPost
    {
        public CompanyPost()
        {
            CityPosts = new HashSet<CityPost>();
            PostSubmits = new HashSet<PostSubmit>();
            Skillposts = new HashSet<Skillpost>();
        }

        public int Id { get; set; }
        public int CompanyId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }
        public int? MinSalary { get; set; }
        public int? MaxSalary { get; set; }
        public bool? IsDollar { get; set; }
        public bool IsApproved { get; set; }
        public virtual CompanyProfile Company { get; set; }
        public virtual ICollection<CityPost> CityPosts { get; set; }
        public virtual ICollection<PostSubmit> PostSubmits { get; set; }
        public virtual ICollection<Skillpost> Skillposts { get; set; }
    }
}
