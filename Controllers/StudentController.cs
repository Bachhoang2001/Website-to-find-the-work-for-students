using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3.Models;
using PBL3.Models.Common;
using PBL3.Models.Student;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Controllers
{
    public class StudentController : BaseController
    {
        StudentModel Model = new StudentModel();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public StudentController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult PostApplication()
        {
            if (!AuthorizeUser("Student")) return RedirectToAction("Index", "Home");
            int ID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            Model._PostApplication = Model.GetPostApplicationByStudentID(ID);
            ViewData["Title"] = "Danh sách đã ứng tuyển";
            return View(Model._PostApplication);
        }
        public IActionResult SubmitPost(int? id)
        {
            if (!AuthorizeUser("Student")) return RedirectToAction("Index", "Home");
            if (id == null || Model.NotFoundPostById((int)id))
            {
                return NotFound();
            }
            int ID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            SubmitPostModel SubmitPost = Model.GetDetailSubmitPost((int)id, ID);
            ViewBag.PostID = id;
            ViewData["Title"] = "Ứng tuyển";
            return View(SubmitPost);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitPost(int id, SubmitPostModel _SubmitPost)
        {
            if (!AuthorizeUser("Student")) return RedirectToAction("Index", "Home");
            int ID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            ViewData["Title"] = "Ứng tuyển";
            if (id != _SubmitPost.PostID || Model.NotFoundPostById(id))
            {
                return NotFound();
            }
            //if (ModelState.IsValid)
            // {
            string FileName = null;
            if (_SubmitPost.CV != null)
            {
                _SubmitPost.IsNewCV = true;
                if (_SubmitPost.CV.Length > 1024000)
                {
                    ViewBag.CVError = "*File chỉ tối đa 1MB.";
                    return View(_SubmitPost);
                }
                string Folder = "CVPath/";
                FileName = Guid.NewGuid().ToString() + "_" + _SubmitPost.CV.FileName;
                if (Path.GetExtension(FileName) == ".doc" ||
                    Path.GetExtension(FileName) == ".docx" ||
                    Path.GetExtension(FileName) == ".pdf")
                {
                    string ServerFolder = Path.Combine(_webHostEnvironment.WebRootPath, Folder + FileName);
                    _SubmitPost.CV.CopyTo(new FileStream(ServerFolder, FileMode.Create));
                    _SubmitPost.NewCVPath = FileName;
                }
                else
                {
                    FileName = null;
                    ViewBag.CVError = "*Định dạng File không hợp lệ.";
                    return View(_SubmitPost);
                }
            }
            else
            {
                if (_SubmitPost.IsNewCV || !_SubmitPost.CheckOldCVExist)
                {
                    ViewBag.ImagesError = "*Hãy tải thêm CV của bạn";
                    return View(_SubmitPost);
                }
            }
            try
            {
                Model._SubmitPost = _SubmitPost;
                Model.UpdateSubmitPostToDB(ID);
            }
            catch (Exception e)
            {
                return NotFound();
            }
            //}
            return RedirectToAction("Post", "Company", new { id = id });
        }
        public IActionResult RatingCompany(int? id)
        {
            if (!AuthorizeUser("Student")) return RedirectToAction("Index", "Home");
            if (id != 0 && !Model.CheckRatingCompanyID((int)id))
            {
                return RedirectToAction("Index", "Home");
            }
            int ID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            Model.AddRatingToDBContext(ID, (int)id);
            return RedirectToAction("Index", "Company", new { id = (id - id % 10) / 10 });
        }
        public IActionResult EditProfile(int? id)
        {
            if (!AuthorizeUser("Student", "SuperAdmin", "Admin")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            int ID;
            if (id != null && AuthorizeUser("Admin", "SuperAdmin") && Model.CheckStudentIdExist((int)id))
            {
                ID = (int)id;
            }
            else ID = UserID;
            Model._ProfileModel = Model.GetStudentProfileByID(ID);
            ViewData["State"] = TempData.ContainsKey("State") ? TempData["State"] : "";
            ViewData["Skill"] = TempData.ContainsKey("Skill") ? TempData["Skill"] : "";
            ViewData["Title"] = "Chỉnh sửa thông tin";
            return View(Model._ProfileModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(int? id,StudentProfileModel ProfileModel, string value = null)
        {
            if (!AuthorizeUser("Student", "SuperAdmin", "Admin")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            int ID;
            if (id != null && AuthorizeUser("Admin", "SuperAdmin") && Model.CheckStudentIdExist((int)id))
            {
                ID = (int)id;
            }
            else ID = UserID;
            ViewData["Title"] = "Chỉnh sửa thông tin";
            ProfileModel.ListGender = Model.GetListGender();
            ProfileModel.GetAllCity = Model.GetListCities();
            if (ModelState.IsValid && value == null)
            {
                Model._ProfileModel = ProfileModel;
                string ImageFileName = null;
                string CVFileName = null;
                if (ProfileModel.LogoImage != null)
                {
                    if (ProfileModel.LogoImage.Length > 102400)
                    {
                        ViewBag.ImagesError = "*Ảnh có kích thước quá lớn";
                        return View(ProfileModel);
                    }
                    else
                    {
                        string Folder = "images/students/";
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
                if(ProfileModel.CVFile != null)
                {
                    if (ProfileModel.CVFile.Length > 1024000)
                    {
                        ViewBag.CVError = "*File có kích thước quá lớn";
                        return View(ProfileModel);
                    }
                    else
                    {
                        string Folder = "CVPath/";
                        CVFileName = Guid.NewGuid().ToString() + "_" + ProfileModel.CVFile.FileName;
                        if (Path.GetExtension(CVFileName) == ".pdf" ||
                            Path.GetExtension(CVFileName) == ".doc" ||
                            Path.GetExtension(CVFileName) == ".docx")
                        {
                            string ServerFolder = Path.Combine(_webHostEnvironment.WebRootPath, Folder + CVFileName);
                            ProfileModel.CVFile.CopyTo(new FileStream(ServerFolder, FileMode.Create));
                        }
                        else
                        {
                            CVFileName = null;
                            ViewBag.CVError = "*Chỉ được tải lên .pdf, .doc, .docx";
                            return View(ProfileModel);
                        }
                    }
                }
                Model.UpdateStudentProfileModelToDB(ID, UserID, ImageFileName, CVFileName);
                ViewData["State"] = "Profile Suscess";
            }
            else if (value == null)
            {
                ViewData["State"] = "Profile Failed";
                return View(ProfileModel);
            }
            else if (value == "search")
            {
                Model._ProfileModel = ProfileModel;
            }
            StudentProfileModel Result = Model.GetStudentProfileByID(UserID, ProfileModel.SearchSkill == null ? "" : ProfileModel.SearchSkill);
            return View(Result);
        }
        public IActionResult AddSkill(int skillid, int studentid)
        {
            if (!AuthorizeUser("Student", "SuperAdmin")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (Model.AddSkill(skillid, studentid, UserID))
                TempData["State"] = "Skill Added";
            else TempData["State"] = "Skill Already Exists";
            TempData["Skill"] = Model.GetSkillById(skillid);
            return RedirectToAction("Editprofile", new { id = studentid });
        }
        public IActionResult DeleteSkill(int skillid, int studentid)
        {
            if (!AuthorizeUser("Student", "SuperAdmin")) return RedirectToAction("Index", "Home");
            Model.DeleteSkill(skillid, studentid);
            TempData["State"] = "Skill Deleted";
            TempData["Skill"] = Model.GetSkillById(skillid);
            return RedirectToAction("EditProfile", new { id = studentid});
        }
        public IActionResult EditAccountPassword()
        {
            if (!AuthorizeUser("Student")) return RedirectToAction("Index", "Home");
            ViewData["Title"] = "Chỉnh sửa thông tin";
            return View(Model._AccountModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccountPassword(AccountModel _AccountModel)
        {
            if (!AuthorizeUser("Student")) return RedirectToAction("Index", "Home");
            ViewData["Title"] = "Chỉnh sửa thông tin";
            int StudentID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (ModelState.IsValid)
            {
                int Result = Model.CheckAccountModel(StudentID, _AccountModel);
                if (Result == Model.CHANGE_PASSWORD_SUCCESS)
                {
                    Model._AccountModel = _AccountModel;
                    Model.UpdateAccountModelToDB(StudentID);
                    ViewBag.Accepted = "Đổi mật khẩu thành công";
                    ViewBag.CheckPassword = " ";
                    ViewBag.CheckOldNewPassword = " ";
                    return View(new AccountModel());
                }
                else if (Result == Model.CHECK_INCORRECT_PASSWORD)
                {
                    ViewBag.CheckPassword = "Mật khẩu cũ không chính xác";
                    ViewBag.Accepted = "Đổi mật khẩu không thành công";
                    ViewBag.CheckOldNewPassword = " ";
                    return View(_AccountModel);
                }
                else if (Result == Model.OLD_NEW_PASSWORD_ARE_THE_SAME)
                {
                    ViewBag.CheckPassword = " ";
                    ViewBag.Accepted = " ";
                    ViewBag.CheckOldNewPassword = "Mật khẩu cũ và mới không được trùng nhau";
                    return View(_AccountModel);
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
    }
}
