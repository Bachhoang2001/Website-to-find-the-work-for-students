using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class StudentProfileModel
    {
        public string STT { get; set; }
        public string MSSV { get; set; }
        public string Email { get; set; }
        public string Password{ get; set; }
        public string SurName { get; set; }
        public string GivenName { get; set; }
        public string DOB { get; set; }
        public string FacultyName { get; set; }
        public string CityName { get; set; }
        public string Phone { get; set; }
        public string Gender { get; set; }
    }
}
