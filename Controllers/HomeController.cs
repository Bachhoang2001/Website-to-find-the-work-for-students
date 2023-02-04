using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PBL3.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using PBL3.Models.Home;

namespace PBL3.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        HomeModel _Model = new HomeModel();
        public IActionResult Index()
        {
            ViewBag.TopCompanies = _Model.GetTopCompanyProfiles();
            ViewBag.ListFaculties = _Model.GetListFaculties();
            ViewData["Title"] = "Trang chủ";
            return View(_Model.Index);
        }
        public IActionResult Login()
        {
            ViewData["Title"] = "Đăng nhập";
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel user)
        {
            LoginModel login = new LoginModel();
            ViewData["Title"] = "Đăng nhập";
            if (ModelState.IsValid)
            {
                if (login.CheckEmailPassword(user))
                {
                    var userSession = login.GetSession(user.Email);
                    HttpContext.Session.SetInt32(CommonConstraints.USER_ID, userSession.ID);
                    HttpContext.Session.SetString(CommonConstraints.USER_SUBNAME, userSession.SubName);
                    HttpContext.Session.SetString(CommonConstraints.USER_GIVENNAME, userSession.GivenName);
                    HttpContext.Session.SetInt32(CommonConstraints.USER_ROLEID, userSession.RoleID);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Đăng nhập không thành công !");
                    return View();
                }
            }
            return View();
        }
        public IActionResult CompanySignUp()
        {
            CompanySignUpModel SignUp = new CompanySignUpModel();
            SignUp.GetAllCity = _Model.GetListCities().Take(_Model.GetListCities().Count()-1).ToList();
            ViewData["Title"] = "Đăng kí tài khoản Doanh nghiệp";
            return View(SignUp);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CompanySignUp(CompanySignUpModel companySignUp)
        {
            companySignUp.GetAllCity = _Model.GetListCities();
            ViewData["Title"] = "Đăng kí tài khoản Doanh nghiệp";
            if (ModelState.IsValid)
            {
                if (_Model.CheckEmailExist(companySignUp.User.Email))
                {
                    ViewBag.EmailErrorMessages = "Email đã tồn tại";
                    ViewBag.PasswordErrorMessages = "";
                    return View(companySignUp);
                }
                else if (!_Model.CheckPassword(companySignUp.Password, companySignUp.PasswordCheck))
                {
                    ViewBag.PasswordErrorMessages = "Mật khẩu không trùng khớp";
                    ViewBag.EmailErrorMessages = "";
                    return View(companySignUp);
                }
                else
                {
                    ViewBag.EmailErrorMessages = ""; ViewBag.PasswordErrorMessages = "";
                }
                _Model.CompanySignUp = companySignUp;
                _Model.AddCompanySignUpToContext();
                return RedirectToAction("Index");
            }

            return View();
        }
        public IActionResult RecommentSearch(string search)
        {
            _Model.Index.CheckString = search;
            _Model.Index.Posts = _Model.UpdatePostsSearch();
            ViewBag.ListCities = _Model.GetListCities();
            ViewBag.ListFaculties = _Model.GetListFaculties();
            return View("Search", _Model.Index);
        }
        public IActionResult Search(IndexModel Index, int? id, int? page)
        {
            _Model.Index = Index;
            _Model.Index.Posts = _Model.UpdatePostsSearch();
            ViewBag.ListCities = _Model.GetListCities();
            ViewBag.ListFaculties = _Model.GetListFaculties();
            if (id == 0)
            {
                if (_Model.Index.Posts.Count() > 0) _Model.Index.SelectedPost = _Model.Index.Posts.First().Id;
                else _Model.Index.SelectedPost = Index.SelectedPost;
            }
            else
            {
                _Model.Index.SelectedPost = (int)id;
            }
            if (page == 0 || page == null)
            {
                if (Index.CurrentPage == 0)
                    _Model.Index.CurrentPage = 1;
                else _Model.Index.CurrentPage = Index.CurrentPage;
            }
            else { _Model.Index.CurrentPage = (int)page; }
            ViewData["Title"] = "Tìm kiếm việc làm";
            return View(_Model.Index);
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
