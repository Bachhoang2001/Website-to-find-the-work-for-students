using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models
{
    public class LoginModel
    {
        private PBL3Context _Context;

        [Required(ErrorMessage ="Vui lòng điền Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Vui lòng điền mật khẩu")]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        public LoginModel()
        {
            _Context = new PBL3Context();
        }
        public string GetMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            // Compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            // Get hash result after compute it
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public UserLogin GetSession(string email)
        {
            IEnumerable<UserLogin> result = from user in _Context.Users
                                            where user.Isdeleted == false && user.Email.Equals(email)
                                            select new UserLogin
                                            {
                                                ID = user.Id,
                                                GivenName = user.GivenName,
                                                SubName = user.SubName,
                                                RoleID = user.RoleId
                                            };
            return result.FirstOrDefault();
        }
        public bool CheckEmailPassword(LoginModel user)
        {
            var f_password = GetMD5(user.Password);
            return _Context.Users.Where(s => s.Email.Equals(user.Email) && s.PasswordHash.Equals(f_password) && s.Isdeleted.Equals(false) && s.IsApproved.Equals(true)).ToList().Count() > 0;
        }
    }
}
