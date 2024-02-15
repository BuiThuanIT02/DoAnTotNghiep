using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class VoucherController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var voucher = new VoucherModels().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(voucher);
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(Voucher voucher)
        {
            if (ModelState.IsValid)
            {
                var voucherCheck = new VoucherModels().uniqueName(voucher.MaVoucher);
                if (voucherCheck != null)
                {
                    ModelState.AddModelError("MaVoucher", "Mã voucher đã tồn tại");
                    return View(voucher);
                }
                if(voucher.SoLanSuDung < 0)
                {
                    ModelState.AddModelError("SoLanSuDung", "Số lần sử dụng phải lớn hơn 0");
                    return View(voucher);
                }
                else if(voucher.DonGiaToiThieu < 0)
                {
                    ModelState.AddModelError("DonGiaToiThieu", "Đơn giá tối thiểu phải lớn hơn 0");
                    return View(voucher);
                }
                else if (voucher.SoTienGiam < 0)
                {
                    ModelState.AddModelError("SoTienGiam", "Số tiền giảm phải lớn hơn 0");
                    return View(voucher);
                }
                else if (voucher.NgayTao > voucher.NgayHetHan)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn ngày tạo!");

                    return View(voucher);
                }

                var voucherIndex = new VoucherModels().Insert(voucher);
                if (voucherIndex > 0)
                {
                    SetAlert("Thêm voucher thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                SetAlert("Thêm voucher thất bại", "error");
            }

            return View(voucher);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var voucherItem = new VoucherModels().Edit(id);
            return View(voucherItem);
        }

        [HttpPost]
        public ActionResult Update(Voucher dm)
        {
            if (ModelState.IsValid)
            {
                var voucher = new VoucherModels();

                if (voucher.Compare(dm))
                {
                    ModelState.AddModelError("MaVoucher", "Mã voucher đã tồn tại");
                    return View(dm);
                }
                if (dm.SoLanSuDung < 0)
                {
                    ModelState.AddModelError("SoLanSuDung", "Số lần sử dụng phải lớn hơn 0");
                    return View(dm);
                }
                else if (dm.DonGiaToiThieu < 0)
                {
                    ModelState.AddModelError("DonGiaToiThieu", "Đơn giá tối thiểu phải lớn hơn 0");
                    return View(dm);
                }
                else if (dm.SoTienGiam < 0)
                {
                    ModelState.AddModelError("SoTienGiam", "Số tiền giảm phải lớn hơn 0");
                    return View(dm);
                }
                else if (dm.NgayTao > dm.NgayHetHan)
                {
                    ModelState.AddModelError("NgayHetHan", "Ngày hết hạn phải lớn hơn ngày tạo!");
                   
                    return View(dm);
                }

                if (voucher.Update(dm))
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "Voucher");

                }
                else
                {
                    SetAlert("Cập nhật thất bại", "error");
                   
                }



            }
            return View(dm);
        }


        [HttpDelete]
        public ActionResult Delete(int id)
        {
           var check= new VoucherModels().Delete(id);
            if (check)
            {
                SetAlert("Xóa bản ghi thành công", "success");
            }
            else
            {
                SetAlert("Xóa bản ghi thất bại", "error");
            }
            return RedirectToAction("Index", "Voucher");
        }




    }
}