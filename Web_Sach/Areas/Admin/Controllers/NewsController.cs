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
    public class NewsController : BaseController
    {
        // GET: Admin/News
        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var listPage = new newModel().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }

        //Thêm mới 
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreatePost(Tin_Tuc news)
        {
            if (ModelState.IsValid)
            {
                var imgUnique = new newModel().Unique(news.Name);
                if (imgUnique)
                {
                    ModelState.AddModelError("Name", "Tên đã tồn tại");
                    return View(news);
                }
                var addNew = new newModel().InserNew(news);
                if (addNew > 0)
                {// thêm silde thành công
                    SetAlert("Thêm tin tức thành công", "success");
                    return RedirectToAction("Index", "News");
                }
                else
                {
                    SetAlert("Thêm tin tức thất bại", "error");
                }

            }
            return View("Create");

        }


        [HttpGet]
        public ActionResult Update(int id)
        {
            var newEdit = new newModel().EditNew(id);
            return View(newEdit);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Tin_Tuc news)
        {
            if (ModelState.IsValid)
            {
                var newUp = new newModel();
                if (newUp.Compare(news))
                {
                    ModelState.AddModelError("Name", "Tên đã tồn tại");

                    return View(news);
                }
                if (newUp.UpdateNew(news))
                {
                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "News");
                }
            }

            SetAlert("Update thất bại", "error");
            return View(news);


        }
        // Xóa bản ghi
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var check = new newModel().Delete(id);
            if (check == true)
            {
                SetAlert("Xóa bản ghi thành công", "success");
                return RedirectToAction("Index", "News");
            }
            else
            {
                SetAlert("Xóa bản ghi thất bại", "error");
            }
            return RedirectToAction("Index", "News");

        }

        public void ExportExcel_EPPLUS()
        {
            using (WebSachDb db = new WebSachDb())
            {
                var list = db.Tin_Tuc.ToList();
                int Out_TotalRecord = list.Count();
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage Ep = new ExcelPackage();

                ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
                Sheet.Cells["A1"].Value = "ID";
                Sheet.Cells["A1"].Style.Font.Bold = true;
                Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["B1"].Value = "Tên tin tức";
                Sheet.Cells["B1"].Style.Font.Bold = true;
                Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["C1"].Value = "Metatitle";
                Sheet.Cells["C1"].Style.Font.Bold = true;
                Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["D1"].Value = "Image";
                Sheet.Cells["D1"].Style.Font.Bold = true;
                Sheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                Sheet.Cells["E1"].Value = "Metatitle";
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
                        Sheet.Cells[string.Format("D{0}", row)].Value = item.Image;
                        Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        Sheet.Cells[string.Format("E{0}", row)].Value = item.CreatedDate?.ToString("dd/MM/yyyy");
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