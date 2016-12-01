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

        #region Index

        /// <summary>
        ///     Trang chủ của IMine
        /// </summary>
        /// <returns>Thông tin hình ảnh của người dùng</returns>
        public ActionResult Index()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var user = Session["user"] as Account;
                
                ViewBag.folder = db.Albums.Where(x => x.UserID == user.ID).OrderByDescending(x => x.AlbumID).Select(x => x.TenAlbum).ToList();
                ViewBag.img = db.HinhAnhs.Where(x => x.UserID == user.ID).OrderByDescending(x => x.HinhID).Select(x => x.TenHinh).ToList();
                return View();
            }
        }
        #endregion

        #region FolderLoad

        /// <summary>
        ///     Hiển thị thông tin folder
        /// </summary>
        /// <param name="id">Compare product id with resource</param>
        /// <param name="quantity">Get product quantity</param>
        /// <returns>Thông tin folder của người dùng</returns>
        public ActionResult FolderLoad()
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var user = Session["user"] as Account;

                ViewBag.folder = db.Albums.Where(x => x.UserID == user.ID).Select(x => x.TenAlbum).ToList();
                ViewBag.img = db.HinhAnhs.Where(x => x.UserID == user.ID).Select(x => x.TenHinh).ToList();
                return View();
            }
        }
        #endregion


        #region Upload

        /// <summary>
        ///     Upload hình ảnh
        /// </summary>
        /// <param name="files">Hình ảnh upload</param>
        /// <returns>Lưu vào db</returns>
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
        #endregion

        #region CreateFolder

        /// <summary>
        ///     Tạo folder mới
        /// </summary>
        /// <param name="model">Thông tin folder( tên, chú thích)</param>
        /// <returns>Folder mới + tên</returns>
        [HttpPost]
        public ActionResult CreateFolder(FolderModel model)
        {
            var user = Session["user"] as Account;
            Album ab = new Album()
            {
                TenAlbum = model.Fname,
                UserID = user.ID
            };

            db.Albums.Add(ab);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }    }
}