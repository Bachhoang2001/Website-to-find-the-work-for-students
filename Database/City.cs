using System;
using System.Collections.Generic;

#nullable disable

namespace PBL3.Database
{
    public partial class City
    {
        public City()
        {
            CityPosts = new HashSet<CityPost>();
            CompanyCities = new HashSet<CompanyCity>();
            StudentProfiles = new HashSet<StudentProfile>();
        }

        public int Id { get; set; }
        public string CityName { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual ICollection<CityPost> CityPosts { get; set; }
        public virtual ICollection<CompanyCity> CompanyCities { get; set; }
        public virtual ICollection<StudentProfile> StudentProfiles { get; set; }
    }
}
