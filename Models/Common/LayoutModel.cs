using PBL3.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Common
{
    public class LayoutModel
    {
        List<string> ListSkills { get; set; }
        List<string> ListLevels { get; set; }
        List<string> ListCities { get; set; }
        List<string> ListCompanies { get; set; }
        public LayoutModel()
        {
            ListLevels = new List<string>();
            ListSkills = new List<string>();
            ListCities = new List<string>();
            ListCompanies = new List<string>();
        }
        /// <summary>
        /// Lấy danh sách top kĩ năng, cấp bậc được sử dụng nhiều nhất theo tham số
        /// </summary>
        /// <param name="Level">Cấp độ 1: Kĩ năng, ví dụ: C#, Backend. Cấp độ 2: Cấp bậc</param>
        /// <returns></returns>
        public List<string> GetTopSkill(int Level)
        {
            PBL3Context Context = new PBL3Context();
            List<string> result = new List<string>();
            List<Skill> Temp1 = Context.Skills.Where(x => x.Isdeleted == false && x.SkillLevel == Level).ToList();
            foreach(var Skill in Temp1)
            {
                Skill.Skillposts = Context.Skillposts.Where(x => x.Isdeleted == false && x.SkillId == Skill.Id).ToList();
            }
            Temp1.OrderBy(x => x.Skillposts.Count());
            result = Temp1.Select(x => x.SkillName).ToList();
            if (result.Count() > 20) return result.Take(20).ToList();
            return result;
        }
    }
}
