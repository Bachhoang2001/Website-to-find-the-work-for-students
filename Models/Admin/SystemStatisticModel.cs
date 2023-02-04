using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class SystemStatisticModel
    {
        public List<string> Labels { get; set; }
        public List<int> Values { get; set; }
        public List<int> Values2 { get; set; }
        public List<int> Values3 { get; set; }
        public SelectList Years { get; set; }
        public SelectList Months { get; set; }
        public int SelectedMonth { get; set; }
        public int SelectedYear { get; set; }
        public SystemStatisticModel()
        {
            Labels = new List<string>();
            Values = new List<int>();
            Values2 = new List<int>();
            Values3 = new List<int>();
            Years = new SelectList(new List<SelectListItem>());
            Months = new SelectList(new List<SelectListItem>());
            SelectedMonth = 0;
            SelectedYear = 0;
        }
    }
}
