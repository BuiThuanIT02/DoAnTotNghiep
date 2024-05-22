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
           // ViewBag.updateInfoUser = TempData["UserInfo"];
            return View(slide);
        }

        [ChildActionOnly]
        public ActionResult BookNew() 
        {

            var productNew = new Book();
            ViewBag.listProductNew = productNew.listNew(10);
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult BookTopHot()
        {

            var productNew = new Book();
            ViewBag.listProductTopHot = productNew.listTopHot(10);
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult FlashSale()
        {
            var productFlashSale = from s in db.Saches
                                   join km in db.KhuyenMai_Sach on s.ID equals km.MaSach
                                   join k in db.KhuyenMais on km.MaKhuyenMai equals k.ID
                                   select new FlashSale
                                   {
                                       sach =s,
                                       NgayBatDau = k.NgayBatDau,
                                       NgayKeThuc=k.NgayKeThuc

                                   };
           
            var dateEnd = productFlashSale.Select(x => x.NgayKeThuc).FirstOrDefault();
            ViewBag.DateEnd = dateEnd;
            ViewBag.listProductNew = productFlashSale.ToList();
            return PartialView();
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