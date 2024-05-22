using BotDetect.Web;
using BotDetect.Web.Mvc;
using CKFinder.Connector;
using Facebook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;
using Web_Sach.Session;
using GoogleAuthentication.Services;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;
using Microsoft.AspNet.Identity;


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

        public ActionResult LoignClients()
        {
            
            var clientId = "805765781650-aaelc156djulnfc7oe7n8ldcpa8mb7k5.apps.googleusercontent.com";
            var url = "https://localhost:44377/User/GoogleCallBack";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.response = response;
            return View();
        }

        public async Task<ActionResult> GoogleCallBack(string code)
        {
            var clientId = "805765781650-aaelc156djulnfc7oe7n8ldcpa8mb7k5.apps.googleusercontent.com";
            var url = "https://localhost:44377/User/GoogleCallBack";
            var clientsecret = "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";
            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientsecret, url);
            if (token == null)
            {
                return Redirect("LoignClients");
            }
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken);
            if (userProfile != null)
            {
                var userGooGle = JObject.Parse(userProfile);
                string email = userGooGle["email"].ToString();
                //string family_name = userGooGle["family_name"].ToString();
                string give_name = userGooGle["given_name"].ToString();
                var userCheck = db.TaiKhoans.Where(x => x.Email == email).FirstOrDefault();
                // lưu thông tin vào session 
                if (userCheck == null)
                {// chưa tồn tại tk
                    var user = new TaiKhoan();
                    user.TaiKhoan1 = give_name;
                    //   user.Password = "123";
                    user.Email = email;
                    //   user.Address = "NULL";
                    //   user.Phone = "NULL";
                    user.GioiTinh = "Nam";

                    user.Role = 0;
                    user.Status = true;

                    user.NgaySinh = DateTime.Now;

                    user.FullName = give_name;
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
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {//đã tồn tại tk
                    var userSession = new UserLoginSession();
                    userSession.UserID = userCheck.ID;
                    userSession.UserName = userCheck.TaiKhoan1;
                    userSession.FullName = userCheck.FullName;
                    userSession.Address = userCheck.Address;
                    userSession.Phone = userCheck.Phone;
                    userSession.Email = userCheck.Email;
                    Session.Add(SessionHelper.USER_KEY, userSession);
                    return RedirectToAction("Index", "Home");
                }


            }
            return Redirect("LoignClients");
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
                var userCheck = db.TaiKhoans.Where(x => x.Email == email).FirstOrDefault();
                if (userCheck == null)
                {// tài khoản f chưa tồn tại

                    string firstName = me.first_name;
                    string middleName = me.middle_name;
                    string lastName = me.last_name;
                    var user = new TaiKhoan();
                    user.TaiKhoan1 = firstName + " " + middleName + " " + lastName;
                    user.Email = email;
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
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {// tk facebook đã tồn tại
                    var userSession = new UserLoginSession();
                    userSession.UserID = userCheck.ID;
                    userSession.UserName = userCheck.TaiKhoan1;
                    userSession.FullName = userCheck.FullName;
                    userSession.Address = userCheck.Address;
                    userSession.Phone = userCheck.Phone;
                    userSession.Email = userCheck.Email;
                    Session.Add(SessionHelper.USER_KEY, userSession);
                    return RedirectToAction("Index", "Home");
                }
            }
            return Redirect("LoignClients");
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

        public ActionResult Register()
        {
           
            setViewBagTinh();
            return View();
        }
        public void setViewBagTinh(int? selectedId = null)
        {
            var drowdoad = db.provinces.ToList();
            ViewBag.MaTinh = new SelectList(drowdoad, "code", "full_name", selectedId);// hiện thị droplisst theo Name
        }

        public JsonResult loadHuyen(string tinhId)
        {
            return Json(db.districts.Where(x => x.province_code == tinhId).Select(x => new
            {
                id = x.code,
                name = x.full_name
            }).ToList(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult loadXa(string huyenId)
        {
            return Json(db.wards.Where(x => x.district_code == huyenId).Select(x => new
            {
                id = x.code,
                name = x.full_name
            }).ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Register(Register model)
        { 
           
            if (ModelState.IsValid)
            { 
                if (model.MaHuyen != "-1" && model.MaXa != "-1")
                {
                    var userNameUnique = from tk in db.TaiKhoans

                                         select tk;
                    if (userNameUnique.Where(x => x.TaiKhoan1 == model.UserName).FirstOrDefault() != null)
                    {// đã tồn tại tên tài khoản
                        ModelState.AddModelError("UserName", "Tên tài khoản đã tồn tại");
                        setViewBagTinh();
                        return View(model);

                    }
                    else if (!model.Password.Any(char.IsUpper) || !model.Password.Any(char.IsDigit) || model.Password.Length < 8)
                    {


                        ModelState.AddModelError("Password", "Mật khẩu  chứa ít nhất 1 chữ viết hoa , 1 số và hơn 8 ký tự");

                        setViewBagTinh();
                        return View(model);

                    }
                    else if (userNameUnique.Where(x => x.Email == model.Email).FirstOrDefault() != null)
                    {//đã tồn tại email
                        ModelState.AddModelError("Email", "Email đã tồn tại");
                        setViewBagTinh();
                        return View(model);

                    }
                  
                    var tinh = db.provinces.Find(model.MaTinh).full_name;
                    var huyen = db.districts.Find(model.MaHuyen).full_name;
                    var xa = db.wards.Find(model.MaXa).full_name;
                    var user = new TaiKhoan();
                    user.TaiKhoan1 = model.UserName;
                    user.Password = model.Password;
                    user.Email = model.Email;
                    user.FullName = model.Name;
                    user.Address = model.Address + "-" + xa + "-" + huyen + "-" + tinh;
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
                        setViewBagTinh();
                        return View();
                    }
                    else
                    {
                        setViewBagTinh();
                        ModelState.AddModelError("", "Đăng ký thất bại");

                    }
                }
            }
            setViewBagTinh();
            return View(model);


        }

        public ActionResult UserSession()
        {
           // ViewBag.ErrorMessage = TempData["AlertContent"];
            return View();
        }
       
        /// <summary>
        /// cập nhật thông tin
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
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
                    TempData["AlertContent"] = "Tên tài khoản đã tồn tại";
                    TempData["AlertMessage"] = "alert-danger";
                    return Redirect("UserSession");
                }

                if (pasOld.Password != tk.Password)
                {
                    var password = tk.Password;

                    if (!password.Any(char.IsUpper) || !password.Any(char.IsDigit) || password.Length < 8)
                    {
                        //ModelState.AddModelError("Password", "Mật khẩu có độ dài lớn hơn 8 và chứa ít nhất 1 chữ viết hoa và 1 số");
                        TempData["AlertContent"] = "Mật khẩu có độ dài lớn hơn 8 và chứa ít nhất 1 chữ viết hoa và 1 số";
                        TempData["AlertMessage"] = "alert-danger";
                        return Redirect("UserSession");
                        
                    }
                }
                if (userUpdate.UpdateUser(tk))
                {
                    // TempData["UserInfo"] = "Cập nhật tài khoản thành công!";
                    TempData["AlertContent"] = "Cập nhật tài khoản thành công";
                    TempData["AlertMessage"] = "alert-success";
                    return Redirect("UserSession");
                   // return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertContent"] = "Cập nhật thất bại";
                    TempData["AlertMessage"] = "alert-danger";
                    return Redirect("UserSession");
                   //ModelState.AddModelError("", "Cập nhật thất bại");
                }
            }
            TempData["AlertContent"] = "Thông tin không được để trống";
            TempData["AlertMessage"] = "alert-danger";
            return Redirect("UserSession");
           // return View(tk);
        }


    }
}