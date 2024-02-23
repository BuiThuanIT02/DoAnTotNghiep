using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Session;

namespace Web_Sach.Controllers
{
    public class OrderController : Controller
    {
        WebSachDb db = new WebSachDb();
        // GET: Order
        public ActionResult Index()
        {
            return View();
        }



        public JsonResult RemoveOrder(int id)
        {

            var order = db.DonHangs.Find(id);
            if (order != null)
            {
                var orderDetail = db.ChiTietDonHangs.Where(x => x.MaDonHang == id).ToList();
               
                    foreach (var item in orderDetail)
                    {// cập nhật số lượng
                        var sachUpdate = db.Saches.Find(item.MaSach);// cập nhật lại số lượng sản phẩm
                        sachUpdate.Quantity = sachUpdate.Quantity + item.Quantity;
                    } 
                    db.ChiTietDonHangs.RemoveRange(orderDetail);
                

                db.DonHangs.Remove(order);

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



        // nhận hàng
        [HttpPost]
        public JsonResult GetOrder(int id)
        {
            var order = db.DonHangs.Find(id);
            if (order != null)
            {
                order.Status = 4;
                order.DaThanhToan = 1;
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

        [ChildActionOnly]
        // đơn hàng thành công
        public ActionResult OrderSuccess()
        {
            var orderSuccess = (List<DonHang>)TempData["OrderSuccess"];

            if (orderSuccess.Count() > 0)
            {
                return PartialView(orderSuccess);
            }
            return PartialView();
        }
        [ChildActionOnly]
        // đơn hàng thất bại
        public ActionResult OrderFailure()
        {
            var orderSuccess = (List<DonHang>)TempData["OrderFailure"];

            if (orderSuccess.Count() > 0)
            {
                return PartialView(orderSuccess);
            }
            return PartialView();
        }
        [ChildActionOnly]
        // đơn hàng chờ xử lý
        public ActionResult OrderPending()
        {
            var orderSuccess = (List<DonHang>)TempData["OrderPending"];

            if (orderSuccess.Count() > 0)
            {
                return PartialView(orderSuccess);
            }
            return PartialView();
        }



        [ChildActionOnly]
        // đơn hàng chờ đóng gói
        public ActionResult OrderPack()
        {
            var orderSuccess = (List<DonHang>)TempData["OrderPack"];

            if (orderSuccess.Count() > 0)
            {
                return PartialView(orderSuccess);
            }
            return PartialView();
        }

        [ChildActionOnly]
        // đơn hàng chờ vận chuyển
        public ActionResult OrderTransport()
        {
            var orderSuccess = (List<DonHang>)TempData["OrderTransport"];

            if (orderSuccess.Count() > 0)
            {
                return PartialView(orderSuccess);
            }
            return PartialView();
        }






    }
}