﻿using System;
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
                return RedirectToAction("Index");
                //return RedirectToAction("abc");
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
            TempData["fid"] = null;
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
            var ktHinh = new[] { ".png", ".jpg", "jpeg" };
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
                        UserID = user.ID,
                        AlbumID = int.Parse(F_id)
                    };
                    db.HinhAnhs.Add(img);
                    db.SaveChanges();

                    item.SaveAs(Server.MapPath("~/Photos/" + userImage));
                }
            }

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
            foreach (var folder in FolderValues)
            {
                Album album = db.Albums.Where(x => x.AlbumID == folder).FirstOrDefault();
                db.Albums.Remove(album);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}