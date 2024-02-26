using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using static System.Data.Entity.Infrastructure.Design.Executor;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class ThongKeController : BaseController
    {
        // GET: Admin/ThongKe
        private WebSachDb db = new WebSachDb();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetStatistical(string fromDate, string toDate)
        {

            var query = from o in db.DonHangs
                        join od in db.ChiTietDonHangs on o.ID equals od.MaDonHang
                        join p in db.Saches on od.MaSach equals p.ID
                        where o.Status == 5
                        select new
                        {
                            CreatedDate = o.NgayDat,
                            Quantitys = od.Quantity,
                            Prices = od.Price,
                            OriginalPrice = p.GiaNhap

                        };

            if (!string.IsNullOrEmpty(fromDate))
            {
                if (DateTime.TryParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startDate))
                {
                    //DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                query = query.Where(x => x.CreatedDate >= startDate);
                }
               
            }

            if (!string.IsNullOrEmpty(toDate))
            {
                if (DateTime.TryParseExact(toDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime endDate))
                {
                     //DateTime endDate = DateTime.ParseExact(toDate, "DD/MM/yyyy", null);
                     query = query.Where(x => x.CreatedDate < endDate);
                }
              
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
            return Json(new { Data = resulf }, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult ThongKeTheoThang()
        {
            return View();
        }


        public ActionResult GetThongKeTheoThang(string fromDate, string toDate)
        {
            var query = from o in db.DonHangs
                        join od in db.ChiTietDonHangs on o.ID equals od.MaDonHang
                        join p in db.Saches on od.MaSach equals p.ID
                        where o.Status == 5
                        select new
                        {
                            CreatedDate = o.NgayDat,
                            Quantitys = od.Quantity,
                            Prices = od.Price,
                            OriginalPrice = p.GiaNhap

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

            var resulf = query.Where(x => x.CreatedDate.HasValue) // Chỉ lấy các dòng có CreatedDate có giá trị
                                .GroupBy(x => new { Months = x.CreatedDate.Value.Month }) //  tháng
                                .Select(x => new
                                {
                                    Date = x.Key.Months, //tháng
                                    TotalBuy = x.Sum(y => y.Quantitys * y.OriginalPrice),
                                    TotalSell = x.Sum(y => y.Quantitys * y.Prices)
                                })
                                .Select(x => new
                                {
                                    Dates = x.Date,
                                    DoanhThu = x.TotalSell,
                                    LoiNhuan = x.TotalSell - x.TotalBuy
                                });

            return Json(new { Data = resulf }, JsonRequestBehavior.AllowGet);





        }


        [HttpGet]
        public ActionResult ThongKeTheoNam()
        {
            return View();
        }


        public ActionResult GetThongKeTheoNam(string fromDate, string toDate)
        {
            var query = from o in db.DonHangs
                        join od in db.ChiTietDonHangs on o.ID equals od.MaDonHang
                        join p in db.Saches on od.MaSach equals p.ID
                        where o.Status == 5
                        select new
                        {
                            CreatedDate = o.NgayDat,
                            Quantitys = od.Quantity,
                            Prices = od.Price,
                            OriginalPrice = p.GiaNhap

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

            var resulf = query.Where(x => x.CreatedDate.HasValue) // Chỉ lấy các dòng có CreatedDate có giá trị
                                .GroupBy(x => new { Years = x.CreatedDate.Value.Year }) //  tháng
                                .Select(x => new
                                {
                                    Date = x.Key.Years, // Tạo ngày từ năm 
                                    TotalBuy = x.Sum(y => y.Quantitys * y.OriginalPrice),
                                    TotalSell = x.Sum(y => y.Quantitys * y.Prices)
                                })
                                .Select(x => new
                                {
                                    Dates = x.Date,
                                    DoanhThu = x.TotalSell,
                                    LoiNhuan = x.TotalSell - x.TotalBuy
                                });

            return Json(new { Data = resulf }, JsonRequestBehavior.AllowGet);





        }




    }
}