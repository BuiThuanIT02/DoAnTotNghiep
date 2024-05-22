using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Areas.Admin.Data;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class AuthorController : BaseController
    {
        // GET: Admin/Author
        WebSachDb db = new WebSachDb();
        public ActionResult Index(string searchString, int page = 1, int pageSize = 100)
        {
            var listPage = new TacGiaModels().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TacGia inputTacGia)
        {
            if (ModelState.IsValid)
            {
                var tacGia = new TacGiaModels();
                
                if (tacGia.Compare(inputTacGia.TenTacGia) != null)
                {// đã tồn tại tên  tác giả
                    ModelState.AddModelError("TenTacGia", "Tên tác giả đã tồn tại!");                  
                    return View(inputTacGia);
                }
                // tên tác giả chưa tồn tại

               var index = tacGia.InsertAuthor(inputTacGia);
                if (index > 0)
                {
                    SetAlert("Thêm tác giả thành công", "success");
                    return RedirectToAction("Index", "Author");
                }


            }
            else
            {
                
                SetAlert("Thêm tác giả thất bại", "error");
            }

            return View(inputTacGia);

        }

        // update
        public ActionResult Update(int id)
        {
            var author = new TacGiaModels().Edit(id);            
            return View(author);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(TacGia inputTacGia)
        {
            if (ModelState.IsValid)
            {
                var authorUpdate = new TacGiaModels();             
                if (authorUpdate.Compare(inputTacGia))
                {
                    ModelState.AddModelError("TenTacGia", "Tên tác giả đã tồn tại");                
                    return View(inputTacGia);
                }             
                if (authorUpdate.Update(inputTacGia))
                {                   
                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "Author");
                }
            }
            else
            {
              
                SetAlert("Update thất bại", "error");
                return View(inputTacGia);
            }
            return View("Update");
        }

        [HttpDelete]
        public ActionResult Delete( int id)
        {
            new TacGiaModels().Delete(id);
            
            return RedirectToAction("Index", "Author");
        }

        public void ExportExcel_EPPLUS()
        {
            var list = db.TacGias.ToList();
            int Out_TotalRecord = list.Count();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "ID";
            Sheet.Cells["A1"].Style.Font.Bold = true;
            Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["B1"].Value = "Tên tác giả";
            Sheet.Cells["B1"].Style.Font.Bold = true;
            Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["C1"].Value = "Tiểu sử";
            Sheet.Cells["C1"].Style.Font.Bold = true;
            Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int row = 2;
            if (Out_TotalRecord > 0)
            {
                foreach (var item in list)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                    Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.TenTacGia;
                    Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.TieuSu;
                    Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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