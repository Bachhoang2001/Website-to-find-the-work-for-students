using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Student
{
    public class StudentProfileModel
    {
        public User User { get; set; }
        public StudentProfile Student { get; set; }
        [Display(Name = "Thay đổi ảnh đại diện")]
        public IFormFile LogoImage { get; set; }
        [Display(Name = "Thay đổi CV")]
        public IFormFile CVFile { get; set; }
        [Display(Name = "Giới tính")]
        [Required]
        public int SelectedGender { get; set; }
        public SelectList ListGender { get; set; }
        [Display(Name = "Chọn thành phố")]
        public IEnumerable<SelectListItem> GetAllCity { get; set; }
        public string SearchSkill { get; set; }
        public List<Skill> ListSkills { get; set; }
        public StudentProfileModel()
        {
            User = new User();
            Student = new StudentProfile();
            SelectedGender = 0;
            ListGender = new SelectList(new List<SelectListItem>());
            ListSkills = new List<Skill>();
            SearchSkill = "";
        }
    }
}
