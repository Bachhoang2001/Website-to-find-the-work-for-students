using PBL3.Database;
using PBL3.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class CompanyAccountManagerModel : SearchModel
    {
        public List<Skill> ListSkills { get; set; }
        public List<City> ListCities { get; set; }
        public  List<CompanyAccountModel> ListAccounts { get; set; }
        public int SelectedCompanyId { get; set; }

    }
}
