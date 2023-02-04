using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Home
{
    public class CompanySignUpModel
    {
        public User User { get; set; }
        public CompanyProfile Company { get; set; }
        public List<City> GetAllCity { get; set; }
        [Required(ErrorMessage ="Vui lòng chọn thành phố")]
        [Display(Name = "Chọn thành phố")]
        public List<int> ListCityId { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mật khẩu")]
        [Display(Name = "Nhập mật khẩu")]
        [StringLength(255, ErrorMessage = "Mật khẩu tối thiểu 8 kí tự.", MinimumLength = 8)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%!&*~^]).{0,})", ErrorMessage = "Mật khẩu tối thiểu 1 chữ số, 1 chữ cái thường, 1 chữ cái hoa, 1 kí tự đặc biệt.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng điền lại mật khẩu")]
        [Compare("Password", ErrorMessage ="Mật khẩu không trùng khớp")]
        [Display(Name = "Nhập lại mật khẩu")]
        public string PasswordCheck { get; set; }
        public CompanySignUpModel()
        {
            User = new User();
            Company = new CompanyProfile();
            User.RoleId = 2;
            User.DateOfBirth = DateTime.Now;
            User.IsApproved = false;
            User.Isdeleted = false;
            User.AvtLocation = "anonymous.png";
            User.Createdate = DateTime.Now;
            User.Updatedate = DateTime.Now;
            User.Createby = 1;
            User.Updateby = 1;
            User.Deleteby = 1;
            User.Gender = 0;
            Company.Createdate = DateTime.Now;
            Company.Updatedate = DateTime.Now;
            Company.Isdeleted = false;
            Company.Createby = 1;
            Company.Updateby = 1;
            Company.Deleteby = 1;
            Company.Rate = (double)0;
        }
    }
}
