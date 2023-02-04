using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Company
{
    public class HomePageModel
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string AvtLocation { get; set; }
        public List<City> ListCities { get; set; }
        public List<Skill> ListSkills { get; set; }
        public List<PostModel> ListPosts { get; set; }
        public string Website { get; set; }
        public double Rate { get; set; }
        public CompanyProfile Company { get; set; }
        public int CurrentPage { get; set; }
        public int MaxPerPage { get; set; }
        public HomePageModel()
        {
            Company = new CompanyProfile();
            MaxPerPage = 5;
            CurrentPage = 1;
        }
    }
}
