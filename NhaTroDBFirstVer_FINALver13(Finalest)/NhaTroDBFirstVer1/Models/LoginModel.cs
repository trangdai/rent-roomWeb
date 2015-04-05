using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NhaTroDBFirstVer1.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "UserName Khong Duoc Trong")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "khong Duoc Dung Ky Tu Dac Biet")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password Khong Duoc Trong")]
        [RegularExpression("[a-zA-Z0-9]*", ErrorMessage = "khong Duoc Dung Ky Tu Dac Biet")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}