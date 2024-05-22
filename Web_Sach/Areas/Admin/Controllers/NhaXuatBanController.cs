using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class NhaXuatBanController : BaseController
    {
        private WebSachDb db = new WebSachDb();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var listPage = new NXB().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }

        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public ActionResult Create(NhaXuatBan nxb)
        {
            if (ModelState.IsValid)
            {
                var itemNXB = new NXB();
                if (itemNXB.Compare(nxb.TenNXB) != null)
                {// đã tồn tại tên  nxb
                    ModelState.AddModelError("TenNXB", "Tên nhà xuất bản đã tồn tại!");
                    return View(nxb);

                }
                // tên nxb chưa tồn tại

                var indexNXB = itemNXB.InsertNXB(nxb);
                if (indexNXB > 0)
                {// thêm user thành công
                    SetAlert("Thêm nhà xuất bản thành công", "success");
                    return RedirectToAction("Index", "NhaXuatBan");
                }
            }
            else
            {
                SetAlert("Thêm nhà xuất bản thất bại", "error");
            }

            return View(nxb);

        }

        // update
        public ActionResult Update(int id)
        {
            var nxb = db.NhaXuatBans.Find(id);
            return View(nxb);
        }

        [HttpPost]
        public ActionResult Update(NhaXuatBan nxb)
        {

            if (ModelState.IsValid)
            {
                var nxbUpdate = new NXB();


                if (nxbUpdate.Compare(nxb))
                {
                    ModelState.AddModelError("TenNXB", "Tên nhà xuất bản đã tồn tại");
                    return View(nxb);
                }


                if (nxbUpdate.Update(nxb))
                {
                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "NhaXuatBan");
                }


            }
            else
            {
                SetAlert("Update thất bại", "error");

            }
            return View(nxb);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
           new NXB().Delete(id);
            return RedirectToAction("Index", "NhaXuatBan");
        }

        public void ExportExcel_EPPLUS()
        {
            var list = db.NhaXuatBans.ToList();
            int Out_TotalRecord = list.Count();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "ID";
            Sheet.Cells["A1"].Style.Font.Bold = true;
            Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["B1"].Value = "Tên nhà xuất bản";
            Sheet.Cells["B1"].Style.Font.Bold = true;
            Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["C1"].Value = "SDT";
            Sheet.Cells["C1"].Style.Font.Bold = true;
            Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["D1"].Value = "Địa chỉ";
            Sheet.Cells["D1"].Style.Font.Bold = true;
            Sheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["E1"].Value = "Email";
            Sheet.Cells["E1"].Style.Font.Bold = true;
            Sheet.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int row = 2;
            if (Out_TotalRecord > 0)
            {
                foreach (var item in list)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                    Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.TenNXB;
                    Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.SDT;
                    Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.DiaChi;
                    Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.Email;
                    Sheet.Cells[string.Format("E{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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