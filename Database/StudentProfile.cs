using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PBL3.Database
{
    public partial class StudentProfile
    {
        public StudentProfile()
        {
            CompanyFeedbacks = new HashSet<CompanyFeedback>();
            PostSubmits = new HashSet<PostSubmit>();
            StudentSkills = new HashSet<StudentSkill>();
        }

        public int Id { get; set; }
        [Display(Name = "MSSV")]
        public int Mssv { get; set; }
        [Display(Name = "Khoa")]
        public int Faculty { get; set; }
        [Display(Name = "Thành phố")]
        public int City { get; set; }
        [Required(ErrorMessage = "Vui lòng điền chứng chỉ ngoại ngữ")]
        [Display(Name = "Ngoại ngữ")]
        public string ForeignLanguage { get; set; }
        [Display(Name = "Điểm trung bình")]
        public double? Gpa { get; set; }
        public string CVLocation { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual City CityNavigation { get; set; }
        public virtual Faculty FacultyNavigation { get; set; }
        public virtual User IdNavigation { get; set; }
        public virtual ICollection<CompanyFeedback> CompanyFeedbacks { get; set; }
        public virtual ICollection<PostSubmit> PostSubmits { get; set; }
        public virtual ICollection<StudentSkill> StudentSkills { get; set; }
    }
}
