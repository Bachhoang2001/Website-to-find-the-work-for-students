using ExcelDataReader;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using PBL3.Models.Common;
using PBL3.Models.Company;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace PBL3.Models.Admin
{
    public class AdminModel
    {
        private PBL3Context _Context;
        public PostManagerModel PostManager;
        public CompanyAccountManagerModel CompanyAccountManager;
        public List<StudentProfileModel> ListStudents;
        public StudentAccountProfileManagerModel StudentAccountProfileManager;
        public AdminProfileModel ProfileModel;
        public AccountModel AccountModel;
        public AdminAccountManagerModel AdminAccountManager;
        public SystemStatisticModel SystemStatistic;
        public int CHANGE_PASSWORD_SUCCESS;
        public int CHECK_INCORRECT_PASSWORD;
        public int OLD_NEW_PASSWORD_ARE_THE_SAME;
        public int NEW_PASSWORD_DOESNOT_MATCH;
        public AdminModel()
        {
            _Context = new PBL3Context();
            PostManager = new PostManagerModel();
            SystemStatistic = new SystemStatisticModel();
            CompanyAccountManager = new CompanyAccountManagerModel();
            ListStudents = new List<StudentProfileModel>();
            StudentAccountProfileManager = new StudentAccountProfileManagerModel();
            ProfileModel = new AdminProfileModel();
            this.AccountModel = new AccountModel();
            AdminAccountManager = new AdminAccountManagerModel();
            CHANGE_PASSWORD_SUCCESS = 1;
            CHECK_INCORRECT_PASSWORD = 2;
            OLD_NEW_PASSWORD_ARE_THE_SAME = 3;
            NEW_PASSWORD_DOESNOT_MATCH = 4;
        }
        public AdminProfileModel GetAdminProfileByID(int UserID)
        {
            AdminProfileModel result = new AdminProfileModel();
            result.User = _Context.Users.Where(x => x.Id == UserID).Select(x => x).FirstOrDefault();
            result.ListGender = GetListGender();
            result.SelectedGender = result.User.Gender;
            return result;
        }
        public SelectList GetListGender()
        {
            SelectList result = new SelectList(new List<SelectListItem>
            { new SelectListItem { Text = "Nam", Value = "1" },
            new SelectListItem { Text = "Nữ", Value = "2" },
            new SelectListItem { Text = "Không công khai", Value = "0" } }, "Value", "Text");
            return result;
        }
        public List<City> GetAllCities()
        {
            City Allcity = new City();
            Allcity.Id = 0;
            Allcity.CityName = "Tất cả thành phố";
            Allcity.Isdeleted = false;
            List<City> List = _Context.Cities.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            List.Add(Allcity);
            return List;
        }
        public List<Skill> GetAllSkills()
        {
            return _Context.Skills.Where(x => x.Isdeleted == false).Select(x => x).ToList();
        }
        public List<Faculty> GetAllFaculties()
        {
            List<Faculty> list = _Context.Faculties.Where(x => x.Isdeleted == false).Select(x => x).ToList();
            Faculty newField = new Faculty()
            {
                Id = 0,
                FacultyName = "Tất cả Khoa",
                Isdeleted = false
            };
            list.Add(newField);
            return list;
        }
        public List<int> GetAllCompaniesId()
        {
            return _Context.CompanyProfiles.Where(x => x.Isdeleted == false).Select(x => x.Id).ToList();
        }
        public List<PostModel> GetUpdatePostsSearch()
        {
            List<PostModel> List = (from ComPost in _Context.CompanyPosts
                                    join user in _Context.Users on ComPost.CompanyId equals user.Id
                                    where ComPost.Isdeleted == PostManager.CheckIsDeleted
                                    && ComPost.IsApproved == PostManager.CheckIsApproved
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
                                        IsDelete = ComPost.Isdeleted,
                                        CitiesId = _Context.CityPosts.Where(x => x.PostId == ComPost.Id && !x.Isdeleted).Select(x => x.CityId).ToList(),
                                        SkillsId = _Context.Skillposts.Where(x => x.PostId == ComPost.Id && !x.Isdeleted).Select(x => x.SkillId).ToList(),
                                    }).ToList();
            //Sử dụng foreach để tìm kiếm về Skill City Ngày tháng....
            if (PostManager.CheckCityId != 0)
            {
                List<PostModel> List1 = (from post in List
                                         where post.CitiesId.Contains(PostManager.CheckCityId)
                                         select post).ToList();
                List = List1;
            }
            if (PostManager.CheckString != null)
            {
                List<Skill> ListKills = (from skill in _Context.Skills
                                         select new Skill
                                         {
                                             Id = skill.Id,
                                             SkillName = skill.SkillName.ToUpper(),
                                             Isdeleted = skill.Isdeleted
                                         }).ToList();
                List<CompanyProfile> ListCompanies = (from company in _Context.CompanyProfiles
                                                      select new CompanyProfile
                                                      {
                                                          Id = company.Id,
                                                          CompanyName = company.CompanyName.ToUpper(),
                                                          Isdeleted = company.Isdeleted
                                                      }).ToList();
                List<int> ListSkillIdFound = (from skill in ListKills
                                              where skill.SkillName.Contains(PostManager.CheckString.ToUpper()) && skill.Isdeleted == false
                                              select skill.Id).ToList();
                List<int> ListCompanyIdFound = (from company in ListCompanies
                                                where company.CompanyName.Contains(PostManager.CheckString.ToUpper()) && company.Isdeleted == false
                                                select company.Id).ToList();
                List<PostModel> List1 = new List<PostModel>();
                foreach (var post in List)
                {
                    string temp = post.Title.ToUpper();
                    if (temp.Contains(PostManager.CheckString.ToUpper()) || ListCompanyIdFound.Contains(post.CompanyId))
                    {
                        List1.Add(post);
                        continue;
                    }
                    else
                    {
                        foreach (var SkillId in ListSkillIdFound)
                        {
                            if (post.SkillsId.Contains(SkillId))
                            {
                                List1.Add(post);
                                continue;
                            }
                        }
                    }
                }
                return List1;
            }
            return List;
        }
        public void PostApproval(int id)
        {
            CompanyPost Post = _Context.CompanyPosts.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            Post.IsApproved = true;
            _Context.Update(Post);
            _Context.SaveChanges();
        }
        public void CancelPostApproval(int id, int AdminID)
        {
            CompanyPost Post = _Context.CompanyPosts.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            Post.IsApproved = false;
            _Context.Update(Post);
            _Context.SaveChanges();
            Post.PostSubmits = _Context.PostSubmits.Where(x => x.PostId == Post.Id && !x.Isdeleted).Select(x => x).ToList();
            foreach (var submit in Post.PostSubmits)
            {
                submit.Isdeleted = true;
                submit.Deleteby = AdminID;
                _Context.Update(submit); _Context.SaveChanges();
            }
        }
        public List<CompanyAccountModel> GetUpdateCompanyAccountsSearch()
        {
            List<CompanyAccountModel> List = (from user in _Context.Users
                                              join company in _Context.CompanyProfiles on user.Id equals company.Id
                                              where user.IsApproved == CompanyAccountManager.CheckIsApproved
                                              && user.Isdeleted == CompanyAccountManager.CheckIsDeleted
                                              select new CompanyAccountModel
                                              {
                                                  User = user,
                                                  Company = company,
                                                  ListCitiesId = _Context.CompanyCities.Where(x => x.CompanyId == company.Id && x.Isdeleted == false).Select(x => x.CityId).ToList(),
                                                  ListSkillsId = _Context.CompanySkills.Where(x => x.CompanyId == company.Id && x.Isdeleted == false).Select(x => x.SkillId).ToList()

                                              }).ToList();
            if (CompanyAccountManager.CheckCityId != 0)
            {
                List<CompanyAccountModel> List1 = (from company in List
                                                   where company.ListCitiesId.Contains(CompanyAccountManager.CheckCityId)
                                                   select company).ToList();
                List = List1;
            }
            if (CompanyAccountManager.CheckString != null)
            {
                List<Skill> ListKills = (from skill in _Context.Skills
                                         select new Skill
                                         {
                                             Id = skill.Id,
                                             SkillName = skill.SkillName.ToUpper(),
                                             Isdeleted = skill.Isdeleted
                                         }).ToList();
                List<CompanyProfile> ListCompanies = (from company in _Context.CompanyProfiles
                                                      select new CompanyProfile
                                                      {
                                                          Id = company.Id,
                                                          CompanyName = company.CompanyName.ToUpper(),
                                                          Isdeleted = company.Isdeleted
                                                      }).ToList();
                List<int> ListSkillIdFound = (from skill in ListKills
                                              where skill.SkillName.Contains(CompanyAccountManager.CheckString.ToUpper()) && skill.Isdeleted == false
                                              select skill.Id).ToList();
                List<int> ListCompanyIdFound = (from company in ListCompanies
                                                where company.CompanyName.Contains(CompanyAccountManager.CheckString.ToUpper()) && company.Isdeleted == false
                                                select company.Id).ToList();
                List<CompanyAccountModel> List1 = new List<CompanyAccountModel>();
                foreach (var company in List)
                {
                    if (ListCompanyIdFound.Contains(company.User.Id))
                    {
                        List1.Add(company);
                        continue;
                    }
                    else
                    {
                        foreach (var SkillId in ListSkillIdFound)
                        {
                            if (company.ListSkillsId.Contains(SkillId))
                            {
                                List1.Add(company);
                                continue;
                            }
                        }
                    }
                }
                return List1;
            }
            return List;
        }
        public void CompanyAccountApproval(int id)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.IsApproved = true;
            _Context.Update(User);
            _Context.SaveChanges();
        }
        public void CancelCompanyAccountApproval(int id)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.IsApproved = false;
            _Context.Update(User);
            _Context.SaveChanges();
        }
        public void CompanyAccountDelete(int id, int AdminID)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.Isdeleted = true;
            User.Deleteby = AdminID;
            _Context.Update(User);
            _Context.SaveChanges();
            CompanyProfile Company = _Context.CompanyProfiles.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            Company.Isdeleted = true;
            Company.Deleteby = AdminID;
            _Context.Update(Company);
            _Context.SaveChanges();

            Company.CompanyPosts = _Context.CompanyPosts.Where(x => x.CompanyId == Company.Id && !x.Isdeleted).Select(x => x).ToList();
            foreach (var Post in Company.CompanyPosts)
            {
                Post.Isdeleted = true;
                Post.IsApproved = false;
                Post.Deleteby = AdminID;
                _Context.Update(Post); _Context.SaveChanges();
            }

            Company.CompanyCities = _Context.CompanyCities.Where(x => x.CompanyId == Company.Id && !x.Isdeleted).Select(x => x).ToList();
            foreach (var City in Company.CompanyCities)
            {
                City.Isdeleted = true;
                City.Deleteby = AdminID;
                _Context.Update(City); _Context.SaveChanges();
            }

            Company.CompanySkills = _Context.CompanySkills.Where(x => x.CompanyId == Company.Id && !x.Isdeleted).Select(x => x).ToList();
            foreach (var Skill in Company.CompanySkills)
            {
                Skill.Isdeleted = true;
                Skill.Deleteby = AdminID;
                _Context.Update(Skill); _Context.SaveChanges();
            }
        }
        public void CancelCompanyAccountDelete(int id)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.Isdeleted = false;
            _Context.Update(User);
            _Context.SaveChanges();
            CompanyProfile Company = _Context.CompanyProfiles.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            Company.Isdeleted = false;
            _Context.Update(Company);
            _Context.SaveChanges();
        }
        public List<StudentProfileModel> GetListStudent(string FileName)
        {
            List<StudentProfileModel> students = new List<StudentProfileModel>();
            var Path = $"{ Directory.GetCurrentDirectory()}{@"\wwwroot\files"}" + "\\" + FileName;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = System.IO.File.Open(Path, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    reader.Read();
                    while (reader.Read())
                    {
                        students.Add(new StudentProfileModel()
                        {
                            STT = reader.GetValue(0).ToString(),
                            MSSV = reader.GetValue(1).ToString(),
                            Email = reader.GetValue(2).ToString(),
                            Password = reader.GetValue(3).ToString(),
                            SurName = reader.GetValue(4).ToString(),
                            GivenName = reader.GetValue(5).ToString(),
                            DOB = reader.GetValue(6).ToString(),
                            FacultyName = reader.GetValue(7).ToString(),
                            CityName = reader.GetValue(8).ToString(),
                            Phone = reader.GetValue(9).ToString(),
                            Gender = reader.GetValue(10).ToString()
                        });
                    }
                }
            }
            return students;
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
        public void AddStudentAccountsToDB(int AdminID)
        {
            foreach (var student in ListStudents)
            {
                User _User = new User()
                {
                    Email = student.Email,
                    PasswordHash = GetMD5(student.Password),
                    SubName = student.SurName,
                    GivenName = student.GivenName,
                    DateOfBirth = DateTime.Parse(student.DOB),
                    Phone = student.Phone,
                    Gender = student.Gender.ToLower() == "nam" ? 1 : student.Gender.ToLower() == "nữ" ? 2 : 0,
                    RoleId = 3,
                    IsApproved = true,
                    AvtLocation = "anonymous.png",
                    Createdate = DateTime.Now,
                    Updatedate = DateTime.Now,
                    Createby = AdminID,
                    Updateby = AdminID,
                    Deleteby = 1,
                    Isdeleted = false,
                };
                _Context.Add(_User); _Context.SaveChanges();
                StudentProfile Student = new StudentProfile()
                {
                    Id = _Context.Users.Where(x => x.Email == student.Email).Select(x => x.Id).FirstOrDefault(),
                    Mssv = Convert.ToInt32(student.MSSV),
                    Faculty = _Context.Faculties.Where(x => x.FacultyName.ToLower().Equals(student.FacultyName.ToLower())).Select(x => x.Id).FirstOrDefault(),
                    City = _Context.Cities.Where(x => x.CityName.ToLower().Equals(student.CityName.ToLower())).Select(x => x.Id).FirstOrDefault(),
                    ForeignLanguage = "TOEIC",
                    Gpa = 0,
                    Createdate = DateTime.Now,
                    Updatedate = DateTime.Now,
                    Createby = AdminID,
                    Updateby = AdminID,
                    Deleteby = 1,
                    Isdeleted = false
                };
                _Context.Add(Student); _Context.SaveChanges();
            }
        }
        public List<StudentProfileModel> GetListStudentInvalid()
        {
            List<StudentProfileModel> students = new List<StudentProfileModel>();
            List<StudentProfileModel> _ListStudents = ListStudents;
            bool IsChanged = false;

            foreach (var student in _ListStudents)
            {
                if (_Context.Users.Where(
                    x => x.Email.ToLower() == student.Email.ToLower()
                    ).Select(x => x).Count() > 0)
                {
                    student.Email = "Email đã tồn tại";
                    IsChanged = true;
                }
                if (_Context.StudentProfiles.Where(x => x.Mssv == Convert.ToInt32(student.MSSV)).Select(x => x).Count() > 0)
                {
                    student.MSSV = "MSSV đã tồn tại";
                    IsChanged = true;
                }
                if (_Context.Faculties.Where(x =>
                         x.FacultyName.ToLower() == student.FacultyName.ToLower() && x.Isdeleted == false
                        ).Select(x => x).Count() == 0)
                {
                    student.FacultyName = "Không tồn tại";
                    IsChanged = true;
                }
                if (_Context.Cities.Where(x =>
                         x.CityName.ToLower() == student.CityName.ToLower() && x.Isdeleted == false
                        ).Select(x => x).Count() == 0)
                {
                    student.CityName = "Không tồn tại";
                    IsChanged = true;
                }
                if (IsChanged)
                {
                    students.Add(student);
                    IsChanged = false;
                }
            }
            return students;
        }
        public List<StudentProfile> GetUpdateStudentAccountProfileSearch()
        {
            List<StudentProfile> Students = new List<StudentProfile>();
            Students = _Context.StudentProfiles.Where(x =>
                        x.Isdeleted == StudentAccountProfileManager.CheckIsDeleted
                        ).Select(x => x).ToList();
            foreach (var Student in Students)
            {
                Student.IdNavigation = _Context.Users.Where(x => x.Id == Student.Id).Select(x => x).FirstOrDefault();
                Student.CityNavigation = _Context.Cities.Where(x => x.Id == Student.City && !x.Isdeleted).Select(x => x).FirstOrDefault();
                Student.StudentSkills = _Context.StudentSkills.Where(x => x.StudentId == Student.Id && !x.Isdeleted).Select(x => x).ToList();
                Student.FacultyNavigation = _Context.Faculties.Where(x => x.Id == Student.Faculty).Select(x => x).FirstOrDefault();
            }
            List<StudentProfile> Students2 = Students.Where(x => x.IdNavigation.IsApproved == StudentAccountProfileManager.CheckIsApproved).Select(x => x).ToList();
            Students = Students2;
            if (StudentAccountProfileManager.CheckCityId != 0)
            {
                Students = Students2.Where(x => x.City == StudentAccountProfileManager.CheckCityId).Select(x => x).ToList();
            }
            if (StudentAccountProfileManager.SelectedFacultyId != 0)
            {
                Students2 = Students.Where(x => x.Faculty == StudentAccountProfileManager.SelectedFacultyId).Select(x => x).ToList();
                Students = Students2;
            }
            if (StudentAccountProfileManager.CheckString != null)
            {
                string CheckString = StudentAccountProfileManager.CheckString.ToLower();
                Students2 = Students.Where(x => CheckString.Contains(x.IdNavigation.GivenName.ToLower()) ||
                                                CheckString.Contains(x.IdNavigation.SubName.ToLower()) ||
                                                CheckString.Contains(x.IdNavigation.Email.ToLower()) ||
                                                CheckString.Contains(x.CityNavigation.CityName.ToLower()) ||
                                                CheckString.Contains(x.Mssv.ToString().ToLower()) ||
                                                CheckString.Contains(x.FacultyNavigation.FacultyName.ToLower())
                                                ).Select(x => x).ToList();
                Students = Students2;
                Students2 = new List<StudentProfile>();
                foreach (var Student in Students)
                {
                    foreach (var Skill in Student.StudentSkills)
                    {
                        Skill.Skill = _Context.Skills.Where(x => x.Id == Skill.SkillId && !x.Isdeleted).Select(x => x).FirstOrDefault();
                        if (CheckString.Contains(Skill.Skill.SkillName.ToLower()))
                        {
                            Students2.Add(Student);
                        }
                    }
                }
                if (Students2.Count() > 0) Students = Students2;
            }
            return Students;
        }
        public void ResetPassword(int UserId, int AdminID)
        {
            User User = new User();
            User = _Context.Users.Where(x => x.Id == UserId).Select(x => x).FirstOrDefault();
            StudentProfile StudentProfile = _Context.StudentProfiles.Where(x => x.Id == UserId).Select(x => x).FirstOrDefault();
            string pw = StudentProfile.Mssv.ToString() + "@Dut";
            User.PasswordHash = new AccountModel().GetMD5(pw);
            User.Updatedate = DateTime.Now;
            User.Updateby = AdminID;
            _Context.Update(User);
            _Context.SaveChanges();
        }
        public void DeleteStudent(int UserId, int AdminID)
        {
            User _User = new User();
            _User = _Context.Users.Where(x => x.Id == UserId).Select(x => x).FirstOrDefault();
            StudentProfile _StudentProfile = _Context.StudentProfiles.Where(x => x.Id == UserId).Select(x => x).FirstOrDefault();
            _User.Isdeleted = true;
            _StudentProfile.Isdeleted = true;
            _User.Deleteby = AdminID;
            _StudentProfile.Deleteby = AdminID;
            _Context.Update(_User);
            _Context.SaveChanges();
            _Context.Update(_StudentProfile);
            _Context.SaveChanges();
        }
        public void CancelDeleteStudent(int UserId, int AdminID)
        {
            User _User = new User();
            _User = _Context.Users.Where(x => x.Id == UserId).Select(x => x).FirstOrDefault();
            StudentProfile _StudentProfile = _Context.StudentProfiles.Where(x => x.Id == UserId).Select(x => x).FirstOrDefault();
            _User.Isdeleted = false;
            _StudentProfile.Isdeleted = false;
            _Context.Update(_User);
            _Context.SaveChanges();
            _Context.Update(_StudentProfile);
            _Context.SaveChanges();
        }
        public string GetUserTag(int ID)
        {
            return _Context.StudentProfiles.Where(x => x.Id == ID).Select(x => x.Mssv).FirstOrDefault().ToString();
        }
        public string GetUserTagCreate(int ID, string Model)
        {
            string result = "";
            if (Model == "User" && ID != 0)
            {
                User User = _Context.Users.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
                result = _Context.Users.Where(x => x.Id == User.Createby).Select(x => x.Email).FirstOrDefault();
            }
            return result;
        }
        public string GetUserTagUpdate(int ID, string Model)
        {
            string result = "";
            if (Model == "User" && ID != 0)
            {
                User User = _Context.Users.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
                result = _Context.Users.Where(x => x.Id == User.Updateby).Select(x => x.Email).FirstOrDefault();
            }
            return result;
        }
        public string GetUserTagDelete(int ID, string Model)
        {
            string result = "";
            if (Model == "User" && ID != 0)
            {
                User User = _Context.Users.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
                result = _Context.Users.Where(x => x.Id == User.Deleteby).Select(x => x.Email).FirstOrDefault();
            }
            return result;
        }
        public void UpdateAdminProfileModelToDB(int ID, int AdminID, string ImageFileName)
        {
            User User = _Context.Users.Where(x => x.Id == ID).Select(x => x).FirstOrDefault();
            if (ImageFileName != null)
            {
                User.AvtLocation = ImageFileName;
            }
            User.Bio = ProfileModel.User.Bio;
            User.DateOfBirth = ProfileModel.User.DateOfBirth;
            User.Gender = ProfileModel.SelectedGender;
            User.Phone = ProfileModel.User.Phone;
            User.Updatedate = DateTime.Now;
            User.Updateby = AdminID;
            _Context.Update(User);
            _Context.SaveChanges();
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
        public void UpdateAccountModelToDB(int AdminID, int ID)
        {
            User _User = _Context.Users.Where(x => x.Id == ID && x.IsApproved.Equals(true) && x.Isdeleted.Equals(false))
                .Select(x => x).FirstOrDefault();
            _User.PasswordHash = GetMD5(AccountModel.NewPassword);
            _User.Updatedate = DateTime.Now;
            _User.Updateby = AdminID;
            _Context.Update(_User);
            _Context.SaveChanges();
        }
        public List<User> GetUpdateAdminAccountsSearch()
        {
            List<User> List = (from user in _Context.Users
                               where user.IsApproved == AdminAccountManager.CheckIsApproved
                               && user.Isdeleted == AdminAccountManager.CheckIsDeleted
                               && user.RoleId == 1
                               select user).ToList();
            if (CompanyAccountManager.CheckString != null)
            {
                List = List.Where(x => x.GivenName.ToLower() == AdminAccountManager.CheckString.ToLower() ||
                                    x.SubName.ToLower() == AdminAccountManager.CheckString.ToLower() ||
                                    x.Email.ToLower().Contains(AdminAccountManager.CheckString.ToLower()) ||
                                    x.Bio.ToLower().Contains(AdminAccountManager.CheckString.ToLower())).Select(x => x).ToList();
            }
            return List;
        }
        public void AdminAccountApproval(int id)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.IsApproved = true;
            _Context.Update(User);
            _Context.SaveChanges();
        }
        public void CancelAdminAccountApproval(int id)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.IsApproved = false;
            _Context.Update(User);
            _Context.SaveChanges();
        }
        public void AdminAccountDelete(int id, int AdminID)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.Isdeleted = true;
            User.Deleteby = AdminID;
            _Context.Update(User);
            _Context.SaveChanges();
        }
        public void CancelAdminAccountDelete(int id)
        {
            User User = _Context.Users.Where(x => x.Id == id).Select(x => x).FirstOrDefault();
            User.Isdeleted = false;
            _Context.Update(User);
            _Context.SaveChanges();
        }
        public bool CheckIdExist(int Id)
        {
            return _Context.Users.Where(x => x.Id == Id).Select(x => x).ToList().Count() > 0;
        }
        public List<string> GetLabelsGraph()
        {
            List<string> result = new List<string>();
            int Year = 2022 + SystemStatistic.SelectedYear;
            List<int> days = new List<int>();
            if (Year % 4 == 0 || Year % 400 == 0)
            {
                days = new List<int>() { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            }
            else days = new List<int>() { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (SystemStatistic.SelectedMonth == 0)
            {
                result = new List<string>() { "Tháng 1", "Tháng 2", "Tháng 3", "Tháng 4", "Tháng 5", "Tháng 6", "Tháng 7", "Tháng 8", "Tháng 9", "Tháng 10", "Tháng 11", "Tháng 12" };
            }
            else if (SystemStatistic.SelectedMonth >= 1)
            {
                for (int i = 1; i <= days[SystemStatistic.SelectedMonth]; i++)
                {
                    result.Add(i.ToString());
                }
            }
            return result;
        }
        public List<int> GetTotalPostValuesGraph()
        {
            List<int> result = new List<int>();
            int Year = 2022 + SystemStatistic.SelectedYear;
            List<int> days = new List<int>();
            if (Year % 4 == 0 || Year % 400 == 0)
            {
                days = new List<int>() { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            }
            else days = new List<int>() { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (SystemStatistic.SelectedMonth == 0)
            {
                for (int i = 1; i <= 12; i++)
                {
                    int temp = _Context.CompanyPosts.Where(x => x.Createdate.Year == Year && x.Createdate.Month == i).Count();
                    result.Add(temp);
                }
            }
            else if (SystemStatistic.SelectedMonth > 0)
            {
                for (int i = 1; i <= days[SystemStatistic.SelectedMonth]; i++)
                {
                    int temp = _Context.CompanyPosts.Where(x => x.Createdate.Year == Year && x.Createdate.Month == SystemStatistic.SelectedMonth && x.Createdate.Day == i).Count();
                    result.Add(temp);
                }
            }
            return result;
        }
        public List<int> GetTotalSubmitValuesGraph()
        {
            List<int> result = new List<int>();
            int Year = 2022 + SystemStatistic.SelectedYear;
            List<int> days = new List<int>();
            if (Year % 4 == 0 || Year % 400 == 0)
            {
                days = new List<int>() { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            }
            else days = new List<int>() { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (SystemStatistic.SelectedMonth == 0)
            {
                for (int i = 1; i <= 12; i++)
                {
                    int temp = _Context.PostSubmits.Where(x => x.Createdate.Year == Year && x.Createdate.Month == i).Count();
                    result.Add(temp);
                }
            }
            else if (SystemStatistic.SelectedMonth > 0)
            {
                for (int i = 1; i <= days[SystemStatistic.SelectedMonth]; i++)
                {
                    int temp = _Context.PostSubmits.Where(x => x.Createdate.Year == Year && x.Createdate.Month == SystemStatistic.SelectedMonth && x.Createdate.Day == i).Count();
                    result.Add(temp);
                }
            }
            return result;
        }
        public List<int> GetTotalPostHasBeenAppliedValuesGraph()
        {
            List<int> result = new List<int>();
            int Year = 2022 + SystemStatistic.SelectedYear;
            List<int> days = new List<int>();
            if (Year % 4 == 0 || Year % 400 == 0)
            {
                days = new List<int>() { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            }
            else days = new List<int>() { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            if (SystemStatistic.SelectedMonth == 0)
            {
                for (int i = 1; i <= 12; i++)
                {
                    List<CompanyPost> temp = _Context.CompanyPosts.Where(x => x.Createdate.Year == Year && x.Createdate.Month == i).ToList();
                    int j = 0;
                    foreach(CompanyPost item in temp)
                    {
                        if(_Context.PostSubmits.Where(x => x.PostId == item.Id).Count() > 0)
                            j++;
                    }
                    result.Add(j);
                }
            }
            else if (SystemStatistic.SelectedMonth > 0)
            {
                for (int i = 1; i <= days[SystemStatistic.SelectedMonth]; i++)
                {
                    List<CompanyPost> temp = _Context.CompanyPosts.Where(x => x.Createdate.Year == Year && x.Createdate.Month == SystemStatistic.SelectedMonth && x.Createdate.Day == i).ToList();
                    int j = 0;
                    foreach (CompanyPost item in temp)
                    {
                        if (_Context.PostSubmits.Where(x => x.PostId == item.Id).Count() > 0)
                            j++;
                    }
                    result.Add(j);
                }
            }
            return result;
        }
        public SelectList GetListYear()
        {
            SelectList result = new SelectList(new List<SelectListItem>
            { new SelectListItem { Text = "2022", Value = "0" },
            new SelectListItem { Text = "2023", Value = "1" },
            new SelectListItem { Text = "2024", Value = "2" } }, "Value", "Text");
            return result;
        }
        public SelectList GetListMonth()
        {
            SelectList result = new SelectList(new List<SelectListItem>
            { new SelectListItem { Text = "Cả năm", Value = "0" },
            new SelectListItem { Text = "Tháng 1", Value = "1" },
             new SelectListItem { Text = "Tháng 2", Value = "2" },
              new SelectListItem { Text = "Tháng 3", Value = "3" },
               new SelectListItem { Text = "Tháng 4", Value = "4" },
                new SelectListItem { Text = "Tháng 5", Value = "5" },
                 new SelectListItem { Text = "Tháng 6", Value = "6" },
                  new SelectListItem { Text = "Tháng 7", Value = "7" },
                   new SelectListItem { Text = "Tháng 8", Value = "8" },
                    new SelectListItem { Text = "Tháng 9", Value = "9" },
                     new SelectListItem { Text = "Tháng 10", Value = "10" },
                      new SelectListItem { Text = "Tháng 11", Value = "11" },
            new SelectListItem { Text = "Tháng 12", Value = "12" } }, "Value", "Text");
            return result;
        }
    }
}
