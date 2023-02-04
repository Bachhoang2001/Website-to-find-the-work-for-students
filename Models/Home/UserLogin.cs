using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models
{
    [Serializable]
    public class UserLogin
    {
        public int ID { get; set; }
        public string GivenName { get; set; }
        public string SubName { get; set; }
        public int RoleID { get; set; } ///Chưa Làm xongg

    }
    public static class CommonConstraints {
        public static string USER_ID = "USER_ID";
        public static string USER_GIVENNAME = "USER_GIVENNAME";
        public static string USER_SUBNAME = "USER_SUBNAME";
        public static string USER_ROLEID = "USER_ROLEID";
    }
}
