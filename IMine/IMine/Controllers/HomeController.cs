using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IMine.Models;

namespace IMine.Controllers
{
    public class HomeController : Controller
    {
        private WebAnhDBEntities db = new WebAnhDBEntities();
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var user = Session["user"] as Account;
                ViewBag.img = db.HinhAnhs.Where(x => x.UserID == user.ID).Select(x => x.TenHinh).ToList();
                return View();
            }
        }


        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            var ktHinh = new[] { ".png", ".jpg", "jpeg"};
            var user = Session["user"] as Account;
            foreach (var item in files)
            {
                var fileName = Path.GetFileName(item.FileName);
                var ext = Path.GetExtension(item.FileName);

                if (ktHinh.Contains(ext))
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string userImage = name + ext;

                    HinhAnh img = new HinhAnh()
                    {
                        TenHinh = name + ext,
                        UserID = user.ID 
                    };
                    db.HinhAnhs.Add(img);
                    db.SaveChanges();

                    item.SaveAs(Server.MapPath("~/Photos/" + userImage));
                }
            }



                return RedirectToAction("Index");
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }    }
}