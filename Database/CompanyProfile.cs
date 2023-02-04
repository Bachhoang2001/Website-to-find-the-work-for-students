using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PBL3.Database
{
    public partial class CompanyProfile
    {
        public CompanyProfile()
        {
            CompanyCities = new HashSet<CompanyCity>();
            CompanyFeedbacks = new HashSet<CompanyFeedback>();
            CompanyPosts = new HashSet<CompanyPost>();
            CompanySkills = new HashSet<CompanySkill>();
        }

        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng điền tên công ty")]
        [Display(Name = "Tên Công ty")]
        public string CompanyName { get; set; }
        [Required(ErrorMessage = "Vui lòng điền website công ty")]
        [Display(Name = "Website Công ty")]
        public string Website { get; set; }
        public double? Rate { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual User IdNavigation { get; set; }
        public virtual ICollection<CompanyCity> CompanyCities { get; set; }
        public virtual ICollection<CompanyFeedback> CompanyFeedbacks { get; set; }
        public virtual ICollection<CompanyPost> CompanyPosts { get; set; }
        public virtual ICollection<CompanySkill> CompanySkills { get; set; }
    }
}
