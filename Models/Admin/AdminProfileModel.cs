using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class AdminProfileModel
    {
        public User User { get; set; }
        [Display(Name = "Thay đổi ảnh đại diện")]
        public IFormFile LogoImage { get; set; }
        [Display(Name = "Giới tính")]
        [Required]
        public int SelectedGender { get; set; }
        public SelectList ListGender { get; set; }
        public AdminProfileModel()
        {
            User = new User();
            ListGender = new SelectList(new List<SelectListItem>());
        }
    }
}
