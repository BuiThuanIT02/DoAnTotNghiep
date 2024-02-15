using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Areas.Admin.Data;
using Web_Sach.Models;

using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class KhuyenMaiController : BaseController
    {
        // GET: Admin/ProductCategory
        private WebSachDb db = new WebSachDb();
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var km = new kmModels().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(km);
        }

        [HttpGet]

        public ActionResult Create()
        {
            setViewBagMaSach();
            return View();
        }

        [HttpPost]

        public ActionResult Create(KhuyenMaiModel km)
        {
            if (ModelState.IsValid)
            {
                if (km.Sale < 0)
                {
                    ModelState.AddModelError("Sale", "Sale phải lớn hơn 0");
                    setViewBagMaSach();                 
                    return View(km);
                }
                if (km.NgayBatDau > km.NgayKeThuc)
                {
                    ModelState.AddModelError("NgayKeThuc", "Ngày kết thúc phải lớn hơn ngày bắt đầu!");
                    setViewBagMaSach();
                    return View(km);
                }
                var kmNew = new KhuyenMai()
                {
                    TenKhuyenMai = km.TenKhuyenMai,
                    NgayBatDau = km.NgayBatDau,
                    NgayKeThuc = km.NgayKeThuc,
                };
               
                var kmCheck = new kmModels().uniqueName(kmNew.TenKhuyenMai);
                if (kmCheck != null)
                {
                    ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mại đã tồn tại");
                    setViewBagMaSach();
                    return View(km);
                }
              

                var kmIndex = new kmModels().Insert(kmNew);
                if (kmIndex > 0)
                {
                    var kmSachNew = new KhuyenMai_Sach()
                    {
                        MaKhuyenMai = kmIndex,
                        MaSach = km.MaSach,
                        Sale = km.Sale,
                    };
                    db.KhuyenMai_Sach.Add(kmSachNew);
                    db.SaveChanges();
                    SetAlert("Thêm khuyến mại thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                SetAlert("Thêm khuyến mại thất bại", "error");
            }
            setViewBagMaSach();
            return View(km);
        }

        [HttpGet]
        public ActionResult Update(int maSach , int maKM)
        {
            var kmItem = new kmModels().Edit(maSach,maKM);
            TempData["maSach"] = maSach;
            setViewBagMaSach(kmItem.MaSach);
            return View(kmItem);
        }

        [HttpPost]
        public ActionResult Update(KhuyenMaiModel km)
        {
            if (ModelState.IsValid)
            {
                if (km.Sale < 0)
                {
                    ModelState.AddModelError("Sale", "Sale phải lớn hơn 0");
                    setViewBagMaSach(km.MaSach);
                    return View(km);
                }
                if (km.NgayBatDau > km.NgayKeThuc)
                {
                    ModelState.AddModelError("NgayKeThuc", "Ngày kết thúc phải lớn hơn ngày bắt đầu!");
                    setViewBagMaSach(km.MaSach);
                    return View(km);
                }                                                                    
                var kmModel = new kmModels();
                var kmSach = new KhuyenMai()
                {
                    ID = km.MaKM,
                    TenKhuyenMai = km.TenKhuyenMai,
                    NgayBatDau = km.NgayBatDau,
                    NgayKeThuc = km.NgayKeThuc,

                };
              
                if (kmModel.Compare(kmSach))
                {
                    ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mại đã tồn tại");
                    setViewBagMaSach(km.MaSach);
                    return View(km);
                } 
                   TempData.Keep("maSach");
                   int maSachOld = (int)TempData["maSach"];

                if (kmModel.Update(kmSach))
                {
                    try
                    {
                        var kmOld = db.KhuyenMai_Sach.Where(t => t.MaKhuyenMai == km.MaKM && t.MaSach == maSachOld).FirstOrDefault();
                        if (kmOld != null)
                        {
                            // Mới chưa tồn tại, cũ đã tồn tại
                            if (kmOld.MaSach != km.MaSach )
                            {
                                // Tạo một đối tượng mới với giá trị mới
                                var newKm_Sach = new KhuyenMai_Sach
                                {
                                    MaSach = km.MaSach,
                                    MaKhuyenMai = km.MaKM,
                                    Sale = km.Sale,
                                    // Các thuộc tính khác nếu có
                                };
                                // Xóa đối tượng cũ
                                db.KhuyenMai_Sach.Remove(kmOld);
                                // Thêm đối tượng mới vào cơ sở dữ liệu
                                db.KhuyenMai_Sach.Add(newKm_Sach);
                                // Lưu thay đổi vào cơ sở dữ liệu
                                db.SaveChanges();                               
                            }
                            else
                            {// sách mới  == sách cũ
                                if (kmOld.Sale != km.Sale)
                                {
                                    kmOld.Sale = km.Sale;
                                    db.SaveChanges();
                                    
                                }
                               
                            }
                        }


                    }
                    catch (Exception)
                    {
                        setViewBagMaSach(km.MaSach);
                        ModelState.AddModelError("MaSach", "Tên sách không hợp lệ!!.");
                        return View(km);
                    }





                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "KhuyenMai");

                }
                else
                {
                    setViewBagMaSach(km.MaSach);
                    SetAlert("Cập nhật thất bại", "error");

                }



            }
            return View(km);
        }



        [HttpDelete]
        public ActionResult Delete(int maSach, int maKM)
        {
            var check = new kmModels().Delete(maSach,maKM);
            if (check)
            {
                SetAlert("Xóa bản ghi thành công", "success");
            }
            else
            {
                SetAlert("Xóa bản ghi thất bại", "error");
            }
            return RedirectToAction("Index", "KhuyenMai");
        }


        public void setViewBagMaSach(int? selectedId = null)
        {
            var drowdoad = new Book();
            ViewBag.MaSach = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);// hiện thị droplisst theo Name
        }


    }
}