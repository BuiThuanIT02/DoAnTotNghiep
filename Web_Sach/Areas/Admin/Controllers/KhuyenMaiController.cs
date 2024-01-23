using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class KhuyenMaiController : BaseController
    {
        // GET: Admin/ProductCategory
        public ActionResult Index(string searchString, int page = 1, int pageSize = 10)
        {
            var km = new kmModels().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;

            return View(km);
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(KhuyenMai km)
        {
            if (ModelState.IsValid)
            {
                var kmCheck = new kmModels().uniqueName(km.TenKhuyenMai);
                if (kmCheck != null)
                {
                    ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mại đã tồn tại");
                    return View(km);
                }
              

                var kmIndex = new kmModels().Insert(km);
                if (kmIndex > 0)
                {
                    SetAlert("Thêm khuyến mại thành công", "success");
                    return RedirectToAction("Index");
                }
            }
            else
            {
                SetAlert("Thêm khuyến mại thất bại", "error");
            }

            return View(km);
        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var kmItem = new kmModels().Edit(id);
            return View(kmItem);
        }

        [HttpPost]
        public ActionResult Update(KhuyenMai km)
        {
            if (ModelState.IsValid)
            {
                var kmModel = new kmModels();

                if (kmModel.Compare(km))
                {
                    ModelState.AddModelError("TenKhuyenMai", "Tên khuyến mại đã tồn tại");
                    return View(km);
                }
                

                if (kmModel.Update(km))
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "KhuyenMai");

                }
                else
                {
                    SetAlert("Cập nhật thất bại", "error");

                }



            }
            return View(km);
        }



        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new kmModels().Delete(id);
            return RedirectToAction("Index", "KhuyenMai");
        }





    }
}