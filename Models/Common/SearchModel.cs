using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Common
{
    public class SearchModel
    {
        [Display(Name="Đã xóa")]
        public bool CheckIsDeleted { get; set; }
        [Display(Name = "Đã duyệt")]
        public bool CheckIsApproved { get; set; }
        public int CheckCityId { get; set; }
        public List<int> CheckListSkillsId { get; set; }
        public int CheckYear { get; set; }
        public string CheckString { get; set; }
        public SearchModel()
        {
            CheckIsApproved = false; CheckIsDeleted = false;
        }
    }
}
