using BotDetect.Web;
using BotDetect.Web.Mvc;
using CKFinder.Connector;
using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;
using Web_Sach.Session;

namespace Web_Sach.Controllers
{
    public class UserController : Controller
    {
        WebSachDb db = new WebSachDb();
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }
        public ActionResult Register()
        {
            return View();
        }


        public ActionResult LoignClients()
        {
            return View();
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }
        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code,
            });
            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string firstName = me.first_name;
                string middleName = me.middle_name;
                string lastName = me.last_name;
                var user = new TaiKhoan();
                user.TaiKhoan1 = firstName + " " + middleName + " " + lastName;
                user.Password = "123";
                user.Email = email;
                user.Address = "NULL";
                user.Phone = "NULL";
                user.GioiTinh = "Nam";

                user.Role = 0;
                user.Status = true;

                user.NgaySinh = DateTime.Now;

                user.FullName = firstName + " " + middleName + " " + lastName;
                var resultInsert = new TaiKhoanModels().InserForFaceBook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLoginSession();
                    userSession.UserID = user.ID;
                    userSession.UserName = user.TaiKhoan1;
                    userSession.FullName = user.FullName;
                    userSession.Address = user.Address;
                    userSession.Phone = user.Phone;
                    userSession.Email = user.Email;
                    Session.Add(SessionHelper.USER_KEY, userSession);

                }

            }
            return RedirectToAction("Index", "Home");

        }
        [HttpPost]

        public ActionResult LoignClients(UserLoginClients model)
        {
            if (ModelState.IsValid)
            {

                var user = new LoginModels().CheckLogin(model.UserName, model.Password);
                if (user == -1)
                {
                    ModelState.AddModelError("UserName", "Tài khoản không tồn tại");
                    return View(model);
                }
                else if (user == 0)
                {
                    ModelState.AddModelError("UserName", "Tài khoản bị khóa");
                    return View(model);
                }
                else if (user == 3)
                {
                    // Điều hướng sang trang đăng nhập của admin
                    // Response.Redirect("~/Admin/Login/Index"); 
                    return Redirect("~/Admin/Login/Index");
                }
                else if (user == 1)
                { // đăng nhập thành công lưu luôn vào session
                    var taiKhoan = new TaiKhoanModels().GetUserName(model.UserName);
                    var userSession = new UserLoginSession();
                    userSession.UserID = taiKhoan.ID;
                    userSession.UserName = taiKhoan.TaiKhoan1;
                    userSession.FullName = taiKhoan.FullName;
                    userSession.Address = taiKhoan.Address;
                    userSession.Phone = taiKhoan.Phone;
                    userSession.Email = taiKhoan.Email;


                    Session.Add(SessionHelper.USER_KEY, userSession);


                    return RedirectToAction("Index", "Home");
                }
                else if (user == 2 || user == 4)
                {
                    ModelState.AddModelError("UserName", "Tài khoản hoặc mật khẩu không đúng");
                }
            }
            return View(model);
        }


        public ActionResult Logout()
        {
            Session[SessionHelper.USER_KEY] = null;
            Session[SessionHelper.CART_KEY] = null;
            Session[SessionHelper.VOUCHER_KEY] = null;
            return Redirect("/");
        }






        [HttpPost]
        //  [CaptchaValidationActionFilter("CaptchaCode", "registerCaptcha", "Mã xác nhận chưa đúng!")]
        public ActionResult Register(Register model)
        {

            //  string pass = model.Password;

            if (ModelState.IsValid)
            {


                var userNameUnique = from tk in db.TaiKhoans

                                     select tk;
                if (userNameUnique.Where(x => x.TaiKhoan1 == model.UserName).FirstOrDefault() != null)
                {// đã tồn tại tên tài khoản
                    ModelState.AddModelError("UserName", "Tên tài khoản đã tồn tại");
                    return View(model);

                }
                else if (!model.Password.Any(char.IsUpper) || !model.Password.Any(char.IsDigit) || model.Password.Length < 8)
                {


                    ModelState.AddModelError("Password", "Mật khẩu  chứa ít nhất 1 chữ viết hoa , 1 số và hơn 8 ký tự");

                    //    model.Password = pass;
                    return View(model);

                }
                else if (userNameUnique.Where(x => x.Email == model.Email).FirstOrDefault() != null)
                {//đã tồn tại email
                    ModelState.AddModelError("Email", "Email đã tồn tại");
                    return View(model);

                }
                var user = new TaiKhoan();
                user.TaiKhoan1 = model.UserName;
                user.Password = model.Password;
                user.Email = model.Email;
                user.FullName = model.Name;
                user.Address = model.Address;
                user.Phone = model.Phone;
                user.GioiTinh = model.GioiTinh;
                user.Role = 0;
                user.Status = true;
                user.NgaySinh = model.NgaySinh;

                db.TaiKhoans.Add(user);
                db.SaveChanges();
                if (user.ID > 0)
                {
                    ViewBag.Success = "Đăng ký thành công!!";
                    //model = null;
                    ModelState.Clear();
                    model.UserName = string.Empty;
                    model.Password = string.Empty;
                    model.Email = string.Empty;
                    model.Name = string.Empty;
                    model.Address = string.Empty;
                    model.Phone = string.Empty;
                    model.GioiTinh = "Nam";

                    //return RedirectToAction("LoignClients","User");
                    return View();

                }
                else
                {
                    ModelState.AddModelError("", "Đăng ký thất bại");

                }


            }

            return View(model);


        }

        public ActionResult UserSession()
        {
            return View();
        }
        // cập nhật thông tin
        [ChildActionOnly]
        public ActionResult UpdateInfo(int userId)
        {
            var user = db.TaiKhoans.Find(userId);

            return View(user);
        }

        [HttpPost]

        public ActionResult UpdateInfo(TaiKhoan tk)
        {

            if (ModelState.IsValid)
            {
                var userUpdate = new TaiKhoanModels();
                // so sánh mật khẩu cũ và mật khẩu mới
                var pasOld = userUpdate.EditUser(tk.ID);
                if (userUpdate.Compare(tk))
                {
                    ModelState.AddModelError("TaiKhoan1", "Tên tài khoản đã tồn tại");
                    return View(tk);
                }

                if (pasOld.Password != tk.Password)
                {
                    var password = tk.Password;

                    if (!password.Any(char.IsUpper) || !password.Any(char.IsDigit) || password.Length < 8)
                    {
                        ModelState.AddModelError("Password", "Mật khẩu có độ dài lớn hơn 8 và chứa ít nhất 1 chữ viết hoa và 1 số");
                        return View(tk);
                    }

                }
                if (userUpdate.UpdateUser(tk))
                {

                    TempData["UserInfo"] = "Cập nhật thành công!";

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Cập nhật thất bại");
                }


            }

            return View(tk);
        }


















    }
}