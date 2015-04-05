using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NhaTroDBFirstVer1.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Tai Khoan Khong Duoc Trong")]
        [StringLength(20, ErrorMessage = "Tai khoan it nhat 5 ky tu va nho hon 20 ky tu.", MinimumLength = 5)]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "tai khoan khong duoc dung ky tu")]
        [System.Web.Mvc.Remote("doesUserNameExist", "User", HttpMethod = "POST", ErrorMessage = "Tai khoan da ton tai. Vui long nhap ten moi")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mat khau khong duoc trong")]
        [StringLength(20, ErrorMessage = "Mat khau it nhat 5 ky tu va nho hon 20 ky tu.", MinimumLength = 5)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Nhap lai lai mat khau khong dung.")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Ho va ten khong duoc trong!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "So dien thoai khong duoc trong!")]
        [RegularExpression("[0-9]*", ErrorMessage = "so dien thoai phai la ki tu so!")]
        public string SoDienThoai { get; set; }

        [Required(ErrorMessage = "Email khong duoc trong!")]
        [RegularExpression(@"^[a-z][a-z|0-9|]*([_][a-z|0-9]+)*([.][a-z|0-9]+([_][a-z|0-9]+)*)?@[a-z][a-z|0-9|]*\.([a-z][a-z|0-9]*(\.[a-z][a-z|0-9]*)?)$",ErrorMessage="Email khong hop le!")]
        public string Email { get; set; }
    }
}