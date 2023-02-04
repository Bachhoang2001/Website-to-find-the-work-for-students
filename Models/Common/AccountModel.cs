using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models.Common
{
    public class AccountModel
    {
        [Required(ErrorMessage = "Vui lòng điền mật khẩu hiện tại")]
        [Display(Name = "Nhập mật khẩu cũ")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Vui lòng điền mật khẩu mới")]
        [Display(Name = "Nhập mật khẩu mới")]
        [StringLength(255, ErrorMessage = "Mật khẩu tối thiểu 8 kí tự.", MinimumLength = 8)]
        [RegularExpression("((?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[@#$%!&*~^]).{0,})", ErrorMessage = "Mật khẩu tối thiểu 1 chữ số, 1 chữ cái thường, 1 chữ cái hoa, 1 kí tự đặc biệt.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "Vui lòng điền lại mật khẩu")]
        [Display(Name = "Nhập lại mật khẩu mới")]
        [Compare("NewPassword",ErrorMessage ="Mật khẩu mới không trùng khớp")]
        public string NewPassword2 { get; set; }
        public AccountModel()
        {
        }
        public string GetMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
