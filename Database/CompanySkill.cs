using System;
using System.Collections.Generic;

#nullable disable

namespace PBL3.Database
{
    public partial class CompanySkill
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int SkillId { get; set; }
        public DateTime Createdate { get; set; }
        public DateTime Updatedate { get; set; }
        public int Createby { get; set; }
        public int Updateby { get; set; }
        public int Deleteby { get; set; }
        public bool Isdeleted { get; set; }

        public virtual CompanyProfile Company { get; set; }
        public virtual Skill Skill { get; set; }
    }
}
