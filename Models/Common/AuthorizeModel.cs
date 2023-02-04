using PBL3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Common
{
    public class AuthorizeModel
    {
        private PBL3Context _Context;
        public AuthorizeModel() {
            _Context = new PBL3Context();
        }
        public bool CheckRoleNameByID(string RoleName1, string RoleName2, string RoleName3,string RoleName4, int ID)
        {
            User User = _Context.Users.Where(x => x.Id == ID && x.IsApproved == true && x.Isdeleted == false).Select(x => x).First();
            if (User == null) return false;
            User.Role = _Context.UserRoles.Where(x => x.Id == User.RoleId && x.Isdeleted == false).Select(x => x).FirstOrDefault();
            return User.Role.RoleName == RoleName1 || User.Role.RoleName == RoleName2 || User.Role.RoleName == RoleName3 || User.Role.RoleName == RoleName4;
        }
    }
}
