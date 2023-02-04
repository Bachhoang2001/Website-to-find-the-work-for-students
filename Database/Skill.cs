using System;
using System.Collections.Generic;

#nullable disable

namespace PBL3.Database
{
    public partial class Skill
    {
        public Skill()
        {
            CompanySkills = new HashSet<CompanySkill>();
            Skillposts = new HashSet<Skillpost>();
            StudentSkills = new HashSet<StudentSkill>();
        }

        public int Id { get; set; }
        public string SkillName { get; set; }
        public int SkillLevel { get; set; }
        public int SkillGroup { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual ICollection<CompanySkill> CompanySkills { get; set; }
        public virtual ICollection<Skillpost> Skillposts { get; set; }
        public virtual ICollection<StudentSkill> StudentSkills { get; set; }
    }
}
