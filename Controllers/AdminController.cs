 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PBL3.Models;
using PBL3.Models.Admin;
using PBL3.Models.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Controllers
{
    public class AdminController : BaseController
    {
        private readonly ILogger<AdminController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        AdminModel _Model = new AdminModel();
        public AdminController(ILogger<AdminController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        //QUẢN LÍ BÀI VIẾT _ DOANH NGHIỆP==================================================================================
        public IActionResult PostManager(int ? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            PostManagerModel PostManager = new PostManagerModel();
            PostManager.Posts = _Model.GetUpdatePostsSearch();
            PostManager.ListSkills = _Model.GetAllSkills();
            PostManager.ListCities = _Model.GetAllCities();
            _Model.PostManager = PostManager;
            if (_Model.PostManager.Posts.Count > 0)
                PostManager.SelectedPostId = (int)_Model.PostManager.Posts.FirstOrDefault().Id;
            else PostManager.SelectedPostId = 0;
            ViewData["Title"] = "Quản lí bài đăng";
            return View(PostManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostManager(PostManagerModel _PostManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            _PostManager.ListSkills = _Model.GetAllSkills();
            _PostManager.ListCities = _Model.GetAllCities();
            _Model.PostManager = _PostManager;
            _PostManager.Posts = _Model.GetUpdatePostsSearch();
            if (id != null)
            {
                _PostManager.SelectedPostId = (int)id;
            }
            else
            {
                if (_PostManager.Posts.Count == 0)
                {
                    _PostManager.SelectedPostId = 0;
                }
                else
                {
                    _PostManager.SelectedPostId = _Model.PostManager.Posts.FirstOrDefault().Id;

                }
            }
            _Model.PostManager = _PostManager;
            ViewData["Title"] = "Quản lí bài đăng";
            return View(_Model.PostManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostApproval(PostManagerModel _PostManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            if (id != null)
            {
                _Model.PostApproval((int)id);
            }
            _PostManager.ListSkills = _Model.GetAllSkills();
            _PostManager.ListCities = _Model.GetAllCities();
            _Model.PostManager = _PostManager;
            _PostManager.Posts = _Model.GetUpdatePostsSearch();
            _PostManager.SelectedPostId = 0;
            _Model.PostManager = _PostManager;
            return View("PostManager",_Model.PostManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelPostApproval(PostManagerModel _PostManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            if (id != null)
            {
                int AdminID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
                _Model.CancelPostApproval((int)id, AdminID);
            }
            _PostManager.ListSkills = _Model.GetAllSkills();
            _PostManager.ListCities = _Model.GetAllCities();
            _Model.PostManager = _PostManager;
            _PostManager.Posts = _Model.GetUpdatePostsSearch();
            _PostManager.SelectedPostId = 0;
            _Model.PostManager = _PostManager;
            return View("PostManager", _Model.PostManager);
        }
        //=================================================================================================================
        //QUẢN LÍ TÀI KHOẢN _ DOANH NGHIỆP=================================================================================
        public IActionResult CompanyAccountManager(int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            CompanyAccountManagerModel AccountManager = new CompanyAccountManagerModel();
            AccountManager.ListAccounts = _Model.GetUpdateCompanyAccountsSearch();
            AccountManager.ListSkills = _Model.GetAllSkills();
            AccountManager.ListCities = _Model.GetAllCities();
            _Model.CompanyAccountManager = AccountManager;
            if (_Model.CompanyAccountManager.ListAccounts.Count > 0)
                AccountManager.SelectedCompanyId = (int)_Model.CompanyAccountManager.ListAccounts.FirstOrDefault().User.Id;
            else AccountManager.SelectedCompanyId = 0;
            ViewBag.UserTagCreate = _Model.GetUserTagCreate(AccountManager.SelectedCompanyId, "User");
            ViewBag.UserTagUpdate = _Model.GetUserTagUpdate(AccountManager.SelectedCompanyId, "User");
            ViewBag.UserTagDelete = _Model.GetUserTagDelete(AccountManager.SelectedCompanyId, "User");
            ViewData["Title"] = "Quản lí doanh nghiệp";
            return View(AccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompanyAccountManager(CompanyAccountManagerModel _AccountManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            _AccountManager.ListSkills = _Model.GetAllSkills();
            _AccountManager.ListCities = _Model.GetAllCities();
            _Model.CompanyAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateCompanyAccountsSearch();
            if (id != null)
            {
                _AccountManager.SelectedCompanyId = (int)id;
            }
            else
            {
                if (_AccountManager.ListAccounts.Count == 0)
                {
                    _AccountManager.SelectedCompanyId = 0;
                }
                else
                {
                    _AccountManager.SelectedCompanyId = _Model.CompanyAccountManager.ListAccounts.FirstOrDefault().User.Id;

                }
            }
            _Model.CompanyAccountManager = _AccountManager;
            ViewBag.UserTagCreate = _Model.GetUserTagCreate(_AccountManager.SelectedCompanyId, "User");
            ViewBag.UserTagUpdate = _Model.GetUserTagUpdate(_AccountManager.SelectedCompanyId, "User");
            ViewBag.UserTagDelete = _Model.GetUserTagDelete(_AccountManager.SelectedCompanyId, "User");
            ViewData["Title"] = "Quản lí Doanh nghiệp";
            return View(_Model.CompanyAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompanyAccountApproval(CompanyAccountManagerModel _AccountManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            if (id != null)
            {
                _Model.CompanyAccountApproval((int)id);
            }
            _AccountManager.ListSkills = _Model.GetAllSkills();
            _AccountManager.ListCities = _Model.GetAllCities();
            _Model.CompanyAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateCompanyAccountsSearch();
            _AccountManager.SelectedCompanyId = 0;
            _Model.CompanyAccountManager = _AccountManager;
            return View("CompanyAccountManager", _Model.CompanyAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelCompanyAccountApproval(CompanyAccountManagerModel _AccountManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            if (id != null)
            {
                _Model.CancelCompanyAccountApproval((int)id);
            }
            _AccountManager.ListSkills = _Model.GetAllSkills();
            _AccountManager.ListCities = _Model.GetAllCities();
            _Model.CompanyAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateCompanyAccountsSearch();
            _AccountManager.SelectedCompanyId = 0;
            _Model.CompanyAccountManager = _AccountManager;
            return View("CompanyAccountManager", _Model.CompanyAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAccount(CompanyAccountManagerModel _AccountManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            if (id != null)
            {
                int AdminID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
                _Model.CompanyAccountDelete((int)id, AdminID);
            }
            _AccountManager.ListSkills = _Model.GetAllSkills();
            _AccountManager.ListCities = _Model.GetAllCities();
            _Model.CompanyAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateCompanyAccountsSearch();
            _AccountManager.SelectedCompanyId = 0;
            _Model.CompanyAccountManager = _AccountManager;
            return View("CompanyAccountManager", _Model.CompanyAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelDeleteAccount(CompanyAccountManagerModel _AccountManager, int? id)
        {
            if(!AuthorizeUser("SuperAdmin","Admin"))return RedirectToAction("Index","Home");
            if (id != null)
            {
                _Model.CancelCompanyAccountDelete((int)id);
            }
            _AccountManager.ListSkills = _Model.GetAllSkills();
            _AccountManager.ListCities = _Model.GetAllCities();
            _Model.CompanyAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateCompanyAccountsSearch();
            _AccountManager.SelectedCompanyId = 0;
            _Model.CompanyAccountManager = _AccountManager;
            return View("CompanyAccountManager", _Model.CompanyAccountManager);
        }
        //=================================================================================================================
        //QUẢN LÍ SINH VIÊN - TẠO TÀI KHOẢN SINH VIÊN =====================================================================
        [HttpGet]
        public IActionResult CreateStudentAccount(List<StudentProfileModel> students = null)
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            students = students == null ? new List<StudentProfileModel>() : students;
            ViewData["Title"] = "Tạo tài khoản sinh viên";
            if (TempData["FileName"] != null)
            {
                string filename = TempData["FileName"].ToString();
                _Model.ListStudents = _Model.GetListStudent(filename);
                List<StudentProfileModel> studentsInvalid = _Model.GetListStudentInvalid();
                if (studentsInvalid.Count() > 0)
                {
                    students = studentsInvalid;
                }
            }
            ViewBag.State = TempData["State"];
            return View(students);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateStudentAccount(IFormFile file)
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            string FileName = null;
            ViewData["Title"] = "Tạo tài khoản sinh viên";
            if (file != null)
            {
                if (file.Length > 10240000)
                {
                    ViewBag.FileError = "File có kích thước quá lớn";
                    return CreateStudentAccount();
                }
                else
                {
                    string Folder = "files/";
                    FileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                    if (Path.GetExtension(FileName) == ".xlsx")
                    {
                        string ServerFolder = Path.Combine(_webHostEnvironment.WebRootPath, Folder + FileName);
                        using (FileStream fileStream = System.IO.File.Create(ServerFolder))
                        {
                            file.CopyTo(fileStream);
                            fileStream.Flush();
                        }
                    }
                    else
                    {
                        FileName = null;
                        ViewBag.FileError = "Chỉ được tải lên file excel.";
                        return CreateStudentAccount();
                    }
                }
                try
                {
                    var students = _Model.GetListStudent(FileName);
                    ViewBag.FileName = FileName;
                    TempData["State"] = 4; // Tải file thành công
                    return CreateStudentAccount(students);
                }
                catch (Exception e)
                {
                    TempData["State"] = 5; // File không hợp lệ.
                }
            }
            return CreateStudentAccount();
        }
        public IActionResult ConfirmCreateStudentAccount(string filename)
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            if(filename == null) return RedirectToAction("CreateStudentAccount");
            _Model.ListStudents = _Model.GetListStudent(filename);
            if (_Model.ListStudents.Count() == 0) return RedirectToAction("CreateStudentAccount");
            List<StudentProfileModel> studentsInvalid = _Model.GetListStudentInvalid();
            TempData["FileName"] = null;
            if (studentsInvalid.Count()>0)
            {
                TempData["FileName"] = filename;
                TempData["State"] = 3; //Thông tin sinh viên không hợp lệ, vui lòng chỉnh sửa lại.
                return RedirectToAction("CreateStudentAccount");
            }
            try
            {
                int AdminID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
                _Model.AddStudentAccountsToDB(AdminID);
                TempData["State"] = 1; //Thêm tài khoản thành công
            }
            catch (Exception e)
            {
                ViewBag.State = e.ToString();
                TempData["State"] = 2; //Đã xảy ra lỗi trong khi lưu
            }
            return RedirectToAction("CreateStudentAccount");
        }
        //=================================================================================================================
        //QUẢN LÍ TÀI KHOẢN THÔNG TIN - QUẢN LÍ SINH VIÊN =================================================================
        public IActionResult StudentAccountProfileManager()
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            StudentAccountProfileManagerModel Students = new StudentAccountProfileManagerModel();
            Students.ListStudents = _Model.GetUpdateStudentAccountProfileSearch();
            Students.ListCities = _Model.GetAllCities();
            Students.ListFaculties = _Model.GetAllFaculties();
            ViewBag.State = TempData["State"]==null?0:TempData["State"];
            ViewData["Title"] = "Quản lí Sinh viên";
            return View(Students);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StudentAccountProfileManager(StudentAccountProfileManagerModel Students)
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            _Model.StudentAccountProfileManager = Students;
            Students.ListStudents = _Model.GetUpdateStudentAccountProfileSearch();
            Students.ListCities = _Model.GetAllCities();
            Students.ListFaculties = _Model.GetAllFaculties();
            ViewBag.State = TempData["State"] == null ? 0 : TempData["State"];
            ViewData["Title"] = "Quản lí Sinh viên";
            return View(Students);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(StudentAccountProfileManagerModel Students, int id)
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            _Model.StudentAccountProfileManager = Students;
            _Model.ResetPassword(id, HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault());
            Students.ListStudents = _Model.GetUpdateStudentAccountProfileSearch();
            Students.ListCities = _Model.GetAllCities();
            Students.ListFaculties = _Model.GetAllFaculties();
            TempData["State"] = 1; //Reset mật khẩu thành công!
            ViewBag.State = TempData["State"] == null ? 0 : TempData["State"];
            Students.SelectedItem = id;
            ViewBag.UserTag = _Model.GetUserTag(id);
            return View("StudentAccountProfileManager", Students);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteStudent(StudentAccountProfileManagerModel Students, int id)
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            _Model.StudentAccountProfileManager = Students;
            _Model.DeleteStudent(id, HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault());
            Students.ListStudents = _Model.GetUpdateStudentAccountProfileSearch();
            Students.ListCities = _Model.GetAllCities();
            Students.ListFaculties = _Model.GetAllFaculties();
            TempData["State"] = 2; //Xóa tài khoản thành công
            ViewBag.State = TempData["State"] == null ? 0 : TempData["State"];
            Students.SelectedItem = id;
            ViewBag.UserTag = _Model.GetUserTag(id);
            return View("StudentAccountProfileManager", Students);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelDeleteStudent(StudentAccountProfileManagerModel Students, int id)
        {
            if (!AuthorizeUser("SuperAdmin","Admin")) return RedirectToAction("Index", "Home");
            _Model.StudentAccountProfileManager = Students;
            _Model.CancelDeleteStudent(id, HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault());
            Students.ListStudents = _Model.GetUpdateStudentAccountProfileSearch();
            Students.ListCities = _Model.GetAllCities();
            Students.ListFaculties = _Model.GetAllFaculties();
            TempData["State"] = 3; //Hủy xóa tài khoản thành công.
            ViewBag.State = TempData["State"] == null ? 0 : TempData["State"];
            Students.SelectedItem = id;
            ViewBag.UserTag = _Model.GetUserTag(id);
            return View("StudentAccountProfileManager", Students);
        }
        //=================================================================================================================
        //ADMIN - EDIT ACCOUNT=============================================================================================
        public IActionResult EditProfile(int? id)
        {
            if (!AuthorizeUser("Admin", "SuperAdmin")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (id != null && AuthorizeUser("SuperAdmin") && _Model.CheckIdExist((int)id)) UserID = (int)id; 
            _Model.ProfileModel = _Model.GetAdminProfileByID(UserID);
            ViewData["State"] = TempData.ContainsKey("State") ? TempData["State"] : "";
            ViewData["Title"] = "Chỉnh sửa thông tin";
            return View(_Model.ProfileModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(int? id, AdminProfileModel ProfileModel)
        {
            if (!AuthorizeUser("Admin", "SuperAdmin")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            int ID;
            if (id != null && AuthorizeUser("SuperAdmin") && _Model.CheckIdExist((int)id)) ID = (int)id;
            else ID = UserID;
            ViewData["Title"] = "Chỉnh sửa thông tin";
            ProfileModel.ListGender = _Model.GetListGender();
            if (ModelState.IsValid)
            {
                _Model.ProfileModel = ProfileModel;
                string ImageFileName = null;
                if (ProfileModel.LogoImage != null)
                {
                    if (ProfileModel.LogoImage.Length > 102400)
                    {
                        ViewBag.ImagesError = "*Ảnh có kích thước quá lớn";
                        return View(ProfileModel);
                    }
                    else
                    {
                        string Folder = "images/admins/";
                        ImageFileName = Guid.NewGuid().ToString() + "_" + ProfileModel.LogoImage.FileName;
                        if (Path.GetExtension(ImageFileName) == ".png" ||
                            Path.GetExtension(ImageFileName) == ".jpg" ||
                            Path.GetExtension(ImageFileName) == ".jpeg")
                        {
                            string ServerFolder = Path.Combine(_webHostEnvironment.WebRootPath, Folder + ImageFileName);
                            ProfileModel.LogoImage.CopyTo(new FileStream(ServerFolder, FileMode.Create));
                        }
                        else
                        {
                            ImageFileName = null;
                            ViewBag.ImagesError = "*Chỉ được tải lên hình ảnh";
                            return View(ProfileModel);
                        }
                    }
                }
                _Model.UpdateAdminProfileModelToDB(ID,UserID, ImageFileName);
                ViewData["State"] = "Profile Suscess";
            }
            else
            {
                ViewData["State"] = "Profile Failed";
                return View(ProfileModel);
            }
            AdminProfileModel Result = _Model.GetAdminProfileByID(UserID);
            return View(Result);
        }
        public IActionResult EditAccountPassword()
        {
            if (!AuthorizeUser("Admin", "SuperAdmin")) return RedirectToAction("Index", "Home");
            ViewData["Title"] = "Chỉnh sửa thông tin";
            return View(_Model.AccountModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccountPassword(int? id,AccountModel AccoutModel)
        {
            if (!AuthorizeUser("Admin", "SuperAdmin")) return RedirectToAction("Index", "Home");
            ViewData["Title"] = "Chỉnh sửa thông tin";
            int AdminID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            int ID;
            if (id != null && AuthorizeUser("SuperAdmin") && _Model.CheckIdExist((int)id)) ID = (int)id;
            else ID = AdminID;
            if (ModelState.IsValid)
            {
                int Result = _Model.CheckAccountModel(ID, AccoutModel);
                if (Result == _Model.CHANGE_PASSWORD_SUCCESS)
                {
                    _Model.AccountModel = AccoutModel;
                    _Model.UpdateAccountModelToDB(ID, AdminID);
                    ViewBag.Accepted = "Đổi mật khẩu thành công";
                    ViewBag.CheckPassword = " ";
                    ViewBag.CheckOldNewPassword = " ";
                    return View(new AccountModel());
                }
                else if (Result == _Model.CHECK_INCORRECT_PASSWORD)
                {
                    ViewBag.CheckPassword = "Mật khẩu cũ không chính xác";
                    ViewBag.Accepted = "Đổi mật khẩu không thành công";
                    ViewBag.CheckOldNewPassword = " ";
                    return View(AccoutModel);
                }
                else if (Result == _Model.OLD_NEW_PASSWORD_ARE_THE_SAME)
                {
                    ViewBag.CheckPassword = " ";
                    ViewBag.Accepted = " ";
                    ViewBag.CheckOldNewPassword = "Mật khẩu cũ và mới không được trùng nhau";
                    return View(AccoutModel);
                }
                else
                {
                    ViewBag.CheckPassword = " ";
                    ViewBag.Accepted = " ";
                    ViewBag.CheckOldNewPassword = " ";
                }
            }
            return View();
        }
        //=================================================================================================================
        //QUẢN LÍ ADMIN - TÀI KHOẢN =======================================================================================
        public IActionResult AdminAccountManager(int? id)
        {
            if (!AuthorizeUser("SuperAdmin")) return RedirectToAction("Index", "Home");
            AdminAccountManagerModel AccountManager = new AdminAccountManagerModel();
            AccountManager.ListAccounts = _Model.GetUpdateAdminAccountsSearch();
            _Model.AdminAccountManager = AccountManager;
            if (_Model.AdminAccountManager.ListAccounts.Count > 0)
                AccountManager.SelectedAdminId = (int)_Model.AdminAccountManager.ListAccounts.FirstOrDefault().Id;
            else AccountManager.SelectedAdminId = 0;
            ViewBag.UserTagCreate = _Model.GetUserTagCreate(AccountManager.SelectedAdminId, "User");
            ViewBag.UserTagUpdate = _Model.GetUserTagUpdate(AccountManager.SelectedAdminId, "User");
            ViewBag.UserTagDelete = _Model.GetUserTagDelete(AccountManager.SelectedAdminId, "User");
            ViewData["Title"] = "Quản lí Admin";
            return View(AccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminAccountManager(AdminAccountManagerModel _AccountManager, int? id)
        {
            if (!AuthorizeUser("SuperAdmin")) return RedirectToAction("Index", "Home");
            _Model.AdminAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateAdminAccountsSearch();
            if (id != null)
            {
                _AccountManager.SelectedAdminId = (int)id;
            }
            else
            {
                if (_AccountManager.ListAccounts.Count == 0)
                {
                    _AccountManager.SelectedAdminId = 0;
                }
                else
                {
                    _AccountManager.SelectedAdminId = _Model.AdminAccountManager.ListAccounts.FirstOrDefault().Id;

                }
            }
            _Model.AdminAccountManager = _AccountManager;
            ViewBag.UserTagCreate = _Model.GetUserTagCreate(_AccountManager.SelectedAdminId, "User");
            ViewBag.UserTagUpdate = _Model.GetUserTagUpdate(_AccountManager.SelectedAdminId, "User");
            ViewBag.UserTagDelete = _Model.GetUserTagDelete(_AccountManager.SelectedAdminId, "User");
            ViewData["Title"] = "Quản lí Admin";
            return View(_Model.AdminAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AdminAccountApproval(AdminAccountManagerModel _AccountManager, int? id)
        {
            if (!AuthorizeUser("SuperAdmin")) return RedirectToAction("Index", "Home");
            if (id != null)
            {
                _Model.AdminAccountApproval((int)id);
            }
            _Model.AdminAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateAdminAccountsSearch();
            _AccountManager.SelectedAdminId = 0;
            _Model.AdminAccountManager = _AccountManager;
            return View("AdminAccountManager", _Model.AdminAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelAdminAccountApproval(AdminAccountManagerModel _AccountManager, int? id)
        {
            if (!AuthorizeUser("SuperAdmin")) return RedirectToAction("Index", "Home");
            if (id != null)
            {
                _Model.CancelAdminAccountApproval((int)id);
            }
            _Model.AdminAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateAdminAccountsSearch();
            _AccountManager.SelectedAdminId = 0;
            _Model.AdminAccountManager = _AccountManager;
            return View("AdminAccountManager", _Model.AdminAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAdminAccount(AdminAccountManagerModel _AccountManager, int? id)
        {
            if (!AuthorizeUser("SuperAdmin", "Admin")) return RedirectToAction("Index", "Home");
            if (id != null)
            {
                int AdminID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
                _Model.AdminAccountDelete((int)id, AdminID);
            }
            _Model.AdminAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateAdminAccountsSearch();
            _AccountManager.SelectedAdminId = 0;
            _Model.AdminAccountManager = _AccountManager;
            return View("AdminAccountManager", _Model.AdminAccountManager);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CancelDeleteAdminAccount(AdminAccountManagerModel _AccountManager, int? id)
        {
            if (!AuthorizeUser("SuperAdmin", "Admin")) return RedirectToAction("Index", "Home");
            if (id != null)
            {
                _Model.CancelAdminAccountDelete((int)id);
            }
            _Model.AdminAccountManager = _AccountManager;
            _AccountManager.ListAccounts = _Model.GetUpdateAdminAccountsSearch();
            _AccountManager.SelectedAdminId = 0;
            _Model.AdminAccountManager = _AccountManager;
            return View("AdminAccountManager", _Model.AdminAccountManager);
        }
        //=================================================================================================================
        //QUẢN LÍ HỆ THỐNG ================================================================================================
        public IActionResult SystemStatistic()
        {
            if (!AuthorizeUser("SuperAdmin")) return RedirectToAction("Index", "Home");
            _Model.SystemStatistic.Years = _Model.GetListYear();
            _Model.SystemStatistic.Months = _Model.GetListMonth();
            _Model.SystemStatistic.Labels = _Model.GetLabelsGraph();
            _Model.SystemStatistic.Values = _Model.GetTotalPostValuesGraph();
            _Model.SystemStatistic.Values2 = _Model.GetTotalSubmitValuesGraph();
            _Model.SystemStatistic.Values3 = _Model.GetTotalPostHasBeenAppliedValuesGraph();
            ViewData["Title"] = "Quản lí hệ thống";
            return View(_Model.SystemStatistic);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SystemStatistic(SystemStatisticModel statisticModel)
        {
            if (!AuthorizeUser("SuperAdmin")) return RedirectToAction("Index", "Home");
            _Model.SystemStatistic = statisticModel;
            _Model.SystemStatistic.Years = _Model.GetListYear();
            _Model.SystemStatistic.Months = _Model.GetListMonth();
            _Model.SystemStatistic.Labels = _Model.GetLabelsGraph();
            _Model.SystemStatistic.Values = _Model.GetTotalPostValuesGraph();
            _Model.SystemStatistic.Values2 = _Model.GetTotalSubmitValuesGraph();
            _Model.SystemStatistic.Values3 = _Model.GetTotalPostHasBeenAppliedValuesGraph();
            ViewData["Title"] = "Quản lí hệ thống";
            return View(_Model.SystemStatistic);
        }
        //=================================================================================================================

    }
}
