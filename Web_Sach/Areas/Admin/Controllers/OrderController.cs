using OfficeOpenXml.Style;
using OfficeOpenXml;
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
                             Image=sach.Images.FirstOrDefault(x=>x.MaSP==sach.ID && x.IsDefault).Image1,
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
                var orderDetail = db.ChiTietDonHangs.Where(x => x.MaDonHang == id);

                foreach (var item in orderDetail)
                {// cập nhật số lượng
                    var sachUpdate = db.Saches.Find(item.MaSach);// cập nhật lại số lượng sản phẩm
                    sachUpdate.Quantity = sachUpdate.Quantity + item.Quantity;
                    db.Entry(sachUpdate).Property(s => s.Quantity).IsModified = true;

                }
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

        public void ExportExcel_EPPLUS()
        {
            var list = db.DonHangs.ToList();
            int Out_TotalRecord = list.Count();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "ID";
            Sheet.Cells["A1"].Style.Font.Bold = true;
            Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["B1"].Value = "Mã khách hàng";
            Sheet.Cells["B1"].Style.Font.Bold = true;
            Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["C1"].Value = "Mã voucher";
            Sheet.Cells["C1"].Style.Font.Bold = true;
            Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["D1"].Value = "Tổng tiền";
            Sheet.Cells["D1"].Style.Font.Bold = true;
            Sheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["E1"].Value = "Đã thanh toán";
            Sheet.Cells["E1"].Style.Font.Bold = true;
            Sheet.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["F1"].Value = "Ngày đặt";
            Sheet.Cells["F1"].Style.Font.Bold = true;
            Sheet.Cells["F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["G1"].Value = "Trạng thái";
            Sheet.Cells["G1"].Style.Font.Bold = true;
            Sheet.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["H1"].Value = "Tên người nhận";
            Sheet.Cells["H1"].Style.Font.Bold = true;
            Sheet.Cells["H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["I1"].Value = "Địa chỉ người nhận";
            Sheet.Cells["I1"].Style.Font.Bold = true;
            Sheet.Cells["I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["J1"].Value = "Email người nhận";
            Sheet.Cells["J1"].Style.Font.Bold = true;
            Sheet.Cells["J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["K1"].Value = "SDT người nhận";
            Sheet.Cells["K1"].Style.Font.Bold = true;
            Sheet.Cells["K1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int row = 2;
            if (Out_TotalRecord > 0)
            {
                foreach (var item in list)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                    Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.MaKH;
                    Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.MaVoucher;
                    Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.TongTien;
                    Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.DaThanhToan;
                    Sheet.Cells[string.Format("E{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("F{0}", row)].Value = item.NgayDat;
                    Sheet.Cells[string.Format("F{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("G{0}", row)].Value = item.Status;
                    Sheet.Cells[string.Format("G{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("H{0}", row)].Value = item.TenNguoiNhan;
                    Sheet.Cells[string.Format("H{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("I{0}", row)].Value = item.DiaChiNguoiNhan;
                    Sheet.Cells[string.Format("I{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("J{0}", row)].Value = item.Email;
                    Sheet.Cells[string.Format("J{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("K{0}", row)].Value = item.Moblie;
                    Sheet.Cells[string.Format("K{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    row++;
                }
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();
        }


    }
}