using IMine.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IMine.Controllers
{
    public class LoginController : Controller
    {
        private readonly WebAnhDBEntities db = new WebAnhDBEntities();
        // GET: Login

        #region Index

        /// <summary>
        ///     Giao diện đăng nhập
        /// </summary>
        /// <returns></returns>
        /// 
        [Route("Welcome")]
        [Route("")]
        public ActionResult Index()
        {
            return View("Login");
        }
        #endregion



        #region DangNhap

        /// <summary>
        ///     Kiểm tra đăng nhập
        /// </summary>
        /// <param name="model">Thông tin user + password người dùng nhập vào</param>
        /// <returns>Kết quả đăng nhập</returns>
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Accounts.SingleOrDefault(x => x.Username.Equals(model.username));
                if (user == null)
                {
                    ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                }
                else
                {
                    if (user.TrangThai == 0) ModelState.AddModelError("", "Xin liên hệ quản trị viên.");
                    else
                    {
                        if (user.Password.Equals(model.password))
                        {
                            Session["user"] = new Account { ID = user.ID, Username = model.username, HoTen = user.HoTen, Phone = user.Phone, GioiTinh = user.GioiTinh, NgaySinh = user.NgaySinh, Email = user.Email };
                            return RedirectToAction("Index", "Home", Session["user"]);
                        }
                        else ModelState.AddModelError("", "Tài khoản hoặc mật khẩu không đúng.");
                    }
                }
            }
            return View();
        }
        #endregion

        #region Register

        /// <summary>
        ///     Giao diện đăng ký mới
        /// </summary>        /// 
        /// <returns></returns>
        public ActionResult Register()
        {
            return View();
        }
        #endregion

        #region Register-Post

        /// <summary> 
        ///     Thông tin đăng ký
        /// </summary>
        /// 
        [HttpPost]
        public ActionResult Register(AccountModel model)
        {
            if (model.password != model.passwordComfirm)
            {
                ModelState.AddModelError("", "Mật khẩu không trùng khớp !");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    Account acc = new Account()
                    {
                        Username = model.username,
                        Password = model.password,
                        HoTen = model.name,
                        Phone = model.phone,
                        Email = model.email,
                        NgaySinh = model.dob,
                        GioiTinh = model.gen,
                        TrangThai = 1
                    };
                    db.Accounts.Add(acc);
                    db.SaveChanges();
                    return RedirectToAction("Finish");
                }

            }


            return View();
        }
        #endregion


        #region Finish

        /// <summary>
        ///     Giao diện đăng ký mới
        /// </summary>        /// 
        /// <returns>Chuyển trang đăng nhập</returns>
        /// 
        [Route("hoan-tat-dang-ky")]
        public ActionResult Finish()
        {
            return View();
        }
        #endregion

        public RedirectToRouteResult LogOut()
        {
            Session["user"] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}