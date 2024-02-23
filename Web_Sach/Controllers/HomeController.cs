using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.Dao;
using Web_Sach.Models.EF;
using Web_Sach.Session;

namespace Web_Sach.Controllers
{
    public class HomeController : Controller
    {
        private WebSachDb db = new WebSachDb();
        // GET: Home
        public ActionResult Index()
        {
            var productNew = new Book();
            var slide = new SlideModel().getSilde();
            ViewBag.listProductNew = productNew.listNew(10);
            ViewBag.listProductTopHot = productNew.listTopHot(10);
            ViewBag.updateInfoUser = TempData["UserInfo"];
            return View(slide);
        }
        

        [ChildActionOnly]
        public ActionResult Menu()
        {
            var category = new ProductCategory().ListAll();
            return PartialView(category);
        }

        [ChildActionOnly]

        public ActionResult HeaderCart()
        {
            var cart = Session[SessionHelper.CART_KEY];
            var list = new List<CartItem>();
            if(cart != null)
            {
                list= cart as List<CartItem>;
            }
            return PartialView(list);

        }


        public ActionResult RateView(int id)
        {
            using(WebSachDb db = new WebSachDb())
            {
                var count = new rateView();
                var rate = (from s in db.Saches
                           join cm in db.Comments on s.ID equals cm.MaSach
                           where cm.MaSach == id && cm.Rate != 0
                           select cm.Rate).ToList();
                if(rate.Count() >0)
                {
                    count.rateCount = rate.Average();
                }
                return PartialView(count);
            }
        }

        public ActionResult RateViewProductDetail(int id)
        {
            using (WebSachDb db = new WebSachDb())
            {
                var count = new rateView();
                var rate = (from s in db.Saches
                            join cm in db.Comments on s.ID equals cm.MaSach
                            where cm.MaSach == id && cm.Rate != 0
                            select cm.Rate).ToList();
                count.countComment = rate.Count();
                if (count.countComment >0)
                {
                    count.rateCount = rate.Average();
                   
                }
                return PartialView(count);
            }
        }





    }
}