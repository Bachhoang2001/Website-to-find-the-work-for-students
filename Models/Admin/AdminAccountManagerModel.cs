using PBL3.Database;
using PBL3.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class AdminAccountManagerModel : SearchModel
    {
        public List<User> ListAccounts { get; set; }
        public int SelectedAdminId { get; set; }
        public AdminAccountManagerModel()
        {
            ListAccounts = new List<User>();
        }
    }
}
