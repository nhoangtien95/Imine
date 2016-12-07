using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMine.Models
{
    public class AccountModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string username { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Xin nhập trên 3 kí tự và dưới 30 kí tự")]
        public string password { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        [Compare("password", ErrorMessage = "Mật khẩu không trùng khớp !")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "Xin nhập trên 3 kí tự và dưới 30 kí tự")]
        public string passwordComfirm { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string email { get; set; }
        public string name { get; set; }
        public string dob { get; set; }        
        public string gen { get; set; }
        public string phone { get; set; }
    }
}