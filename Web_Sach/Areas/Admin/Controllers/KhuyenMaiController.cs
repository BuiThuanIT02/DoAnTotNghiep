using Newtonsoft.Json;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web_Sach.Areas.Admin.Data;
using Web_Sach.Models;

using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class KhuyenMaiController : BaseController
    {
        // GET: Admin/ProductCategory
        private WebSachDb db = new WebSachDb();
        public ActionResult Index(string searchString, int page = 1, int pageSize = 100)
        {
            var km = new kmModels().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(km);
        }
        public class adm_Sale
        {
            public int Id { get; set; }
            public string Name { get; set; }
            // public int Price { get; set;}   
        }
        public class ProductData
        {
            public int CheckBoxId { get; set; }
            public List<adm_Sale> SelectedOptions { get; set; }
        }
        public class createSale
        {
            public string tenKM { get; set; }
            public DateTime? ngayBD { get; set; }
            public DateTime? ngayKT { get; set; }
        }
        public class updateSale
        {
            public int maKM { get; set; }
            public string tenKM { get; set; }
            public DateTime? ngayBD { get; set; }
            public DateTime? ngayKT { get; set; }
        }
        public class InputValues
        {
            public int ProductId { get; set; } // ID của sản phẩm
            public int PriceSale { get; set; } // Giá sale của sản phẩm

        }

        [HttpGet]
        public ActionResult GetProducts(string arrId)
        {
            // Chuyển đổi chuỗi JSON thành mảng các ID checkbox
            var checkboxIds = JsonConvert.DeserializeObject<List<int>>(arrId);

            // Tạo danh sách dữ liệu cho các select tương ứng
            var productDataList = new List<ProductData>();
            foreach (var checkboxId in checkboxIds)
            {
                var selectedOptions = new List<adm_Sale>();
                var sach = db.Saches.Where(x => x.DanhMucID == checkboxId).Select(a => new adm_Sale
                {
                    Id = a.ID,
                    Name = a.Name,
                }).ToList();
                selectedOptions.AddRange(sach);

                // Xử lý logic để lấy dữ liệu cho select tương ứng với checkboxId

                productDataList.Add(new ProductData { CheckBoxId = checkboxId, SelectedOptions = selectedOptions });
            }

            // Trả về dữ liệu JSON
            return Json(productDataList.ToList(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Create()
        {
            // setViewBagMaSach();
            //ViewBag.Saches = saches();
            // setViewBagCategory();
            ViewBag.Dm = ListDM();
            return View();
        }

        [HttpPost]
        public ActionResult CreatePost(string sale, string inputValues)
        {

            try
            {
                var obSale = new JavaScriptSerializer().Deserialize<createSale>(sale);
                var kmCheck = new kmModels().uniqueName(obSale.tenKM);
                if (kmCheck != null)
                {
                    return Json(new { status = false, error = 204 });
                }
                List<Dictionary<string, string>> dsInputSale = new JavaScriptSerializer().Deserialize<List<Dictionary<string, string>>>(inputValues);
                // Create new KhuyenMai
                var kmNew = new KhuyenMai()
                {
                    TenKhuyenMai = obSale.tenKM,
                    NgayBatDau = obSale.ngayBD,
                    NgayKeThuc = obSale.ngayKT,
                    KhuyenMai_Sach = new List<KhuyenMai_Sach>() // Initialize list
                };

                // Add new KhuyenMai to database
                db.KhuyenMais.Add(kmNew);
                dsInputSale.RemoveAll(item => item == null);
                // Iterate through dsInputSale
                foreach (var dict in dsInputSale)
                {
                    foreach (var kvp in dict)
                    {
                        // Add new KhuyenMai_Sach to KhuyenMai
                        kmNew.KhuyenMai_Sach.Add(new KhuyenMai_Sach
                        {
                            MaSach = int.Parse(kvp.Key),
                            Sale = int.Parse(kvp.Value)
                        });

                    }
                }
                db.SaveChanges();
                TempData["AlertContent"] = "Thêm mới thành công!";
                TempData["AlertMessage"] = "alert-success";

                return Json(new { status = true });
            }
            catch (Exception)
            {
                TempData["AlertContent"] = "Thêm mới thất bại!";
                TempData["AlertMessage"] = "alert-danger";
                return Json(new { status = false, error = 500 });
            }

        }


        [HttpGet]
        public ActionResult Update(int maDM, int maKM)
        {
            var kmItem = new kmModels().Edit(maDM, maKM);
            var sachDM = db.Saches.Where(x => x.DanhMucID == maDM).Select(x => x.ID).ToList();
            var resultf = db.KhuyenMai_Sach.Where(x => sachDM.Contains(x.MaSach) && x.MaKhuyenMai == maKM).ToList();
            var cateName = db.DanhMucSPs.Find(maDM).Name;
            ViewBag.CateName = cateName;
            ViewBag.CateId = maDM;
            ViewBag.MaKM = maKM;
            ViewBag.listSale = resultf;
            TempData["maDM"] = maDM;
            setViewBagCategory(maDM);
            return View(kmItem);
        }

        [HttpPost]
        public ActionResult UpdatePost(string sale, string inputValues)
        {
            try
            {
                var obSale = new JavaScriptSerializer().Deserialize<updateSale>(sale);
                var kmModel = new kmModels();
                if (kmModel.Compare(obSale.maKM, obSale.tenKM))
                {
                    return Json(new { status = false, error = 204 });
                }
                List<Dictionary<string, string>> dsInputSale = new JavaScriptSerializer().Deserialize<List<Dictionary<string, string>>>(inputValues);
                if (dsInputSale.Count > 0)
                {
                    dsInputSale.RemoveAll(item => item == null);
                    // return Json(new { status = true });
                }
                var khuyenMai = db.KhuyenMais.Find(obSale.maKM);
                if (khuyenMai != null)
                {
                    khuyenMai.TenKhuyenMai = obSale.tenKM;
                    khuyenMai.NgayBatDau = obSale.ngayBD;
                    khuyenMai.NgayKeThuc = obSale.ngayKT;
                    var kmUpdate = db.KhuyenMai_Sach.Where(x => x.MaKhuyenMai == obSale.maKM);
                    if (kmUpdate != null)
                    {
                        foreach (var dict in dsInputSale)
                        {
                            foreach (var kvp in dict)
                            {
                                int maSach = int.Parse(kvp.Key);
                                int saleUpdate = int.Parse(kvp.Value);
                                // Tìm KhuyenMai_Sach cần cập nhật
                                var existingKhuyenMaiSach = kmUpdate.FirstOrDefault(kms => kms.MaSach == maSach);

                                if (existingKhuyenMaiSach != null)
                                {
                                    // Nếu KhuyenMai_Sach đã tồn tại, cập nhật giá trị
                                    existingKhuyenMaiSach.Sale = saleUpdate;
                                }

                            }
                        }


                    }
                    db.SaveChanges();
                    TempData["AlertContent"] = "Cập nhật thành công!";
                    TempData["AlertMessage"] = "alert-success";
                    return Json(new { status = true });

                }
                TempData["AlertContent"] = "Thêm mới thất bại!";
                TempData["AlertMessage"] = "alert-danger";
                return Json(new { status = false });
            }
            catch (Exception)
            {
                TempData["AlertContent"] = "Thêm mới thất bại!";
                TempData["AlertMessage"] = "alert-danger";
                return Json(new { status = false, error = 500 });
            }
        }

        [HttpDelete]
        public ActionResult Delete(int maKM)
        {
            new kmModels().Delete(maKM);
            return RedirectToAction("Index", "KhuyenMai");
        }


        public void setViewBagMaSach(int? selectedId = null)
        {
            var drowdoad = new Book();
            ViewBag.MaSach = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);// hiện thị droplisst theo Name
        }
        public void setViewBagMaKM(int? selectedId = null)
        {
            var drowdoad = new kmModels();
            ViewBag.MaKhuyenMai = new SelectList(drowdoad.ListAll(), "ID", "TenKhuyenMai", selectedId);// hiện thị droplisst theo Name
        }
        public void setViewBagCategory(int? selectedId = null)
        {
            var drowdoad = new ProductCategory();
            ViewBag.MaDM = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);// hiện thị droplisst theo Name
        }
        public List<DanhMucSP> ListDM()
        {
            return db.DanhMucSPs.ToList();
        }

        public void ExportExcel_EPPLUS()
        {
            var list = db.KhuyenMais.ToList();
            int Out_TotalRecord = list.Count();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "ID";
            Sheet.Cells["A1"].Style.Font.Bold = true;
            Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["B1"].Value = "Tên khuyến mại";
            Sheet.Cells["B1"].Style.Font.Bold = true;
            Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["C1"].Value = "Ngày bắt đầu";
            Sheet.Cells["C1"].Style.Font.Bold = true;
            Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["D1"].Value = "Ngày kết thúc";
            Sheet.Cells["D1"].Style.Font.Bold = true;
            Sheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int row = 2;
            if (Out_TotalRecord > 0)
            {
                foreach (var item in list)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                    Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.TenKhuyenMai;
                    Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.NgayBatDau?.ToString("dd/MM/yyyy");
                    Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.NgayKeThuc?.ToString("dd/MM/yyyy");
                    Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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