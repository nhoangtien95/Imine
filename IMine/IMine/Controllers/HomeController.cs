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
        /// 
        [Route("IMine")]
        public ActionResult Index(int? F_id = null)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                var user = Session["user"] as Account;
                //if (TempData["fid"] != null)
                //{
                //    TempData["fid"] = TempData["fid"];
                //}
                TempData["fid"] = F_id;
                ViewBag.F_id = F_id;
                ViewBag.folder = db.Albums.Where(x => x.UserID == user.ID && x.AlbumCha == F_id).ToList();
                ViewBag.img = db.HinhAnhs.Where(x => x.UserID == user.ID && x.AlbumID == F_id).ToList();

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
        /// 
        [Route("Folder/{F_id}")]
        [HttpPost]
        public ActionResult _FolderLoad(int F_id)
        {
            if (Session["user"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                TempData["fid"] = F_id;
                return RedirectToAction("abc");
            }
        }
        #endregion
        [Route("abc")]
        public PartialViewResult abc()
        {
            var user = Session["user"] as Account;
            int? F_id;
            if (TempData["fid"] == null)
            {
                F_id = null;
            }
            else
            {
                F_id = Convert.ToInt32(TempData["fid"]);
                TempData["fid"] = TempData["fid"];
            }


            ViewBag.F_id = F_id;
            ViewBag.folder = db.Albums.Where(x => x.UserID == user.ID && x.AlbumCha == F_id).ToList();
            ViewBag.img = db.HinhAnhs.Where(x => x.UserID == user.ID && x.AlbumID == F_id).ToList();

            return PartialView();
        }

        #region Upload

        /// <summary>
        ///     Upload hình ảnh
        /// </summary>
        /// <param name="files">Hình ảnh upload</param>
        /// <returns>Lưu vào db</returns>
        [HttpPost]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files, string F_id)
        {
            var ktHinh = new[] { ".png", ".jpg", ".jpeg" };
            var user = Session["user"] as Account;
            foreach (var item in files)
            {
                var fileName = Path.GetFileName(item.FileName);
                var ext = Path.GetExtension(item.FileName);

                if (ktHinh.Contains(ext))
                {
                    string name = Path.GetFileNameWithoutExtension(fileName);
                    string userImage = name + ext;
                    if (F_id == "0")
                    {
                        HinhAnh img = new HinhAnh()
                        {
                            TenHinh = name + ext,
                            UserID = user.ID,
                            AlbumID = null
                        };
                        db.HinhAnhs.Add(img);
                    }
                    else
                    {
                        HinhAnh img = new HinhAnh()
                        {
                            TenHinh = name + ext,
                            UserID = user.ID,
                            AlbumID = int.Parse(F_id)
                        };
                        db.HinhAnhs.Add(img);
                    }

                    db.SaveChanges();

                    item.SaveAs(Server.MapPath("~/Photos/" + userImage));
                }
            }
            if (F_id == "0") { F_id = null; }
            return RedirectToAction("Index", new { F_id = F_id });
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
                UserID = user.ID,
                AlbumCha = model.F_id
            };

            db.Albums.Add(ab);
            db.SaveChanges();
            return RedirectToAction("Index", new { F_id = model.F_id });
        }
        #endregion
        [HttpPost]
        [Route("delete")]
        public RedirectToRouteResult Delete(int?[] FolderValues, int?[] ImgValues)
        {
            if (FolderValues != null)
            {
                foreach (var folder in FolderValues)
                {
                    Album album = db.Albums.Where(x => x.AlbumID == folder).FirstOrDefault();
                    db.Albums.Remove(album);
                }
            }

            if (ImgValues != null)
            {
                foreach (var img in ImgValues)
                {
                    HinhAnh image = db.HinhAnhs.Where(x => x.HinhID == img).FirstOrDefault();
                    db.HinhAnhs.Remove(image);
                }
            }

            db.SaveChanges();
            return RedirectToAction("abc");
        }

        public ActionResult SlideBar()
        {
            var user = Session["user"] as Account;
            ViewBag.HeadFolder = db.Albums.Where(x => x.AlbumCha == null && x.UserID == user.ID).ToList();
            return View();
        }

        public ActionResult SlideBarSelect(int? F_id)
        {
            return RedirectToAction("Index", new { F_id = F_id });
        }

        public ActionResult UserProfile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserProfile(AccountModel model)
        {
            if (model.password == null || model.passwordComfirm == null)
            {
                ModelState.AddModelError("", "Vui lòng không để trống Password");
            }
            else
            {
                if (model.password != model.passwordComfirm)
                {
                    ModelState.AddModelError("", "Mật khẩu không trùng khớp");
                }
                else
                {
                    //Account ac = new Account()
                    //{
                    //    Username = model.username,
                    //    Password = model.password,
                    //    HoTen = model.name,
                    //    Phone = model.phone,
                    //    Email = model.email,
                    //    NgaySinh = model.dob,
                    //    GioiTinh = model.gen,
                    //    TrangThai = 1
                    //};
                    var ac = db.Accounts.Where(x => x.ID == model.id).FirstOrDefault();
                    ac.Phone = model.phone;
                    ac.Password = model.password;
                    ac.GioiTinh = model.gen;

                    db.Entry(ac).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
    }
}