using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class ProductCategoryController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var product = new ProductCategory().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(product);
        }

        //thêm mới danh mục sản phẩm

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(DanhMucSP dm)
        {
            if (ModelState.IsValid)
            {
                var productCategory = new ProductCategory().uniqueName(dm.Name);
                if (productCategory != null)
                {
                    ModelState.AddModelError("Name", "Tên danh mục đã tồn tại");
                    return View(dm);
                }
                dm.CreatedDate = DateTime.Now;
                var productCategoryID = new ProductCategory().Insert(dm);
                if (productCategoryID > 0)
                {
                    SetAlert("Thêm danh mục thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                SetAlert("Thêm danh mục thất bại", "error");
            }

            return View("Create");
        }


        //End thêm mới danh mục sản phẩm

        // sửa bản ghi danh mục

        [HttpGet]
        public ActionResult Update(int id)
        {
            var productCategory = new ProductCategory().Edit(id);
            return View(productCategory);
        }

        [HttpPost]
        public ActionResult Update(DanhMucSP dm)
        {
            if (ModelState.IsValid)
            {
                var category = new ProductCategory();

                if (category.Compare(dm))
                {
                    ModelState.AddModelError("Name", "Tên danh mục đã tồn tại");
                    return View(dm);
                }
                dm.CreatedDate = DateTime.Now;
                if (category.Update(dm))
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "ProductCategory");

                }
                else
                {
                    SetAlert("Cập nhật thất bại", "error");
                    return RedirectToAction("Index", "ProductCategory");
                }



            }
            return View("Update");
        }



        [HttpDelete]

        public ActionResult Delete(int id)
        {
            new ProductCategory().Delete(id);

            return RedirectToAction("Index", "ProductCategory");
        }




        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var resulf = new ProductCategory().Change(id);
            return Json(new
            {
                status = resulf
            });
        }

        public void ExportExcel_EPPLUS()
        {
            using (WebSachDb db = new WebSachDb())
            {
                var list = db.DanhMucSPs.ToList();
                int Out_TotalRecord = list.Count();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage Ep = new ExcelPackage();

                ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
                Sheet.Cells["A1"].Value = "ID";
                Sheet.Cells["A1"].Style.Font.Bold = true;
                Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["B1"].Value = "Tên danh mục";
                Sheet.Cells["B1"].Style.Font.Bold = true;
                Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["C1"].Value = "Metatitle";
                Sheet.Cells["C1"].Style.Font.Bold = true;
                Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["D1"].Value = "Ngày tạo";
                Sheet.Cells["D1"].Style.Font.Bold = true;
                Sheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["E1"].Value = "Trạng thái";
                Sheet.Cells["E1"].Style.Font.Bold = true;
                Sheet.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                int row = 2;
                if (Out_TotalRecord > 0)
                {
                    foreach (var item in list)
                    {
                        Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                        Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        Sheet.Cells[string.Format("B{0}", row)].Value = item.Name;
                        Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        Sheet.Cells[string.Format("C{0}", row)].Value = item.MetaTitle;
                        Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        Sheet.Cells[string.Format("D{0}", row)].Value = item.CreatedDate?.ToString("dd/MM/yyyy");
                        Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        Sheet.Cells[string.Format("E{0}", row)].Value = item.Status;
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
}