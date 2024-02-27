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
using Microsoft.AspNet.Identity.Owin;
using Web_Sach.Models.Dao;
using Web_Sach.App_Start;
using System.Net;
using Microsoft.Owin.Host.SystemWeb;


namespace Web_Sach.Controllers
{
    public class UserController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public UserController()
        {
        }

        public UserController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
               
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {

                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }




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
                string family_name = userGooGle["family_name"].ToString();
                string give_name = userGooGle["given_name"].ToString();

                // lưu thông tin vào session 
                var user = new TaiKhoan();
                user.TaiKhoan1 = family_name + " " + give_name;
                user.Password = "123";
                user.Email = email;
                user.Address = "NULL";
                user.Phone = "NULL";
                user.GioiTinh = "Nam";

                user.Role = 0;
                user.Status = true;

                user.NgaySinh = DateTime.Now;

                user.FullName = family_name + " " + give_name;
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

        public async Task<ActionResult> ChooseEmail(string code)
        {
            var clientId = "805765781650-aaelc156djulnfc7oe7n8ldcpa8mb7k5.apps.googleusercontent.com";
            var url = "https://localhost:44377/User/ChooseEmail";
            var clientsecret = "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";
            var token = await GoogleAuth.GetAuthAccessToken(code, clientId, clientsecret, url);
            if (token == null)
            {
                return Redirect("/dang-ky");
            }
            var userProfile = await GoogleAuth.GetProfileResponseAsync(token.AccessToken);
            if (userProfile != null)
            {
                var userGooGle = JObject.Parse(userProfile);
                string email = userGooGle["email"].ToString();
             
                TempData["Email"] = email;
            }
            return Redirect("/dang-ky");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(UserLoginClients model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    var mdUser = UserManager.FindByEmail(model.Email);
                    if (!mdUser.EmailConfirmed)
                    {
                        HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
                        return View("NottificationEmailConfirm");
                    }
                    else
                    {

                    }
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid login attempt.");
                    return View(model);
            }
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Register()
        {
      
            var clientId = "805765781650-aaelc156djulnfc7oe7n8ldcpa8mb7k5.apps.googleusercontent.com";
            var url = "https://localhost:44377/User/ChooseEmail";
            var response = GoogleAuth.GetAuthUrl(clientId, url);
            ViewBag.response = response;
            return View();
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        [HttpPost]
       
        public async Task<ActionResult> RegisterPost(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email,EmailConfirmed = false};
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    var callbackUrl = Url.Action("ConfirmEmail", "User", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    Web_Sach.Common.common.SendMailRgister(user.Email, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>", "");
                }
                AddErrors(result);
                return View("NottificationEmailConfirm");
            }

            return View(model);


        }

        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
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