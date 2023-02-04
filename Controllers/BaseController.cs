using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3.Models;
using PBL3.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Controllers
{
    public class BaseController : Controller
    {
        public LayoutModel _LayoutModel = new LayoutModel();
        public bool AuthorizeUser(string RoleName1, string RoleName2 = "", string RoleName3 = "", string RoleName4 = "")
        {
            int ID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (ID == 0) return false;
            AuthorizeModel Model = new AuthorizeModel();
            return Model.CheckRoleNameByID(RoleName1,RoleName2, RoleName3,RoleName4, ID);
        }
    }
}
