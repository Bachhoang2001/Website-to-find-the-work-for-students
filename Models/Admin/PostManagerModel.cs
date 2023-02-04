using PBL3.Database;
using PBL3.Models.Common;
using PBL3.Models.Company;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class PostManagerModel : SearchModel
    {
        public int SelectedPostId { get; set; }
        public List<PostModel> Posts { get; set; }
        public List<Skill> ListSkills { get; set; }
        public List<City> ListCities { get; set; }

        public PostManagerModel()
        {
            CheckCityId = 0;
        } 
        
    }
}
