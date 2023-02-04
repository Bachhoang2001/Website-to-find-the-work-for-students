using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using PBL3.Models.Common;
using PBL3.Models.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models.Home
{
    public class HomeModel
    {
        private PBL3Context _Context;
        public CompanySignUpModel CompanySignUp;
        public IndexModel Index;
        
        public HomeModel()
        {
            _Context = new PBL3Context();
            CompanySignUp = new CompanySignUpModel();
            Index = new IndexModel();
           
        }
        public List<CompanyPost> UpdatePostsSearch()
        {
            List<CompanyPost> result = _Context.CompanyPosts.Where(x => x.IsApproved && !x.Isdeleted).Select(x => x).ToList();
            foreach (var Post in result)
            {
                Post.Company = _Context.CompanyProfiles.Where(x => x.Id == Post.CompanyId && !x.Isdeleted).Select(x => x).FirstOrDefault();
                Post.CityPosts = _Context.CityPosts.Where(x => x.PostId == Post.Id && x.Isdeleted == false).Select(x => x).ToList();
                Post.Skillposts = _Context.Skillposts.Where(x => x.PostId == Post.Id && !x.Isdeleted).Select(x => x).ToList();
                Post.Company.IdNavigation = _Context.Users.Where(x => x.IsApproved && !x.Isdeleted && x.Id == Post.CompanyId).FirstOrDefault();
                foreach (var City in Post.CityPosts)
                {
                    City.City = _Context.Cities.Where(x => x.Id == City.CityId && !x.Isdeleted).Select(x => x).FirstOrDefault();
                }
                foreach (var Skill in Post.Skillposts)
                {
                    Skill.Skill = _Context.Skills.Where(x => x.Id == Skill.SkillId && !x.Isdeleted).Select(x => x).FirstOrDefault();
                }
            }
            // SEARCH ======================================================
            if (Index.CheckCityId != 0)
            {
                result = result.Where(x => x.CityPosts.Where(x => x.CityId == Index.CheckCityId).ToList().Count() > 0).ToList();
            }
            if (Index.SelectedFaculty != 0)
            {
                result = result.Where(x => x.Skillposts.Where(x => x.Skill.SkillGroup == Index.SelectedFaculty).ToList().Count() > 0).Select(x => x).ToList();
            }
            if (Index.CheckString != null)
            {
                string CheckString = Index.CheckString.ToLower();
                result = result.Where(x =>
                         x.Company.CompanyName.ToLower().Contains(CheckString) ||
                         x.Skillposts.Where(x => x.Skill.SkillName.ToLower().Contains(CheckString) ||
                                               CheckString.Contains(x.Skill.SkillName.ToLower())).Count() > 0 ||
                         x.CityPosts.Where(x => x.City.CityName.ToLower() == CheckString ||
                                                CheckString.Contains(x.City.CityName.ToLower())).Count() > 0 ||
                         CheckString.Contains(x.Company.CompanyName.ToLower())
                         ).Select(x => x).ToList();
            }
            result.Reverse();
            return result;
        }
        public List<CompanyProfile> GetTopCompanyProfiles()
        {
            float RateScore = 5, PostScore = 3, SubmitScore = 2;
            List<CompanyProfile> result = _Context.CompanyProfiles.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            foreach (var Company in result)
            {
                int SubmitCount = 0;
                Company.CompanyPosts = _Context.CompanyPosts.Where(x => x.CompanyId == Company.Id && x.IsApproved == true && x.Isdeleted == false).Select(x => x).ToList();
                foreach (var Post in Company.CompanyPosts)
                {
                    Post.PostSubmits = _Context.PostSubmits.Where(x => x.Isdeleted == false && x.PostId == Post.Id).Select(x => x).ToList();
                    SubmitCount += Post.PostSubmits.Count();
                }
                Company.Rate = Company.Rate * RateScore + Company.CompanyPosts.Count() * PostScore + SubmitCount * SubmitScore;
                Company.IdNavigation = _Context.Users.Where(x => x.Id == Company.Id).Select(x => x).FirstOrDefault();
                Company.CompanyCities = _Context.CompanyCities.Where(x => x.CompanyId == Company.Id && x.Isdeleted == false).Select(x => x).ToList();
                foreach (var City in Company.CompanyCities)
                {
                    City.City = _Context.Cities.Where(x => x.Id == City.CityId && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                }
            }
            result = result.Where(x => x.IdNavigation.IsApproved).Select(x => x).ToList();
            result.OrderBy(x => x.Rate);
            if (result.Count() >= 8) return result.Take(8).ToList();
            return result.Take(4).ToList();
        }
        public List<Faculty> GetListFaculties()
        {
            List<Faculty> result = _Context.Faculties.Where(x => x.Isdeleted == false).ToList();
            Faculty NewFaculty = new Faculty()
            {
                Id = 0,
                FacultyName = "Tất cả các Khoa"
            }; result.Add(NewFaculty);
            return result;
        }
        public void AddCompanySignUpToContext()
        {
            CompanySignUp.User.PasswordHash = GetMD5(CompanySignUp.Password);
            _Context.Add(CompanySignUp.User);
            _Context.SaveChanges();
            int CompanyID = _Context.Users.Where(x => x.Email == CompanySignUp.User.Email).Select(x => x.Id).FirstOrDefault();
            CompanySignUp.User.Id = CompanyID;
            CompanySignUp.User.Createby = CompanyID;
            CompanySignUp.User.Updateby = CompanyID;
            CompanySignUp.Company.Id = CompanyID;
            CompanySignUp.Company.Createby = CompanyID;
            CompanySignUp.Company.Updateby = CompanyID;
            _Context.Update(CompanySignUp.User);
            _Context.SaveChanges();
            _Context.Add(CompanySignUp.Company);
            _Context.SaveChanges();

            foreach (var city in CompanySignUp.ListCityId)
            {
                CompanyCity ComCity = new CompanyCity();
                ComCity.CompanyId = CompanyID;
                ComCity.CityId = city;
                ComCity.Createdate = DateTime.Now;
                ComCity.Updatedate = DateTime.Now;
                ComCity.Createby = CompanyID;
                ComCity.Updateby = CompanyID;
                ComCity.Deleteby = 1;
                ComCity.Isdeleted = false;
                _Context.Add(ComCity);
                _Context.SaveChanges();
            }
        }
        public List<City> GetListCities()
        {
            List<City> list = new List<City>();
            list = _Context.Cities.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            list.Add(new City() { Id = 0, CityName = "Tất cả thành phố"});
            return list;
        }
        public bool CheckEmailExist(string email)
        {
            return _Context.Users.Where(x => x.Email.Equals(email)).Select(x => x).Count() > 0;
        }
        public bool CheckPassword(string password1, string password2)
        {
            return password1.Equals(password2);
        }
        public string GetMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            // Compute hash from the bytes of text
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            // Get hash result after compute it
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
    }
}
