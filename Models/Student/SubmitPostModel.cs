using Microsoft.AspNetCore.Http;
using PBL3.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PBL3.Models.Student
{
    public class SubmitPostModel
    {
        public int PostID { get; set; }
        [Required(ErrorMessage ="Hãy cho doanh nghiệp biết tóm tắt thông tin về bạn.")]
        [MaxLength(1000, ErrorMessage ="Tối đa 1000 kí tự"), MinLength(200,ErrorMessage = "Tối thiểu 200 kí tự")]
        [Display(Name = "Tóm tắt về bản thân")]
        public string ShortText { get; set; }
        public string OldCVPath { get; set; }
        public string NewCVPath { get; set; }
        [FileExtensions(Extensions = "doc,docx,pdf", ErrorMessage = "Định dạng file không hợp lệ")]
        public IFormFile CV { get; set; }
        [Display(Name = "Bạn đã có sẵn CV")]
        public bool CheckOldCVExist { get; set; }
        [Display(Name ="Sử dụng CV mới.")]
        public bool IsNewCV { get; set; }
        public SubmitPostModel()
        {
            CheckOldCVExist = true;
        }
    }
}
