using System;
using System.Collections.Generic;
using System.Linq;
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
            setViewBagCategory();
            setViewBagNCC();
            setViewBagNXB();

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]  // tắt tính năng kiểm tra đầu vào
        // Thêm sản phẩm
        public ActionResult Create(Sach sach, List<string> Images, List<int> rDefault)
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
                // có thể thiế SaveChange
                // kiểm tra tên sách có trùng không
                var book = new Book();
                if (book.Compare(sach))
                {// nếu trùng thông báo
                    ModelState.AddModelError("Name", "Tên sách đã tồn tại");
                    setViewBagCategory();
                    setViewBagNCC();
                    setViewBagNXB();
                    return View(sach);
                }
                if (sach.Price < 0)
                {
                    ModelState.AddModelError("Price", "Giá sách phải lớn hơn 0");
                    setViewBagCategory();
                    setViewBagNCC();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.Quantity < 0)
                {
                    ModelState.AddModelError("Quantity", "Số lượng sách phải lớn hơn 0");
                    setViewBagCategory();
                    setViewBagNCC();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.SoTrang < 0)
                {
                    ModelState.AddModelError("SoTrang", "Số trang phải lớn hơn 0");
                    setViewBagCategory();
                    setViewBagNCC();
                    setViewBagNXB();
                    return View(sach);
                }

                if (book.Insert(sach) > 0)
                {// chèn thành công

                    SetAlert("Thêm bản ghi thành công", "success");
                    return RedirectToAction("Index", "Product");

                }
                else
                {// chèn thất bại
                    SetAlert("Thêm bản ghi thất bại", "error");

                    return RedirectToAction("Create", "Product");
                }

            }
            // kiểm tra thông tin đầu vào thất bại
            setViewBagCategory();
            setViewBagNCC();
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
            setViewBagCategory(bookEdit.DanhMucID);
            setViewBagNCC(bookEdit.NhaCungCapID);
            setViewBagNXB(bookEdit.NhaXuatBanID);


            return View(bookEdit);

        }

        //end sửa sản phẩm

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Update(Sach sach, List<string> Images, List<int> rDefault)
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

                // kiểm tra trùng tên
                var bookUpdate = new Book();
                if (bookUpdate.CompareUpdate(sach))
                {// trùng tên sách
                    ModelState.AddModelError("Name", "Tên sách bị trùng");
                    //mới thêm ngày 15-10-2023
                    setViewBagCategory(sach.DanhMucID);
                    setViewBagNCC(sach.NhaCungCapID);
                    setViewBagNXB(sach.NhaXuatBanID);
                    return View(sach);

                }
                if (sach.Price < 0)
                {
                    ModelState.AddModelError("Price", "Giá sách phải lớn hơn 0");
                    setViewBagCategory();
                    setViewBagNCC();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.Quantity < 0)
                {
                    ModelState.AddModelError("Quantity", "Số lượng sách phải lớn hơn 0");
                    setViewBagCategory();
                    setViewBagNCC();
                    setViewBagNXB();
                    return View(sach);
                }
                else if (sach.SoTrang < 0)
                {
                    ModelState.AddModelError("SoTrang", "Số trang phải lớn hơn 0");
                    setViewBagCategory();
                    setViewBagNCC();
                    setViewBagNXB();
                    return View(sach);
                }
                if (bookUpdate.Update(sach))
                {// update thành công
                    SetAlert("Cập nhật thành công", "success");

                    return RedirectToAction("Index", "Product");
                }

                else
                {
                    SetAlert("Cập nhật thất bại", "error");
                    //mới thêm ngày 15-10-2023
                    setViewBagCategory(sach.DanhMucID);
                    setViewBagNCC(sach.NhaCungCapID);
                    setViewBagNXB(sach.NhaXuatBanID);
                }


            }
            setViewBagCategory(sach.DanhMucID);
            setViewBagNCC(sach.NhaCungCapID);
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

        public void setViewBagNCC(int? selectedId = null)
        {
            var drowdoad = new Supplier();
            ViewBag.NhaCungCapID = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);
        }
        public void setViewBagNXB(int? selectedId = null)
        {
            var drowdoad = new NXB();
            ViewBag.NhaXuatBanID = new SelectList(drowdoad.ListAll(), "ID", "TenNXB", selectedId);
        }

    }
}