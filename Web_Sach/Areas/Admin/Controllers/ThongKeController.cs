using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class ThongKeController : BaseController
    {
        // GET: Admin/ThongKe
       
        public ActionResult Index()
        {
            return View(); 
        }

         public JsonResult GetStatistical(string fromDate, string toDate)
        {
            using (WebSachDb db = new WebSachDb())
            {
                var query = from o in db.DonHangs
                            join od in db.ChiTietDonHangs on o.ID equals od.MaDonHang
                            join p in db.Saches on od.MaSach equals p.ID
                            select new
                            {
                                CreatedDate = o.NgayDat,
                                Quantitys = od.Quantity,
                                Prices = od.Price,
                                OriginalPrice = p.Price

                            };

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
                    query = query.Where(x => x.CreatedDate >= startDate);
                }

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
                    query = query.Where(x => x.CreatedDate < endDate);
                }


                var resulf = query.GroupBy(x => DbFunctions.TruncateTime(x.CreatedDate)).Select(x => new
                {
                    Date = x.Key.Value,
                    TotalBuy = x.Sum(y => y.Quantitys * y.OriginalPrice),
                    TotalSell = x.Sum(y => y.Quantitys * y.Prices)

                }).Select(x => new
                {
                    Dates = x.Date,
                    DoanhThu = x.TotalSell,
                    LoiNhuan = x.TotalSell - x.TotalBuy
                });
                return Json(new { Data = resulf}, JsonRequestBehavior.AllowGet);
                //return new JsonResult { Data = db.Employees, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
       

        }






    }
}