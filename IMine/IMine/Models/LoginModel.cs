using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMine.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string username { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string password { get; set; }
    }
}