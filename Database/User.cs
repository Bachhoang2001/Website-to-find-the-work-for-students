using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace PBL3.Database
{
    public partial class User
    {
        public int Id { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage ="Vui lòng điền Email")]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public string Cicno { get; set; }
        [Display(Name = "Tên")]
        [Required(ErrorMessage = "Vui lòng điền tên")]
        public string GivenName { get; set; }
        [Display(Name = "Họ")]
        [Required(ErrorMessage = "Vui lòng điền họ")]
        public string SubName { get; set; }
        [Display(Name ="Ngày sinh")]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "Vui lòng điền số điện thoại")]
        [Display(Name = "Số điện thoại")]
        [RegularExpression("^(?!0+$)(\\+\\d{1,3}[- ]?)?(?!0+$)\\d{10,15}$", ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string Phone { get; set; }
        [Display(Name ="Giới tính")]
        public int Gender { get; set; }
        public string LastLogin { get; set; }
        public bool IsApproved { get; set; }
        public string AvtLocation { get; set; }
        [Display(Name ="Tóm tắt")]
        public string Bio { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual UserRole Role { get; set; }
        public virtual CompanyProfile CompanyProfile { get; set; }
        public virtual StudentProfile StudentProfile { get; set; }
    }
}
