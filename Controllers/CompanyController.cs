using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PBL3.Models;
using PBL3.Models.Common;
using PBL3.Models.Company;
using System;
using System.IO;

namespace PBL3.Controllers
{
    public class CompanyController : BaseController
    {
        CompanyModel Model = new CompanyModel();
        private readonly IWebHostEnvironment _webHostEnvironment;
        public CompanyController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index(int? id, int? currentpage)
        {
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (currentpage == null) currentpage = 1;
            if (id == null && Model.CheckCompanyIDExists(CompanyID))
            {
            }
            else
            {
                CompanyID = (int)id;
            }
            HomePageModel Home = new HomePageModel();
            Home = Model.GetHomePageModelData(CompanyID, null);
            Home.CurrentPage = (int)currentpage;
            ViewData["Title"] = "Trang chủ - " + Home.CompanyName;
            return View(Home);
        }
        public IActionResult PostSubmitted()
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            Model._PostSubmitted = Model.GetPostSubmittedByCompanyID(CompanyID);
            ViewData["Title"] = "Thông báo ứng tuyển";
            return View(Model._PostSubmitted);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult PostSubmitted(int? id)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            Model._PostSubmitted = Model.GetPostSubmittedByCompanyID(CompanyID);
            if (id != null)
            {
                Model._PostSubmitted.SelectedPostId = (int)id;
            }
            else
            {
            }
            ViewData["Title"] = "Thông báo ứng tuyển";
            return View(Model._PostSubmitted);
        }
        public IActionResult EditProfile(int? id)
        {
            if (!AuthorizeUser("Company", "Admin", "SuperAdmin")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            int ID;
            if (id != null && AuthorizeUser("Admin", "SuperAdmin") && Model.CheckCompanyIDExists((int)id))
            {
                ID = (int)id;
            }
            else ID = UserID;
            Model._ProfileModel = Model.GetCompanyProfileByID(ID);
            ViewData["State"] = TempData.ContainsKey("State") ? TempData["State"] : "";
            ViewData["Skill"] = TempData.ContainsKey("Skill") ? TempData["Skill"] : "";
            ViewData["Title"] = "Chỉnh sửa thông tin";
            return View(Model._ProfileModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditProfile(int? id,CompanyProfileModel ProfileModel, string value = null)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            ViewData["Title"] = "Chỉnh sửa thông tin";
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            int ID;
            if (id != null && AuthorizeUser("Admin", "SuperAdmin") && Model.CheckCompanyIDExists((int)id))
            {
                ID = (int)id;
            }
            else ID = UserID;
            ProfileModel.ListGender = Model.GetListGender();
            ProfileModel.GetAllCity = Model.GetListCities();
            if (ModelState.IsValid && value == null)
            {
                Model._ProfileModel = ProfileModel;
                string FileName = null;
                if (ProfileModel.LogoImage != null)
                {
                    if (ProfileModel.LogoImage.Length > 102400)
                    {
                        ViewBag.ImagesError = "*Ảnh có kích thước quá lớn";
                        return View(ProfileModel);
                    }
                    else
                    {
                        string Folder = "images/companies/";
                        FileName = Guid.NewGuid().ToString() + "_" + ProfileModel.LogoImage.FileName;
                        if (Path.GetExtension(FileName) == ".png" ||
                            Path.GetExtension(FileName) == ".jpg" ||
                            Path.GetExtension(FileName) == ".jpeg")
                        {
                            string ServerFolder = Path.Combine(_webHostEnvironment.WebRootPath, Folder + FileName);
                            ProfileModel.LogoImage.CopyTo(new FileStream(ServerFolder, FileMode.Create));
                        }
                        else
                        {
                            FileName = null;
                            ViewBag.ImagesError = "*Chỉ được tải lên hình ảnh";
                            return View(ProfileModel);
                        }
                    }
                }
                Model.UpdateCompanyProfileModelToDB(ID, UserID, FileName);
                ViewBag.Messages = true;
                ViewData["State"] = "Profile Suscess";
            }
            else if (value == null)
            {
                ViewBag.Messages = false;
                ViewData["State"] = "Profile Failed";
                return View(ProfileModel);
            }
            else if (value == "search")
            {
                Model._ProfileModel = ProfileModel;
            }
            CompanyProfileModel Result = Model.GetCompanyProfileByID(ID, ProfileModel.SearchSkill == null ? "" : ProfileModel.SearchSkill);
            return View(Result);
        }
        public IActionResult AddSkill(int skillid, int companyid)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (Model.AddSkill(skillid, companyid, UserID))
                TempData["State"] = "Skill Added";
            else TempData["State"] = "Skill Already Exists";
            TempData["Skill"] = Model.GetSkillById(skillid);
            return RedirectToAction("Editprofile");
        }
        public IActionResult DeleteSkill(int skillid, int companyid)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            Model.DeleteSkill(skillid, companyid);
            TempData["State"] = "Skill Deleted";
            TempData["Skill"] = Model.GetSkillById(skillid);
            return RedirectToAction("EditProfile");
        }
        public IActionResult EditAccountPassword()
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            ViewData["Title"] = "Chỉnh sửa thông tin";
            return View(Model._AccountModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAccountPassword(AccountModel _AccountModel)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            ViewData["Title"] = "Chỉnh sửa thông tin";
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (ModelState.IsValid)
            {
                int Result = Model.CheckAccountModel(CompanyID, _AccountModel);
                if (Result == Model.CHANGE_PASSWORD_SUCCESS)
                {
                    Model._AccountModel = _AccountModel;
                    Model.UpdateAccountModelToDB(CompanyID);
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
        public IActionResult PendingApproval(int? id, int? currentpage)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (currentpage == null) currentpage = 1;
            if (id == null && Model.CheckCompanyIDExists(CompanyID))
            {
            }
            else
            {
                CompanyID = (int)id;
            }
            HomePageModel Home = new HomePageModel();
            Home = Model.GetHomePageModelData(CompanyID, null);
            Home.CurrentPage = (int)currentpage;
            ViewData["Title"] = "Bài đăng chờ duyệt";
            return View(Home);
        }
        public IActionResult Reviews(int? id, int? currentpage)
        {
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (currentpage == null) currentpage = 1;
            if (id == null && Model.CheckCompanyIDExists(CompanyID))
            {
            }
            else
            {
                CompanyID = (int)id;
            }
            HomePageModel Home = new HomePageModel();
            Home = Model.GetHomePageModelData(CompanyID, null);
            Home.CurrentPage = (int)currentpage;
            ViewData["Title"] = "Xem đánh giá";
            return View(Home);
        }
        //Phân quyền chỉ Admin và Company
        public IActionResult CreatePost()
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            PostModel model = new PostModel();
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            model.GetAllCity = Model.GetListCities(CompanyID);
            model.GetAllSkill = Model.GetListSkills(CompanyID);
            ViewData["Title"] = "Tạo bài đăng mới";
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(PostModel post)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (ModelState.IsValid)
            {
                post.AddToContext(CompanyID);
                return RedirectToAction("Index");
            }
            ViewData["Title"] = "Tạo bài đăng mới";
            return View();
        }
        public IActionResult Post(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewBag.CurrentPostID = id;
            HomePageModel post = new HomePageModel();
            post = Model.GetHomePageModelData(null, id);
            ViewData["Title"] = "Bài đăng - " + post.CompanyName;
            return View(post);
        }
        public IActionResult EditPost(int? id)
        {
            if (!AuthorizeUser("Company", "Admin", "SuperAdmin")) return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }
            PostModel post = Model.GetPostDetail(id);
            post.GetAllCity = Model.GetListCities(post.CompanyId);
            post.GetAllSkill = Model.GetListSkills(post.CompanyId);
            ViewData["Title"] = "Chỉnh sửa bài đăng";
            return View(post);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditPost(int id, PostModel post)
        {
            if (!AuthorizeUser("Company", "Admin", "SuperAdmin")) return RedirectToAction("Index", "Home");
            int UserID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            if (id != post.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    post.CompanyId = Model.GetCompanyProfileByPostId(id).Id;
                    post.UpdateToContext(UserID);
                }
                catch (Exception e)
                {
                    if (!Model.PostIDExists(post.Id))
                    {
                        return NotFound();
                    }
                    else throw;
                }
            }
            ViewData["Title"] = "Chỉnh sửa bài đăng";
            return RedirectToAction("Index", new { id = post.CompanyId });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            if (!AuthorizeUser("Company")) return RedirectToAction("Index", "Home");
            if (id == null)
            {
                return NotFound();
            }
            int CompanyID = HttpContext.Session.GetInt32(CommonConstraints.USER_ID).GetValueOrDefault();
            PostModel post = Model.GetPostDetail(id);
            if (post.CompanyId != CompanyID)
            {
                return NotFound();
            }
            post.DeletePost();
            return RedirectToAction("Index");
        }
    }
}
