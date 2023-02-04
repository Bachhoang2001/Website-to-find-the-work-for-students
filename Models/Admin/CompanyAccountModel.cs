using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class CompanyAccountModel
    {
        public User User { get; set; }
        public CompanyProfile Company { get; set; }
        public List<int> ListSkillsId { get; set; }
        public List<int> ListCitiesId { get; set; }
        public CompanyAccountModel()
        {
            User = new User();
            Company = new CompanyProfile();
        }
    }
}
