using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
namespace PBL3.Models.Company
{
    public class PostModel
    {
        private static PBL3Context _Context;
        [Required(ErrorMessage ="Vui lòng điền tiêu đề")]
        [Display(Name = "Tiêu đề")]
        [MaxLength(100, ErrorMessage ="Tối đa 100 kí tự"), MinLength(25, ErrorMessage = "Tối thiểu 25 kí tự")]
        public string Title { get; set; }
        [Required(ErrorMessage ="Vui lòng điền nội dung")]
        [Display(Name = "Nội dung")]
        [MaxLength(1000, ErrorMessage = "Tối đa 1000 kí tự"), MinLength(200, ErrorMessage ="Tối thiểu 200 kí tự")]
        public string Content { get; set; }
        [Required(ErrorMessage ="Vui lòng chọn kĩ năng")]
        [Display(Name = "Chọn Kĩ năng yêu cầu")]
        public List<int> ListSkillId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn thành phố")]
        [Display(Name = "Chọn thành phố áp dụng")]
        public List<int> ListCityId { get; set; } //Lấy từ danh sách City có ID là Company, cho người dùng chọn từ danh sách.
        [Required]
        [Display(Name = "Lương thấp nhất")]
        [RegularExpression("([0-9]*)", ErrorMessage ="Lương không hợp lệ")]
        public int MinSalary { get; set; }
        [Required]
        [RegularExpression("([0-9]*)", ErrorMessage ="Lương không hợp lệ")]
        [Display(Name = "Lương cao nhất")]
        public int MaxSalary { get; set; }
        [Required]
        [Display(Name = "Chọn nếu lương tính bằng Dollar $. Bỏ qua nếu lương tính bằng VNĐ")]
        public bool IsDollar { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<int> CitiesId { get; set; }
        public List<int> SkillsId { get; set; }
        public IEnumerable<SelectListItem> GetAllCity { get; set; }
        public IEnumerable<SelectListItem> GetAllSkill { get; set; }
        public string CompanyAvtPath { get; set; }
        public bool IsApproved { get; set; }
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int Createdby { get; set; }
        public int Deletedby { get; set; }
        public int Updatedby { get; set; }
        public bool IsDelete { get; set; }
        public PostModel()
        {
            _Context = new PBL3Context();
            //CreateDate = DateTime.Now;
            //UpdateDate = DateTime.Now;
            //CompanyAvtPath = "Anonymous.png";
            //IsApproved = false;
            CitiesId = new List<int>();
            CitiesId.Add(0);
            SkillsId = new List<int>();
            SkillsId.Add(0);
            IsDelete = false;
            Id = 0;
            //Id = 0;
            //CompanyId = 0; Createdby = 1; Updatedby = 1; Deletedby = 1;
        }

        public void AddToContext(int CompanyID)
        {
            CompanyPost ComPost = new CompanyPost();
            ComPost.CompanyId = CompanyID;
            ComPost.Title = this.Title;
            ComPost.Content = this.Content;
            ComPost.Createdate = DateTime.Now;
            ComPost.Updatedate = DateTime.Now;
            ComPost.IsApproved = false;
            ComPost.Createby = CompanyID;
            ComPost.Updateby = 1;
            ComPost.Deleteby = 1;
            ComPost.Isdeleted = false;
            ComPost.MinSalary = this.MinSalary;
            ComPost.MaxSalary = this.MaxSalary;
            ComPost.IsDollar = this.IsDollar;
            _Context.Add(ComPost);
            _Context.SaveChanges();
            int PostID = _Context.CompanyPosts.Max(x => x.Id);
            foreach (var item in this.ListSkillId)
            {
                Skillpost SkillPost = new Skillpost();
                SkillPost.SkillId = item;
                SkillPost.PostId = PostID;
                SkillPost.Createdate = DateTime.Now;
                SkillPost.Updatedate = DateTime.Now;
                SkillPost.Createby = CompanyID;
                SkillPost.Updateby = 1;
                SkillPost.Deleteby = 1;
                SkillPost.Isdeleted = false;
                _Context.Add(SkillPost);
                _Context.SaveChanges();
            }
            foreach (var item in this.ListCityId)
            {
                CityPost _CityPost = new CityPost();
                _CityPost.CityId = item;
                _CityPost.PostId = PostID;
                _CityPost.Createdate = DateTime.Now;
                _CityPost.Updatedate = DateTime.Now;
                _CityPost.Createby = CompanyID;
                _CityPost.Updateby = 1;
                _CityPost.Deleteby = 1;
                _CityPost.Isdeleted = false;
                _Context.Add(_CityPost);
                _Context.SaveChanges();
            }
        }
        public void UpdateToContext(int UserID)
        {
            CompanyPost ComPost = new CompanyPost();
            ComPost = _Context.CompanyPosts.Where(x => x.Id == this.Id).Select(x => x).FirstOrDefault();
            ComPost.Id = this.Id;
            ComPost.CompanyId = this.CompanyId;
            ComPost.Title = this.Title;
            ComPost.Content = this.Content;
            ComPost.Updatedate = DateTime.Now;
            ComPost.IsApproved = false;
            ComPost.Updateby = UserID;
            ComPost.MinSalary = this.MinSalary;
            ComPost.MaxSalary = this.MaxSalary;
            ComPost.IsDollar = this.IsDollar;
            _Context.Update(ComPost);
            _Context.SaveChanges();
            //=========================================
            List<Skillpost> PrevSkills = new List<Skillpost>();
            PrevSkills = _Context.Skillposts.Where(x => x.PostId == this.Id).Select(x => x).ToList();
            int PrevSkillCount = PrevSkills.Count();
            int NextSkillCount = this.ListSkillId.Count();
            if (PrevSkillCount == NextSkillCount)
            {
                foreach (var item in this.ListSkillId)
                {
                    Skillpost SkillPost = PrevSkills.First();
                    PrevSkills.RemoveAt(0);
                    SkillPost.SkillId = item;
                    SkillPost.PostId = this.Id;
                    SkillPost.Updatedate = DateTime.Now;
                    SkillPost.Updateby = UserID;
                    SkillPost.Isdeleted = false;
                    _Context.Update(SkillPost);
                    _Context.SaveChanges();
                }
            }
            else if (PrevSkillCount > NextSkillCount)
            {
                foreach (var item in this.ListSkillId)
                {
                    Skillpost SkillPost = PrevSkills.First();
                    PrevSkills.RemoveAt(0);
                    SkillPost.SkillId = item;
                    SkillPost.PostId = this.Id;
                    SkillPost.Updatedate = DateTime.Now;
                    SkillPost.Updateby = UserID;
                    SkillPost.Isdeleted = false;
                    _Context.Update(SkillPost);
                    _Context.SaveChanges();
                }
                for (int i = 0; i < PrevSkillCount - NextSkillCount; i++)
                {
                    Skillpost SkillPost = PrevSkills.First();
                    PrevSkills.RemoveAt(0);
                    if (!SkillPost.Isdeleted)
                    {
                        SkillPost.Isdeleted = true;
                        SkillPost.Deleteby = UserID;
                    }
                    _Context.Update(SkillPost);
                    _Context.SaveChanges();
                }
            }
            else
            {
                foreach (var SkillPost in PrevSkills)
                {
                    int item = this.ListSkillId.First();
                    this.ListSkillId.RemoveAt(0);
                    SkillPost.SkillId = item;
                    SkillPost.PostId = this.Id;
                    SkillPost.Updatedate = DateTime.Now;
                    SkillPost.Updateby = UserID;
                    SkillPost.Isdeleted = false;
                    _Context.Update(SkillPost);
                    _Context.SaveChanges();
                }
                for (int i = 0; i < NextSkillCount - PrevSkillCount; i++)
                {
                    Skillpost SkillPost = new Skillpost();
                    int item = this.ListSkillId.First();
                    this.ListSkillId.RemoveAt(0);
                    SkillPost.SkillId = item;
                    SkillPost.PostId = this.Id;
                    SkillPost.Createdate = DateTime.Now;
                    SkillPost.Updatedate = DateTime.Now;
                    SkillPost.Createby = this.CompanyId;
                    SkillPost.Updateby = UserID;
                    SkillPost.Deleteby = UserID;
                    SkillPost.Isdeleted = false;
                    _Context.Update(SkillPost);
                    _Context.SaveChanges();
                }
            }
            //====================================================
            //=========================================
            List<CityPost> PrevCities = new List<CityPost>();
            PrevCities = _Context.CityPosts.Where(x => x.PostId == this.Id).Select(x => x).ToList();
            int PrevCityCount = PrevCities.Count();
            int NextCityCount = this.ListCityId.Count();
            if (PrevCityCount == NextCityCount)
            {
                foreach (var item in this.ListCityId)
                {
                    CityPost cityPost = PrevCities.First();
                    PrevCities.RemoveAt(0);
                    cityPost.CityId = item;
                    cityPost.PostId = this.Id;
                    cityPost.Updatedate = DateTime.Now;
                    cityPost.Updateby = UserID;
                    cityPost.Isdeleted = false;
                    _Context.Update(cityPost);
                    _Context.SaveChanges();
                }
            }
            else if (PrevCityCount > NextCityCount)
            {
                foreach (var item in this.ListCityId)
                {
                    CityPost cityPost = PrevCities.First();
                    PrevCities.RemoveAt(0);
                    cityPost.CityId = item;
                    cityPost.PostId = this.Id;
                    cityPost.Updatedate = DateTime.Now;
                    cityPost.Updateby = this.CompanyId;
                    cityPost.Isdeleted = false;
                    _Context.Update(cityPost);
                    _Context.SaveChanges();
                }
                for (int i = 0; i < PrevCityCount - NextCityCount; i++)
                {
                    CityPost cityPost = PrevCities.First();
                    PrevCities.RemoveAt(0);
                    if (!cityPost.Isdeleted)
                    {
                        cityPost.Isdeleted = true;
                        cityPost.Deleteby = this.CompanyId;
                    }
                    _Context.Update(cityPost);
                    _Context.SaveChanges();
                }
            }
            else
            {
                foreach (var cityPost in PrevCities)
                {
                    int item = this.ListCityId.First();
                    this.ListCityId.RemoveAt(0);
                    cityPost.CityId = item;
                    cityPost.PostId = this.Id;
                    cityPost.Updatedate = DateTime.Now;
                    cityPost.Updateby = this.CompanyId;
                    cityPost.Isdeleted = false;
                    _Context.Update(cityPost);
                    _Context.SaveChanges();
                }
                for (int i = 0; i < NextCityCount - PrevCityCount; i++)
                {
                    CityPost cityPost = new CityPost();
                    int item = this.ListCityId.First();
                    this.ListCityId.RemoveAt(0);
                    cityPost.CityId = item;
                    cityPost.PostId = this.Id;
                    cityPost.Createdate = DateTime.Now;
                    cityPost.Updatedate = DateTime.Now;
                    cityPost.Createby = this.CompanyId;
                    cityPost.Updateby = this.CompanyId;
                    cityPost.Deleteby = 0;
                    cityPost.Isdeleted = false;
                    _Context.Update(cityPost);
                    _Context.SaveChanges();
                }
            }
        }
        public void DeletePost()
        {
            CompanyPost ComPost = _Context.CompanyPosts.Where(x => x.Id == this.Id).Select(x => x).FirstOrDefault();
            ComPost.Isdeleted = true;
            ComPost.Deleteby = this.CompanyId;
            _Context.Update(ComPost);
            _Context.SaveChanges();
            ComPost.PostSubmits = _Context.PostSubmits.Where(x => x.PostId == this.Id && !x.Isdeleted).Select(x => x).ToList();
            foreach(var submit in ComPost.PostSubmits)
            {
                submit.Isdeleted = true;
                submit.Deleteby = this.CompanyId;
                _Context.Update(submit); _Context.SaveChanges();
            }
        }
    }

}
