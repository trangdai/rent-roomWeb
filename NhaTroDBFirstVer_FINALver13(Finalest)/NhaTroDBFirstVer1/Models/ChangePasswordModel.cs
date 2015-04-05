using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NhaTroDBFirstVer1.Models
{
    public class ChangePasswordModel
    {
        [Required(ErrorMessage = "mat khau cu khong duoc de trong")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "mat khau moi khong duoc de trong")]
        [StringLength(20, ErrorMessage = "mat khau moi phai it nhat la 5 ky tu va nho hon 20 ky tu", MinimumLength = 5)]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "Mat khau khong duoc dung ky tu dac biet")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "mat khau nhap lai khong chinh xac")]
        public string ConfirmPassword { get; set; }
    }
}