using PBL3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Student
{
    public class PostApplicationModel
    {
        public List<PostSubmit> ListPostSubmit { get; set; }
        public List<City> ListAllCity { get; set; }
        public List<Skill> ListAllSkill { get; set; }
        public PostApplicationModel()
        {
            ListPostSubmit = new List<PostSubmit>();
            ListAllCity = new List<City>();
            ListAllSkill = new List<Skill>();
        }
    }
}
