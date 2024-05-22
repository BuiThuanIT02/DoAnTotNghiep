using OfficeOpenXml.Style;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private WebSachDb db = new WebSachDb();
        // GET: Admin/Product
        public ActionResult Index(string search, int page = 1, int pageSize = 20)
        {
            var product = new Book().listPage(search, page, pageSize);
            ViewBag.Search = search;
            return View(product);
        }
        [HttpGet]
        //Form tạo Sách
        public ActionResult Create()
        {

            ViewBag.Author = callAuthor();
            setViewBagCategory();
            setViewBagNXB();

            return View();
        }
       
        [HttpPost]
        [ValidateInput(false)]  // tắt tính năng kiểm tra đầu vào
        // Thêm sản phẩm
        public ActionResult Create(Sach sach, List<string> Images, List<int> rDefault, List<int> Authors)
        {
            ModelState.Remove("Images"); // xóa bỏ thuộc tính Image từ model cũ >> mới check đc 

            if (ModelState.IsValid)
            {
                //check ảnh input
                if (Images != null && Images.Count > 0)
                {
                    for (int i = 0; i < Images.Count; i++)
                    {
                        if (i + 1 == rDefault[0])
                        {// kiểm tra xem hình ảnh nào được đặt Default
                            sach.Images.Add(new Image
                            {
                                ID = sach.ID,
                                Image1 = Images[i],
                                IsDefault = true
                            });
                        }// thêm ảnh vào bảng Image thông qua đối tượng Sach
                        else
                        {
                            // kiểm tra xem hình ảnh nào được không phải Default
                            sach.Images.Add(new Image
                            {
                                ID = sach.ID,
                                Image1 = Images[i],
                                IsDefault = false
                            });

                        }
                    }
                }
                else
                {// sp k có ảnh
                    SetAlert("Thiếu hình ảnh cho sản phẩm", "error");
                    ViewBag.Author = callAuthor();
                    setViewBagCategory();
                    setViewBagNXB();
                    return View(sach);

                }
                // kiểm tra tác giả
                if (Authors == null)
                {
                    SetAlert("Chưa chọn tác giả", "error");
                    ViewBag.Author = callAuthor();
                    setViewBagCategory();
                    setViewBagNXB();
                    return View(sach);
                }
                // có thể thiế SaveChange
                // kiểm tra tên sách có trùng không
                var book = new Book();
                if (book.Compare(sach))
                {// nếu trùng thông báo
                    ModelState.AddModelError("Name", "Tên sách đã tồn tại");
                    ViewBag.Author = callAuthor();
                    setViewBagCategory();
                    setViewBagNXB();
                    return View(sach);
                }
                if (sach.Price < 0)
                {
                    ModelState.AddModelError("Price", "Giá bán phải lớn hơn 0");
                    ViewBag.Author = callAuthor();
                    setViewBagCategory();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.GiaNhap < 0)
                {
                    ModelState.AddModelError("GiaNhap", "Giá nhập phải lớn hơn 0");
                    setViewBagCategory();
                    ViewBag.Author = callAuthor();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.GiaNhap > sach.Price)
                {
                    ModelState.AddModelError("GiaNhap", "Giá nhập phải nhỏ hơn hoặc bằng giá bán");
                    setViewBagCategory();
                    ViewBag.Author = callAuthor();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.Quantity < 0)
                {
                    ModelState.AddModelError("Quantity", "Số lượng sách phải lớn hơn 0");
                    setViewBagCategory();
                    ViewBag.Author = callAuthor();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.SoTrang < 0)
                {
                    ModelState.AddModelError("SoTrang", "Số trang phải lớn hơn 0");
                    setViewBagCategory();
                    ViewBag.Author = callAuthor();
                    setViewBagNXB();
                    return View(sach);
                }
                var idNew = book.Insert(sach);
                if (idNew > 0)
                {// chèn thành công
                    foreach (var item in Authors)
                    {
                        var thamGia = new ThamGia()
                        {
                            MaTacGia = item,
                            MaSach = idNew,
                        };
                        db.ThamGias.Add(thamGia);
                    }
                    db.SaveChanges();
                    SetAlert("Thêm bản ghi thành công", "success");
                    return RedirectToAction("Index", "Product");

                }
                else
                {// chèn thất bại
                    SetAlert("Thêm bản ghi thất bại", "error");

                    //return RedirectToAction("Create", "Product");
                }

            }
            // kiểm tra thông tin đầu vào thất bại
            setViewBagCategory();
            ViewBag.Author = callAuthor();
            setViewBagNXB();
            return View("Create");
        }


        [HttpGet]

        // sửa sản phẩm
        public ActionResult Update(int id)
        {
            var bookEdit = new Book().Edit(id);// tim sach theo ID
            var bookImages = bookEdit.Images.ToList();
            ViewBag.bookImages = bookImages;
            //AsEnumerable ép kiểu ThamGia về IEnumberable
            var tacgia = bookEdit.ThamGias.AsEnumerable().Select(x => id = x.MaTacGia);
            ViewBag.AuthorEdit =tacgia.ToList() ;
            ViewBag.Author = callAuthor();
            setViewBagCategory(bookEdit.DanhMucID);
            setViewBagNXB(bookEdit.NhaXuatBanID);
            return View(bookEdit);

        }

        //end sửa sản phẩm

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Sach sach, List<string> Images, List<int> rDefault, List<int> Authors)
        {
            ModelState.Remove("Images");

            if (ModelState.IsValid)
            {
                var idSach = sach.ID;

                if (Images != null && Images.Count > 0)
                {
                    var imgSql = db.Images.Where(a => a.MaSP == idSach).ToList();// danh sách ảnh của 1 SP

                    var countImageSql = imgSql.Count; // số lượng ảnh có trong CSDL
                    var countImageCurrent = Images.Count; // số lượng ảnh hiện tại
                    var countSurPlus = (countImageCurrent - countImageSql);// số dư khi lớn hơn
                    if (countImageCurrent >= countImageSql)
                    {// ảnh trong CSDL nhở hơn ảnh đầu vào >> nên thêm ảnh vào CSDL
                        var arrCount = countImageSql + countSurPlus;
                        for (int i = 0; i < arrCount; i++)
                        {
                            if (i < countImageSql)
                            {// thay thế cho hết image trong CSDL
                                if (i + 1 == rDefault[0])
                                {
                                    imgSql[i].Image1 = Images[i];
                                    imgSql[i].IsDefault = true;
                                }
                                else
                                {
                                    imgSql[i].Image1 = Images[i];
                                    imgSql[i].IsDefault = false;
                                }
                            }

                            else
                            {// hết index trong mảng ảnh CSDL nên ===> add ảnh vào
                                var imgNew = new Image();

                                if (i + 1 == rDefault[0])
                                {
                                    imgNew.MaSP = idSach;
                                    imgNew.Image1 = Images[i];
                                    imgNew.IsDefault = true;
                                }
                                else
                                {
                                    imgNew.MaSP = idSach;
                                    imgNew.Image1 = Images[i];
                                    imgNew.IsDefault = false;
                                }

                                db.Images.Add(imgNew);

                            }
                        }


                        db.SaveChanges();

                    }
                    else
                    {// ảnh trong CSDL lớn hơn ảnh đc đầu vào  (muốn xóa ảnh)

                        // Sử dụng vòng lặp ngược để tránh vấn đề với việc xóa phần tử trong quá trình duyệt
                        for (int i = imgSql.Count - 1; i >= 0; i--)
                        {
                            // Kiểm tra nếu vẫn còn phần tử trong danh sách Images và còn phần tử trong danh sách từ cơ sở dữ liệu
                            if (i < Images.Count && i < imgSql.Count)
                            {
                                // Kiểm tra nếu đây là vị trí hình ảnh mặc định (tính từ danh sách rDefault)
                                if (i + 1 == rDefault[0])
                                {
                                    imgSql[i].Image1 = Images[i];
                                    imgSql[i].IsDefault = true;
                                }
                                else
                                {
                                    imgSql[i].Image1 = Images[i];
                                    imgSql[i].IsDefault = false;
                                }
                            }
                            // Nếu không còn phần tử trong danh sách Images hoặc danh sách từ cơ sở dữ liệu, xóa đối tượng từ cơ sở dữ liệu
                            else
                            {
                                // Kiểm tra xem index i có hợp lệ không trước khi xóa
                                if (i >= 0 && i < imgSql.Count)
                                {
                                    // Xóa đối tượng tại vị trí i
                                    db.Images.Remove(imgSql[i]);
                                }
                            }
                        }

                        db.SaveChanges();



                    }

                }
                else
                {// sp k có ảnh
                    SetAlert("Thiếu hình ảnh cho sản phẩm", "error");
                    ViewBag.Author = callAuthor();
                    setViewBagCategory();
                    setViewBagNXB();
                    return View(sach);

                }
                // kiểm tra tác giả
                if (Authors == null || Authors.Count < 0)
                {
                    SetAlert("Chưa chọn tác giả", "error");
                    ViewBag.Author = callAuthor();
                    setViewBagCategory();
                    setViewBagNXB();
                    return View(sach);
                }
                // kiểm tra trùng tên
                var bookUpdate = new Book();
                if (bookUpdate.CompareUpdate(sach))
                {// trùng tên sách
                    ModelState.AddModelError("Name", "Tên sách bị trùng");
                    //mới thêm ngày 15-10-2023
                    setViewBagCategory(sach.DanhMucID);
                    ViewBag.Author = callAuthor();
                    setViewBagNXB(sach.NhaXuatBanID);
                    return View(sach);

                }
                if (sach.Price < 0)
                {
                    ModelState.AddModelError("Price", "Giá sách phải lớn hơn 0");
                    setViewBagCategory(sach.DanhMucID);
                    ViewBag.Author = callAuthor();
                    setViewBagNXB(sach.NhaXuatBanID);
                    return View(sach);
                }
                else if (sach.GiaNhap < 0)
                {
                    ModelState.AddModelError("GiaNhap", "Giá nhập phải lớn hơn 0");
                    setViewBagCategory(sach.DanhMucID);
                    ViewBag.Author = callAuthor();
                    setViewBagNXB(sach.NhaXuatBanID);
                    return View(sach);
                }
                else if (sach.GiaNhap > sach.Price)
                {
                    ModelState.AddModelError("GiaNhap", "Giá nhập phải nhỏ hơn hoặc bằng giá bán");
                    setViewBagCategory(sach.DanhMucID);
                    ViewBag.Author = callAuthor();
                    setViewBagNXB(sach.NhaXuatBanID);
                    return View(sach);
                }
                else if (sach.Quantity < 0)
                {
                    ModelState.AddModelError("Quantity", "Số lượng sách phải lớn hơn 0");
                    setViewBagCategory(sach.DanhMucID);
                    ViewBag.Author = callAuthor();
                    setViewBagNXB(sach.NhaXuatBanID);
                    return View(sach);
                }
                else if (sach.SoTrang < 0)
                {
                    ModelState.AddModelError("SoTrang", "Số trang phải lớn hơn 0");
                    setViewBagCategory(sach.DanhMucID);
                    ViewBag.Author = callAuthor();
                    setViewBagNXB(sach.NhaXuatBanID);
                    return View(sach);
                }
                if (bookUpdate.Update(sach))
                {// update thành công
                    var tacgia = db.ThamGias.Where(x=>x.MaSach == idSach).ToList();
                    db.ThamGias.RemoveRange(tacgia);
                    foreach (var item in Authors)
                    {
                        var thamGia = new ThamGia()
                        {
                            MaTacGia = item,
                            MaSach = idSach,
                        };
                        db.ThamGias.Add(thamGia);
                    }
                    db.SaveChanges();
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "Product");
                }
            }
            SetAlert("Cập nhật thất bại", "error");
            setViewBagCategory(sach.DanhMucID);
            ViewBag.Author = callAuthor();
            setViewBagNXB(sach.NhaXuatBanID);
            return View("Update");
        }

        [HttpDelete]
        // xóa bản ghi
        public ActionResult Delete(int id)
        {
            new Book().Delete(id);
           
            return RedirectToAction("Index", "Product");
        }


        // thay đổi trạng thái
        [HttpPost]
        public JsonResult ChangeStatus(int id)
        {
            var resulf = new Book().ChangeStatus(id);
            return Json(new
            {
                status = resulf
            });
        }


        public void setViewBagCategory(int? selectedId = null)
        {
            var drowdoad = new ProductCategory();
            ViewBag.DanhMucID = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);// hiện thị droplisst theo Name
        }


        public void setViewBagNXB(int? selectedId = null)
        {
            var drowdoad = new NXB();
            ViewBag.NhaXuatBanID = new SelectList(drowdoad.ListAll(), "ID", "TenNXB", selectedId);
        }
        public List<TacGia> callAuthor()
        {
            return db.TacGias.ToList();

        }

        public void ExportExcel_EPPLUS()
        {
            var list = db.Saches.ToList();
            int Out_TotalRecord = list.Count();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "ID";
            Sheet.Cells["A1"].Style.Font.Bold = true;
            Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["B1"].Value = "Mã danh mục";
            Sheet.Cells["B1"].Style.Font.Bold = true;
            Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["C1"].Value = "Mã nhà xuất bản";
            Sheet.Cells["C1"].Style.Font.Bold = true;
            Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["D1"].Value = "Tên sách";
            Sheet.Cells["D1"].Style.Font.Bold = true;
            Sheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["E1"].Value = "Giá bán";
            Sheet.Cells["E1"].Style.Font.Bold = true;
            Sheet.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["F1"].Value = "Giá nhập kho";
            Sheet.Cells["F1"].Style.Font.Bold = true;
            Sheet.Cells["F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["G1"].Value = "Kích thước";
            Sheet.Cells["G1"].Style.Font.Bold = true;
            Sheet.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["H1"].Value = "Trọng lượng";
            Sheet.Cells["H1"].Style.Font.Bold = true;
            Sheet.Cells["H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["I1"].Value = "Số trang";
            Sheet.Cells["I1"].Style.Font.Bold = true;
            Sheet.Cells["I1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["J1"].Value = "Ngày cập nhật";
            Sheet.Cells["J1"].Style.Font.Bold = true;
            Sheet.Cells["J1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["K1"].Value = "Metatitle";
            Sheet.Cells["K1"].Style.Font.Bold = true;
            Sheet.Cells["K1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["L1"].Value = "Trạng thái";
            Sheet.Cells["L1"].Style.Font.Bold = true;
            Sheet.Cells["L1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["M1"].Value = "Số lượng";
            Sheet.Cells["M1"].Style.Font.Bold = true;
            Sheet.Cells["M1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            int row = 2;
            if (Out_TotalRecord > 0)
            {
                foreach (var item in list)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                    Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.DanhMucID;
                    Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.NhaXuatBanID;
                    Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("D{0}", row)].Value = item.Name;
                    Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("E{0}", row)].Value = item.Price;
                    Sheet.Cells[string.Format("E{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("F{0}", row)].Value = item.GiaNhap;
                    Sheet.Cells[string.Format("F{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("G{0}", row)].Value = item.KichThuoc;
                    Sheet.Cells[string.Format("G{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("H{0}", row)].Value = item.TrongLuong;
                    Sheet.Cells[string.Format("H{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("I{0}", row)].Value = item.SoTrang;
                    Sheet.Cells[string.Format("I{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("J{0}", row)].Value = item.NgayCapNhat?.ToString("dd/MM/yyyy");
                    Sheet.Cells[string.Format("J{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("K{0}", row)].Value = item.MetaTitle;
                    Sheet.Cells[string.Format("K{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("L{0}", row)].Value = item.Status;
                    Sheet.Cells[string.Format("L{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("M{0}", row)].Value = item.Quantity;
                    Sheet.Cells[string.Format("M{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
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