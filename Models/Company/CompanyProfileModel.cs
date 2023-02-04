using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Company
{
    public class CompanyProfileModel
    {
        public User User { get; set; }
        public CompanyProfile Company { get; set; }
        [Display(Name = "Thay đổi Logo Công ty")]
        public IFormFile LogoImage { get; set; }
        [Display(Name ="Giới tính")]
        [Required]
        public int SelectedGender { get; set; }
        public SelectList ListGender { get; set; }
        [Required,MinLength(1, ErrorMessage = "Bạn chưa chọn thành phố")]
        [Display(Name ="Chọn thành phố")]
        public List<int> ListCitiesId { get; set; }
        public IEnumerable<SelectListItem> GetAllCity { get; set; }
        public string SearchSkill { get; set; }
        public List<Skill> ListSkills { get; set; }
        public CompanyProfileModel()
        {
            User = new User();
            Company = new CompanyProfile();
            SelectedGender = 0;
            ListGender = new SelectList(new List<SelectListItem>());
            ListCitiesId = new List<int>();
            ListSkills = new List<Skill>();
            SearchSkill = "";
        }
    }
}
