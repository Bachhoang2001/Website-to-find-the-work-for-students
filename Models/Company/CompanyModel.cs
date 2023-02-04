using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using PBL3.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models.Company
{
    public class CompanyModel
    {
        private PBL3Context _Context;
        public AccountModel _AccountModel;
        public CompanyProfileModel _ProfileModel;
        public PostSubmittedModel _PostSubmitted;
        public PostSubmit _DetailPostSubmit;
        public int CHANGE_PASSWORD_SUCCESS;
        public int CHECK_INCORRECT_PASSWORD;
        public int OLD_NEW_PASSWORD_ARE_THE_SAME;
        public int NEW_PASSWORD_DOESNOT_MATCH;
        public CompanyModel()
        {
            _Context = new PBL3Context();
            _AccountModel = new AccountModel();
            _ProfileModel = new CompanyProfileModel();
            _DetailPostSubmit = new PostSubmit();
            CHANGE_PASSWORD_SUCCESS = 1;
            CHECK_INCORRECT_PASSWORD = 2;
            OLD_NEW_PASSWORD_ARE_THE_SAME = 3;
            NEW_PASSWORD_DOESNOT_MATCH = 4;
        }
        public PostSubmittedModel GetPostSubmittedByCompanyID(int CompanyID)
        {
            PostSubmittedModel Model = new PostSubmittedModel();
            Model.ListPosts = _Context.CompanyPosts.Where(x => x.CompanyId == CompanyID && x.Isdeleted == false && x.IsApproved == true).Select(x => x).ToList();
            if (Model.ListPosts.Count() == 0)
            {
                return Model;
            }
            foreach(var Post in Model.ListPosts)
            {
                Post.Company = _Context.CompanyProfiles.Where(x => x.Id == CompanyID && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                Post.Company.IdNavigation = _Context.Users.Where(x => x.Id == CompanyID && x.IsApproved == true && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                Post.CityPosts = _Context.CityPosts.Where(x => x.PostId == Post.Id && x.Isdeleted == false).Select(x => x).ToList();
                Post.Skillposts = _Context.Skillposts.Where(x => x.PostId == Post.Id && x.Isdeleted == false).Select(x => x).ToList();
                Post.PostSubmits = _Context.PostSubmits.Where(x => x.PostId == Post.Id && x.Isdeleted == false).Select(x => x).ToList();
                foreach(var PostSubmitted in Post.PostSubmits)
                {
                    PostSubmitted.PostNavigation = _Context.StudentProfiles.Where(x => x.Id == PostSubmitted.StudentId && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                    PostSubmitted.PostNavigation.IdNavigation = _Context.Users.Where(x => x.Id == PostSubmitted.StudentId && x.IsApproved == true && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                    PostSubmitted.PostNavigation.FacultyNavigation = _Context.Faculties.Where(x => x.Id == PostSubmitted.PostNavigation.Faculty && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                    PostSubmitted.PostNavigation.CityNavigation = _Context.Cities.Where(x => x.Id == PostSubmitted.PostNavigation.City && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                }
                Model.SelectedPostId = Post.Id;
            }
            Model.ListAllSkills = _Context.Skills.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            Model.ListAllCities = _Context.Cities.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            return Model;
        }
        public CompanyProfileModel GetCompanyProfileByID(int CompanyID, string SearchSkill = "")
        {
            CompanyProfileModel CompanyProfile = new CompanyProfileModel();
            CompanyProfile.User = _Context.Users.Where(x => x.Id == CompanyID).Select(x => x).FirstOrDefault();
            CompanyProfile.Company = _Context.CompanyProfiles.Where(x => x.Id == CompanyID).Select(x => x).FirstOrDefault();
            CompanyProfile.ListGender = GetListGender();
            CompanyProfile.SelectedGender = CompanyProfile.User.Gender;
            CompanyProfile.ListCitiesId = (from Company in _Context.CompanyCities
                                           join City in _Context.Cities on Company.CityId equals City.Id
                                           where Company.CompanyId == CompanyID && City.Isdeleted == false && Company.Isdeleted == false
                                           select City.Id).ToList();
            CompanyProfile.GetAllCity = GetListCities();
            CompanyProfile.Company.CompanySkills = _Context.CompanySkills.Where(x => x.Isdeleted == false && x.CompanyId == CompanyID).Select(x => x).ToList();
            foreach(var CompanySkill in CompanyProfile.Company.CompanySkills)
            {
                CompanySkill.Skill = _Context.Skills.Where(x => x.Id == CompanySkill.SkillId && x.Isdeleted == false).Select(x => x).FirstOrDefault();
            }
            CompanyProfile.ListSkills = _Context.Skills.Where
                (x => x.Isdeleted == false && x.SkillName.ToLower() == SearchSkill.ToLower()).Select(x => x).ToList();
            return CompanyProfile;
        }
        public List<SelectListItem> GetListCities()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                List<City> listCityName = (from city in _Context.Cities
                                           where city.Isdeleted == false
                                           select new City
                                           {
                                               Id = city.Id,
                                               CityName = city.CityName
                                           }).ToList();
                foreach (var item in listCityName)
                {
                    list.Add(new SelectListItem { Text = item.CityName, Value = item.Id.ToString() });
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public List<SelectListItem> GetListSkills(int CompanyID)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                List<Skill> listSkillName = (from skill in _Context.Skills
                                             join ComSkill in _Context.CompanySkills on skill.Id equals ComSkill.SkillId
                                             where ComSkill.CompanyId == CompanyID && ComSkill.Isdeleted == false && skill.Isdeleted == false
                                             select new Skill
                                             {
                                                 Id = skill.Id,
                                                 SkillName = skill.SkillName
                                             }).ToList();
                foreach (var item in listSkillName)
                {
                    list.Add(new SelectListItem { Text = item.SkillName, Value = item.Id.ToString() });
                }
            }
            catch (Exception e)
            {
                list.Add(new SelectListItem { Text = "Lỗi rồi", Value = "1" });
            }
            return list;
        }
        public SelectList GetListGender()
        {
            SelectList result = new SelectList(new List<SelectListItem>
            { new SelectListItem { Text = "Nam", Value = "1" },
            new SelectListItem { Text = "Nữ", Value = "2" },
            new SelectListItem { Text = "Không công khai", Value = "0" } }, "Value", "Text");
            return result;
        }
        public List<SelectListItem> GetListCities(int CompanyID)
        {
            List<SelectListItem> list = new List<SelectListItem>();
            try
            {
                List<City> listCityName = (from city in _Context.Cities
                                           join Comcity in _Context.CompanyCities on city.Id equals Comcity.CityId
                                           where Comcity.CompanyId == CompanyID && Comcity.Isdeleted == false && city.Isdeleted == false
                                           select new City
                                           {
                                               Id = city.Id,
                                               CityName = city.CityName
                                           }).ToList();
                foreach (var item in listCityName)
                {
                    list.Add(new SelectListItem { Text = item.CityName, Value = item.Id.ToString() });
                }
            }
            catch (Exception e)
            {

            }
            return list;
        }
        public HomePageModel GetHomePageModelData(int? CompanyID, int? PostId)
        {
            if (CompanyID == null) CompanyID = _Context.CompanyPosts.Where(x => x.Id == PostId).Select(x => x.CompanyId).FirstOrDefault();
            HomePageModel Model = new HomePageModel();
            Model.CompanyID = (int)CompanyID;
            Model.CompanyName = _Context.CompanyProfiles.Where(x => x.Id == CompanyID).Select(x => x.CompanyName).FirstOrDefault();
            Model.AvtLocation = _Context.Users.Where(x => x.Id == CompanyID).Select(x => x.AvtLocation).FirstOrDefault();
            Model.Website = _Context.CompanyProfiles.Where(x => x.Id == CompanyID).Select(x => x.Website).FirstOrDefault();
            Model.Rate = (double)_Context.CompanyProfiles.Where(x => x.Id == CompanyID).Select(x => x.Rate).FirstOrDefault();
            Model.ListCities = (from city in _Context.Cities
                                join Comcity in _Context.CompanyCities on city.Id equals Comcity.CityId
                                where Comcity.CompanyId == CompanyID && Comcity.Isdeleted == false && city.Isdeleted == false
                                select new City
                                {
                                    Id = city.Id,
                                    CityName = city.CityName
                                }).ToList();
            Model.ListSkills = (from skill in _Context.Skills
                                join ComSkill in _Context.CompanySkills on skill.Id equals ComSkill.SkillId
                                where ComSkill.CompanyId == CompanyID && ComSkill.Isdeleted == false && skill.Isdeleted == false
                                select new Skill
                                {
                                    Id = skill.Id,
                                    SkillName = skill.SkillName
                                }).ToList();

            Model.ListPosts = (from ComPost in _Context.CompanyPosts
                               join user in _Context.Users on ComPost.CompanyId equals user.Id
                               where ComPost.CompanyId == CompanyID && ComPost.Isdeleted == false
                               select new PostModel
                               {
                                   CompanyId = ComPost.CompanyId,
                                   Id = ComPost.Id,
                                   Title = ComPost.Title,
                                   Content = ComPost.Content,
                                   MinSalary = (int)ComPost.MinSalary,
                                   MaxSalary = (int)ComPost.MaxSalary,
                                   IsDollar = (bool)ComPost.IsDollar,
                                   IsApproved = ComPost.IsApproved,
                                   CompanyAvtPath = user.AvtLocation,
                                   CreateDate = ComPost.Createdate,
                                   UpdateDate = ComPost.Updatedate,
                                   Createdby = ComPost.Createby,
                                   Updatedby = ComPost.Updateby,
                                   Deletedby = ComPost.Deleteby,
                                   CitiesId = _Context.CityPosts.Where(x => x.PostId == ComPost.Id && !x.Isdeleted).Select(x => x.CityId).ToList(),
                                   SkillsId = _Context.Skillposts.Where(x => x.PostId == ComPost.Id && !x.Isdeleted).Select(x => x.SkillId).ToList(),
                               }).ToList();
            Model.Company.CompanyFeedbacks = _Context.CompanyFeedbacks.Where(x => x.CompanyId == (int)CompanyID && x.Isdeleted == false).Select(x => x).ToList();
            foreach (var Feedback in Model.Company.CompanyFeedbacks)
            {
                Feedback.Student = _Context.StudentProfiles.Where(x => x.Id == Feedback.StudentId).Select(x => x).FirstOrDefault();
                Feedback.Student.IdNavigation = _Context.Users.Where(x => x.Id == Feedback.StudentId).Select(x => x).FirstOrDefault();
                Feedback.Student.FacultyNavigation = _Context.Faculties.Where(x => x.Id == Feedback.Student.Faculty).Select(x => x).FirstOrDefault();
            }
            Model.Company.IdNavigation = _Context.Users.Where(x => x.Id == CompanyID).Select(x => x).FirstOrDefault();
            return Model;
        }
        public PostModel GetPostDetail(int? PostID)
        {
            List<PostModel> post = new List<PostModel>();
            if (PostID != null)
            {
                post = (from ComPost in _Context.CompanyPosts
                        join user in _Context.Users on ComPost.CompanyId equals user.Id
                        where ComPost.Id == PostID && ComPost.Isdeleted == false && user.Isdeleted == false
                        select new PostModel
                        {
                            CompanyId = ComPost.CompanyId,
                            Id = ComPost.Id,
                            Title = ComPost.Title,
                            Content = ComPost.Content,
                            MinSalary = (int)ComPost.MinSalary,
                            MaxSalary = (int)ComPost.MaxSalary,
                            IsDollar = (bool)ComPost.IsDollar,
                            IsApproved = ComPost.IsApproved,
                            CompanyAvtPath = user.AvtLocation,
                            CreateDate = ComPost.Createdate,
                            Createdby = ComPost.Createby,
                            Updatedby = ComPost.Updateby,
                            Deletedby = ComPost.Deleteby,
                            CitiesId = _Context.CityPosts.Where(x => x.PostId == ComPost.Id).Select(x => x.CityId).ToList(),
                            SkillsId = _Context.Skillposts.Where(x => x.PostId == ComPost.Id).Select(x => x.SkillId).ToList(),
                        }).ToList();
            }
            return post.FirstOrDefault();
        }
        public bool PostIDExists(int PostId)
        {
            return _Context.CompanyPosts.Any(x => x.Id == PostId);
        }
        public bool CheckCompanyIDExists(int CompanyId)
        {
            return _Context.CompanyProfiles.Any(x => x.Id == CompanyId);
        }
        public string GetMD5(string password)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(password));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public int CheckAccountModel(int CompanyID, AccountModel AccountModel)
        {
            User _User = _Context.Users.Where(x => x.Id == CompanyID && x.Isdeleted.Equals(false) && x.IsApproved.Equals(true))
                .Select(x => x).FirstOrDefault();
            if (!GetMD5(AccountModel.Password).Equals(_User.PasswordHash))
            {
                return CHECK_INCORRECT_PASSWORD;//Mật khẩu cũ không chính xác
            }
            else if (AccountModel.Password == AccountModel.NewPassword)
            {
                return OLD_NEW_PASSWORD_ARE_THE_SAME;//Mật khẩu mới không trùng khớp
            }
            else if (AccountModel.NewPassword2 != AccountModel.NewPassword)
            {
                return NEW_PASSWORD_DOESNOT_MATCH;
            }
            return CHANGE_PASSWORD_SUCCESS;
        }
        public void UpdateAccountModelToDB(int CompanyID)
        {
            User _User = _Context.Users.Where(x => x.Id == CompanyID && x.IsApproved.Equals(true) && x.Isdeleted.Equals(false))
                .Select(x => x).FirstOrDefault();
            _User.PasswordHash = GetMD5(_AccountModel.NewPassword);
            _User.Updatedate = DateTime.Now;
            _User.Updateby = CompanyID;
            _Context.Update(_User);
            _Context.SaveChanges();
        }
        public void UpdateCompanyProfileModelToDB(int ID,int UserID, string FileName)
        {
            User User = _Context.Users.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            if (FileName != null)
            {
                User.AvtLocation = FileName;
            }
            User.Email = _ProfileModel.User.Email;
            User.Bio = _ProfileModel.User.Bio;
            User.DateOfBirth = _ProfileModel.User.DateOfBirth;
            User.Gender = _ProfileModel.SelectedGender;
            User.GivenName = _ProfileModel.User.GivenName;
            User.SubName = _ProfileModel.User.SubName;
            User.Phone = _ProfileModel.User.Phone;
            User.Updatedate = DateTime.Now;
            User.Updateby = UserID;
            _Context.Update(User);
            _Context.SaveChanges();
            CompanyProfile Company = _Context.CompanyProfiles.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            Company.CompanyName = _ProfileModel.Company.CompanyName;
            Company.Website = _ProfileModel.Company.Website;
            Company.Updatedate = DateTime.Now;
            Company.Updateby = UserID;
            _Context.Update(Company);
            _Context.SaveChanges();
            //=========================================
            List<CompanyCity> PrevCities = new List<CompanyCity>();
            PrevCities = _Context.CompanyCities.Where(x => x.CompanyId == ID).Select(x => x).ToList();
            int PrevCityCount = PrevCities.Count();
            List<int> NextCities = new List<int>();
            NextCities = _ProfileModel.ListCitiesId;
            int NextCityCount = NextCities.Count();
            if (PrevCityCount == NextCityCount)
            {
                foreach (var item in NextCities)
                {
                    CompanyCity CompanyCity = PrevCities.First();
                    PrevCities.RemoveAt(0);
                    CompanyCity.CityId = item;
                    CompanyCity.CompanyId = ID;
                    CompanyCity.Updatedate = DateTime.Now;
                    CompanyCity.Updateby = UserID;
                    CompanyCity.Isdeleted = false;
                    _Context.Update(CompanyCity);
                    _Context.SaveChanges();
                }
            }
            else if (PrevCityCount > NextCityCount)
            {
                foreach (var item in NextCities)
                {
                    CompanyCity CompanyCity = PrevCities.First();
                    PrevCities.RemoveAt(0);
                    CompanyCity.CityId = item;
                    CompanyCity.CompanyId = ID;
                    CompanyCity.Updatedate = DateTime.Now;
                    CompanyCity.Updateby = UserID;
                    CompanyCity.Isdeleted = false;
                    _Context.Update(CompanyCity);
                    _Context.SaveChanges();
                }
                for (int i = 0; i < PrevCityCount - NextCityCount; i++)
                {
                    CompanyCity CompanyCity = PrevCities.First();
                    PrevCities.RemoveAt(0);
                    if (!CompanyCity.Isdeleted)
                    {
                        CompanyCity.Isdeleted = true;
                        CompanyCity.Deleteby = UserID;
                    }
                    _Context.Update(CompanyCity);
                    _Context.SaveChanges();
                }
            }
            else
            {
                foreach (var CompanyCity in PrevCities)
                {
                    int item = NextCities.First();
                    NextCities.RemoveAt(0);
                    CompanyCity.CityId = item;
                    CompanyCity.CompanyId = ID;
                    CompanyCity.Updatedate = DateTime.Now;
                    CompanyCity.Updateby = UserID;
                    CompanyCity.Isdeleted = false;
                    _Context.Update(CompanyCity);
                    _Context.SaveChanges();
                }
                for (int i = 0; i < NextCityCount - PrevCityCount; i++)
                {
                    CompanyCity CompanyCity = new CompanyCity();
                    int item = NextCities.First();
                    NextCities.RemoveAt(0);
                    CompanyCity.CityId = item;
                    CompanyCity.CompanyId = ID;
                    CompanyCity.Createdate = DateTime.Now;
                    CompanyCity.Updatedate = DateTime.Now;
                    CompanyCity.Createby = UserID;
                    CompanyCity.Updateby = UserID;
                    CompanyCity.Deleteby = 0;
                    CompanyCity.Isdeleted = false;
                    _Context.Update(CompanyCity);
                    _Context.SaveChanges();
                }
            }
        }
        public bool AddSkill(int SkillId, int CompanyId, int UserId)
        {
            List<CompanySkill> Skill1 = _Context.CompanySkills.Where(x => x.CompanyId == CompanyId && x.Isdeleted).Select(x => x).ToList();
            if(_Context.CompanySkills.Where(x=>x.SkillId == SkillId && x.CompanyId == CompanyId && x.Isdeleted == false).Select(x => x).ToList().Count() > 0)
            {
                return false;
            }
            CompanySkill Skill = new CompanySkill();
            if (Skill1.Count() == 0)
            {
                Skill = new CompanySkill()
                {
                    CompanyId = CompanyId,
                    SkillId = SkillId,
                    Createby = UserId,
                    Updateby = UserId,
                    Deleteby = 1,
                    Createdate = DateTime.Now,
                    Updatedate = DateTime.Now,
                    Isdeleted = false,
                };
                _Context.Add(Skill);
                _Context.SaveChanges();
            }
            else
            {
                Skill = Skill1.First();
                Skill.SkillId = SkillId;
                Skill.Updatedate = DateTime.Now;
                Skill.Updateby = UserId;
                Skill.Isdeleted = false;
                _Context.Update(Skill);
                _Context.SaveChanges();
            }
            return true;
        }
        public void DeleteSkill(int SkillId, int CompanyId)
        {
            List<CompanySkill> Skill1 = _Context.CompanySkills.Where(x => x.CompanyId == CompanyId && x.Isdeleted==false && x.SkillId == SkillId).Select(x => x).ToList();
            CompanySkill Skill = new CompanySkill();
            if (Skill1.Count() == 0)
            {
            }
            else
            {
                Skill = Skill1.First();
                Skill.Isdeleted = true;
            }
            _Context.Update(Skill);
            _Context.SaveChanges();
        }
        public string GetSkillById(int SkillId)
        {
            return _Context.Skills.Where(x => x.Id == SkillId && x.Isdeleted == false).Select(x => x.SkillName).FirstOrDefault();
        }
        public CompanyProfile GetCompanyProfileByPostId(int PostId)
        {
            CompanyPost Post = _Context.CompanyPosts.Where(x => x.Id == PostId).Select(x => x).FirstOrDefault();
            return _Context.CompanyProfiles.Where(x => x.Id == Post.CompanyId).Select(x => x).FirstOrDefault();
        }
    }
}
