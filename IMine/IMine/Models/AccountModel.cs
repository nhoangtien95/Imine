using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMine.Models
{
    public class AccountModel
    {
        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string username { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string password { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string passwordComfirm { get; set; }

        [Required(ErrorMessage = "Vui lòng không để trống")]
        public string email { get; set; }
        public string name { get; set; }
        public string dob { get; set; }        
        public string gen { get; set; }
    }
}