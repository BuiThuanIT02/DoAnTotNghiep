using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        private WebSachDb db = new WebSachDb();
        private int page = 1;
        private int pageSize = 50;
        public ActionResult Index()
        {
            var order = new Order().listPage(page, pageSize);
            return View(order);
        }

        public ActionResult OrderPackAd()
        {
            var order = new Order().listPagePackAd(page, pageSize);
            return View(order);
        }

        public ActionResult OrderTransportAd()
        {
            var order = new Order().listPageTransportAd(page, pageSize);
            return View(order);
        }
        public ActionResult OrderSuccessAd()
        {
            var order = new Order().listPageSuccessAd(page, pageSize);
            return View(order);
        }
        public ActionResult OrderFailureAd()
        {
            var order = new Order().listPageFailureAd(page, pageSize);
            return View(order);
        }
        // xem chi tiết

        public ActionResult OrderDetail(int id)
        {
          
            var order = db.DonHangs.Find(id);
            ViewBag.order = order;
            var detail = from dh in db.DonHangs
                         join dt in db.ChiTietDonHangs on dh.ID equals dt.MaDonHang
                         join sach in db.Saches on dt.MaSach equals sach.ID
                         where dh.ID == id
                         select new OrderDetail()
                         {
                             sachId = sach.ID,
                             sachName = sach.Name,
                             PriceBuy = (double)dt.Price,                           
                             QuantityBuy = (int)dt.Quantity
                         };
            var voucherOrder = from dh in db.DonHangs
                               join v in db.Vouchers on dh.MaVoucher equals v.ID
                               where dh.ID == id
                               select new VoucherOrder()
                               {
                                   maVoucher = dh.MaVoucher,
                                   SoTienGiams = v.SoTienGiam
                               };
            ViewBag.voucherOrder = voucherOrder.FirstOrDefault();
            return View(detail.ToList());
        }

   
        // thay doi trang thai
        [HttpPost]
        
        public ActionResult ChangeStatusPending(int id)
        {// thay đổi trạng thái pending
            var order = db.DonHangs.Find(id);
            if (order != null)
            {
                order.Status = 2;
                db.SaveChanges();
                return Json(new {status=true});
            }

            return Json(new { status = false });

        }
        [HttpPost]
        public ActionResult ChangeStatusPack(int id)
        {// thay đổi trạng thái pack
            var order = db.DonHangs.Find(id);
            if (order != null)
            {
                order.Status = 3;
                order.NgayGiao= DateTime.Now;
                db.SaveChanges();
                return Json(new { status = true });
            }

            return Json(new { status = false });

        }


        [HttpPost]
        public JsonResult RemoveOrder(int id)
        {
            var order = db.DonHangs.Find(id);
            if (order != null)
            {
                order.Status = 5;
                order.NgayGiao = DateTime.Now;
                db.SaveChanges();
                return Json(new
                {
                    status = true
                });

            }
            return Json(new
            {
                status = false
            });




        }




    }
}