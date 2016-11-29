using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IMine.Models
{
    public class FolderModel
    {
        [Required(ErrorMessage = "Xin nhập tên thư mục")]
        public string Fname { get; set; }

        public string ChuThich { get; set; }
    }
}