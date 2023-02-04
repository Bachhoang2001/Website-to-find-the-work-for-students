using PBL3.Database;
using PBL3.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class StudentAccountProfileManagerModel : SearchModel
    {
        public List<StudentProfile> ListStudents { get; set; }
        public int SelectedItem { get; set; }
        public List<Faculty> ListFaculties { get; set; }
        public List<City> ListCities { get; set; }
        public int SelectedFacultyId { get; set; }
        public StudentAccountProfileManagerModel()
        {
            ListStudents = new List<StudentProfile>();
            SelectedFacultyId = 0;
            CheckIsApproved = true;
        }
    }
}
