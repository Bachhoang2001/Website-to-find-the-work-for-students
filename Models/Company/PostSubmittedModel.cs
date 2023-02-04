using PBL3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Company
{
    public class PostSubmittedModel
    {
        public List<CompanyPost> ListPosts { get; set; }
        public List<City> ListAllCities { get; set; }
        public List<Skill> ListAllSkills { get; set; }
        public int SelectedPostId { get; set; }
        public PostSubmittedModel()
        {
            ListPosts = new List<CompanyPost>();
            ListAllCities = new List<City>();
            ListAllSkills = new List<Skill>();
        }
    }
}
