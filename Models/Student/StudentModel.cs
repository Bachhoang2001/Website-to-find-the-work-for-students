using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using PBL3.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models.Student
{
    public class StudentModel
    {
        private PBL3Context _Context;
        public SubmitPostModel _SubmitPost;
        public PostApplicationModel _PostApplication;
        public StudentProfileModel _ProfileModel;
        public AccountModel _AccountModel;
        public int CHANGE_PASSWORD_SUCCESS;
        public int CHECK_INCORRECT_PASSWORD;
        public int OLD_NEW_PASSWORD_ARE_THE_SAME;
        public int NEW_PASSWORD_DOESNOT_MATCH;
        public StudentModel()
        {
            _Context = new PBL3Context();
            _SubmitPost = new SubmitPostModel();
            _PostApplication = new PostApplicationModel();
            CHANGE_PASSWORD_SUCCESS = 1;
            CHECK_INCORRECT_PASSWORD = 2;
            OLD_NEW_PASSWORD_ARE_THE_SAME = 3;
            NEW_PASSWORD_DOESNOT_MATCH = 4;
        }
        public StudentProfileModel GetStudentProfileByID(int UserID, string SearchSkill = "")
        {
            StudentProfileModel result = new StudentProfileModel();
            result.User = _Context.Users.Where(x => x.Id == UserID).Select(x => x).FirstOrDefault();
            result.Student = _Context.StudentProfiles.Where(x => x.Id == UserID).Select(x => x).FirstOrDefault();
            result.ListGender = GetListGender();
            result.SelectedGender = result.User.Gender;
            result.GetAllCity = GetListCities();
            result.Student.StudentSkills = _Context.StudentSkills.Where(x => x.Isdeleted == false && x.StudentId == UserID).Select(x => x).ToList();
            foreach (var studentSkill in result.Student.StudentSkills)
            {
                studentSkill.Skill = _Context.Skills.Where(x => x.Id == studentSkill.SkillId && x.Isdeleted == false).Select(x => x).FirstOrDefault();
            }
            result.ListSkills = _Context.Skills.Where
                (x => x.Isdeleted == false && x.SkillName.ToLower() == SearchSkill.ToLower()).Select(x => x).ToList();
            return result;
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
        public SelectList GetListGender()
        {
            SelectList result = new SelectList(new List<SelectListItem>
            { new SelectListItem { Text = "Nam", Value = "1" },
            new SelectListItem { Text = "Nữ", Value = "2" },
            new SelectListItem { Text = "Không công khai", Value = "0" } }, "Value", "Text");
            return result;
        }
        public PostApplicationModel GetPostApplicationByStudentID(int StudentID)
        {
            PostApplicationModel Model = new PostApplicationModel();
            Model.ListPostSubmit = _Context.PostSubmits.Where(x => x.StudentId == StudentID && x.Isdeleted == false).Select(x => x).ToList();
            if (Model.ListPostSubmit.Count() == 0)
            {
                return Model;
            }
            foreach(var PostSubmit in Model.ListPostSubmit)
            {
                PostSubmit.Post = _Context.CompanyPosts.Where(x => x.Id == PostSubmit.PostId && x.Isdeleted == false && x.IsApproved == true).Select(x => x).FirstOrDefault();
                PostSubmit.Post.Company = _Context.CompanyProfiles.Where(x => x.Id == PostSubmit.Post.CompanyId && x.Isdeleted == false).Select(x => x).FirstOrDefault();
                PostSubmit.Post.Company.IdNavigation = _Context.Users.Where(x => x.Id == PostSubmit.Post.CompanyId && x.Isdeleted == false && x.IsApproved == true).Select(x => x).FirstOrDefault();
                PostSubmit.Post.CityPosts = _Context.CityPosts.Where(x => x.PostId == PostSubmit.Post.Id && x.Isdeleted == false).Select(x => x).ToHashSet();
                PostSubmit.Post.Skillposts = _Context.Skillposts.Where(x => x.PostId == PostSubmit.Post.Id && x.Isdeleted == false).Select(x => x).ToHashSet();
            }
            Model.ListAllSkill = _Context.Skills.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            Model.ListAllCity = _Context.Cities.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            return Model;
        }
        public void UpdateSubmitPostToDB(int StudentID)
        {
            PostSubmit Submit = new PostSubmit();
            Submit.StudentId = StudentID;
            Submit.PostId = _SubmitPost.PostID;
            Submit.ShortText = _SubmitPost.ShortText;
            if (_SubmitPost.IsNewCV)
            {
                Submit.StudentCvpath = _SubmitPost.NewCVPath;
            }
            else
            {
                Submit.StudentCvpath = _SubmitPost.OldCVPath;
            }
            Submit.Createby = StudentID;
            Submit.Createdate = DateTime.Now;
            Submit.Deleteby = 1;
            Submit.Updateby = StudentID;
            Submit.Updatedate = DateTime.Now;
            Submit.Isdeleted = false;
            _Context.Add(Submit);
            _Context.SaveChanges();
        }
        public SubmitPostModel GetDetailSubmitPost(int PostId, int StudentID)
        {
            SubmitPostModel SubmitPost = new SubmitPostModel();
            SubmitPost.PostID = PostId;
            List<StudentProfile> temp = _Context.StudentProfiles.Where(x => x.Isdeleted == false && x.Id == StudentID).Select(x => x).ToList();
            if (temp.FirstOrDefault().CVLocation != null)
            {
                SubmitPost.OldCVPath = temp.FirstOrDefault().CVLocation;
                SubmitPost.CheckOldCVExist = true;
            }
            else
            {
                SubmitPost.OldCVPath = "";
                SubmitPost.CheckOldCVExist = false;
                SubmitPost.IsNewCV = true;
            }
            return SubmitPost;
        }
        public bool NotFoundPostById(int PostId)
        {
            return _Context.CompanyPosts.Where(x => x.Id == PostId && x.Isdeleted.Equals(false) && x.IsApproved.Equals(true)).Select(x => x).Count() != 1;
        }
        public bool CheckRatingCompanyID(int id)
        {
            if (id % 10 > 5) return false;
            id -= id % 10;
            id /= 10;
            if (_Context.Users.Where(x => x.Id == id && x.IsApproved == true && x.Isdeleted == false && x.RoleId == 2).Select(x => x).Count() != 1)
                return false;
            return true;
        }
        public void AddRatingToDBContext(int StudentID, int Code)
        {
            int Rate = Code % 10;
            int CompanyID = (Code - Rate) / 10;
            CompanyFeedback Feedback = _Context.CompanyFeedbacks.Where(x => x.StudentId == StudentID && x.CompanyId == CompanyID).Select(x => x).FirstOrDefault();
            if(Feedback != null)
            {
                Feedback.Rating = Rate;
                Feedback.Updatedate = DateTime.Now;
                Feedback.Updateby = StudentID;
                Feedback.Isdeleted = false;
                _Context.Update(Feedback);
                _Context.SaveChanges();
            }
            else
            {
                Feedback = new CompanyFeedback();
                Feedback.CompanyId = CompanyID;
                Feedback.StudentId = StudentID;
                Feedback.Rating = Rate;
                Feedback.Createdate = DateTime.Now;
                Feedback.Updatedate = DateTime.Now;
                Feedback.Isdeleted = false;
                Feedback.Deleteby = 1;
                Feedback.Createby = StudentID;
                Feedback.Updateby = StudentID;
                _Context.Add(Feedback);
                _Context.SaveChanges();
            }
            CompanyProfile Company = _Context.CompanyProfiles.Where(x => x.Id == CompanyID).Select(x => x).FirstOrDefault();
            List<CompanyFeedback> ListFeedbacks = _Context.CompanyFeedbacks.Where(x => x.CompanyId == CompanyID && x.Isdeleted == false).Select(x => x).ToList();
            float total = 0;
            foreach(var item in ListFeedbacks)
            {
                total += item.Rating;
            }
            float Avg = total / ListFeedbacks.Count();
            Company.Rate = Avg;
            _Context.Update(Company);
            _Context.SaveChanges();
        }
        public void UpdateStudentProfileModelToDB(int ID, int UserID, string ImageFileName, string CVFileName)
        {
            User User = _Context.Users.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            if (ImageFileName != null)
            {
                User.AvtLocation = ImageFileName;
            }
            User.Bio = _ProfileModel.User.Bio;
            User.DateOfBirth = _ProfileModel.User.DateOfBirth;
            User.Gender = _ProfileModel.SelectedGender;
            User.Phone = _ProfileModel.User.Phone;
            User.Updatedate = DateTime.Now;
            User.Updateby = UserID;
            _Context.Update(User);
            _Context.SaveChanges();
            StudentProfile Student = _Context.StudentProfiles.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            if (CVFileName != null)
            {
                Student.CVLocation = CVFileName;
            }
            Student.ForeignLanguage = _ProfileModel.Student.ForeignLanguage;
            Student.City = _ProfileModel.Student.City;
            Student.Updatedate = DateTime.Now;
            Student.Updateby = UserID;
            _Context.Update(Student);
            _Context.SaveChanges();
        }
        public bool AddSkill(int SkillId, int StudentId, int UserId)
        {
            List<StudentSkill> Skill1 = _Context.StudentSkills.Where(x => x.StudentId == StudentId && x.Isdeleted).Select(x => x).ToList();
            if (_Context.StudentSkills.Where(x => x.SkillId == SkillId && x.StudentId == StudentId && x.Isdeleted == false).Select(x => x).ToList().Count() > 0)
            {
                return false;
            }
            StudentSkill Skill = new StudentSkill();
            if (Skill1.Count() == 0)
            {
                Skill = new StudentSkill()
                {
                    StudentId = StudentId,
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
        public void DeleteSkill(int SkillId, int StudentId)
        {
            List<StudentSkill> Skill1 = _Context.StudentSkills.Where(x => x.StudentId == StudentId && x.Isdeleted == false && x.SkillId == SkillId).Select(x => x).ToList();
            StudentSkill Skill = new StudentSkill();
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
        public int CheckAccountModel(int StudentID, AccountModel AccountModel)
        {
            User _User = _Context.Users.Where(x => x.Id == StudentID && x.Isdeleted.Equals(false) && x.IsApproved.Equals(true))
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
        public void UpdateAccountModelToDB(int StudentID)
        {
            User _User = _Context.Users.Where(x => x.Id == StudentID && x.IsApproved.Equals(true) && x.Isdeleted.Equals(false))
                .Select(x => x).FirstOrDefault();
            _User.PasswordHash = GetMD5(_AccountModel.NewPassword);
            _User.Updatedate = DateTime.Now;
            _User.Updateby = StudentID;
            _Context.Update(_User);
            _Context.SaveChanges();
        }
        public bool CheckStudentIdExist(int ID)
        {
            return _Context.Users.Where(x => x.RoleId == 3 && x.Id == ID).Select(x => x).ToList().Count() > 0;
        }
    }
}
