using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;
using Web_Sach.Session;


namespace Web_Sach.Controllers
{
    [Authorize]
    public class ReViewController : Controller
    {

        // GET: ReView
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult _ReView(int productId)
        {
            ViewBag.ProductId = productId;
            using (WebSachDb db = new WebSachDb())
            {
                var sessionUser = Session[SessionHelper.USER_KEY];
                if (sessionUser != null)
                {
                    var item = from rv in db.ReViews 
                               join s in db.Saches on rv.MaSach equals s.ID
                               join tk in db.TaiKhoans on rv.MaKH equals tk.ID
                               select new reViewModel
                               {
                                   Email = tk.Email,
                                   FullName = tk.FullName,
                                   taiKhoan=tk.TaiKhoan1
                               };
                    return PartialView(item.FirstOrDefault());
                }
            }

            return PartialView();
        }
        public ActionResult _Load_Review(int productId)
        {
            using(WebSachDb db = new WebSachDb())
            {
                var item = db.ReViews.Where(x=>x.MaSach == productId).OrderByDescending(x=>x.ID).ToList();
                return PartialView(item);
            }
        }
    
        public ActionResult PostReView(ReView req)
        {
            if(ModelState.IsValid)
            {
                
            }
            return Json(true);
        }
    }
}