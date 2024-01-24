using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class KhuyenMai_SachController : BaseController
    {
      
        public ActionResult Index(int page = 1, int pageSize = 50)
        {
            var tg = new kmSachModels().listPage(page, pageSize);
            return View(tg);
        }

        [HttpGet]
        public ActionResult Create()
        {
            setViewBagMaSach();
            setViewBagMaKM();
            return View();
        }

        [HttpPost]

        public ActionResult Create(KhuyenMai_Sach tg)
        {
            if (ModelState.IsValid)
            {
                 if(tg.Sale < 0)
                {
                    ModelState.AddModelError("Sale", "Sale phải lớn hơn 0");
                    setViewBagMaSach();
                    setViewBagMaKM();
                    return View(tg);
                }   
                var tgIndex = new kmSachModels().Insert(tg);
                if (tgIndex)
                {
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index");
                }
            }

            setViewBagMaSach();
            setViewBagMaKM();
            SetAlert("Thêm  thất bại", "error");


            return View(tg);

        }

        [HttpGet]
        public ActionResult Update(int maSach, int maKM)
        {
            
            var kmItem = new kmSachModels().Edit(maSach, maKM);
            setViewBagMaSach(kmItem.MaSach);
            setViewBagMaKM(kmItem.MaKhuyenMai);
            TempData["maSach"] = maSach;
            TempData["maKM"] = maKM;
            return View(kmItem);
        }

        [HttpPost]
        public ActionResult UpdatePost(KhuyenMai_Sach km)
        {
            if (ModelState.IsValid)
            {
                if (km.Sale < 0)
                {
                    ModelState.AddModelError("Sale", "Sale phải lớn hơn 0");
                    setViewBagMaSach(km.MaSach);
                    setViewBagMaKM(km.MaKhuyenMai);
                    return View("Update");
                }
                TempData.Keep("maSach");
                TempData.Keep("maKM");
                var tgModel = new kmSachModels();
                int maSach = (int)TempData["maSach"];
                int maKM = (int)TempData["maKM"];
                if (tgModel.Update(maSach, maKM, km))
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "KhuyenMai_Sach");

                }
            }
            setViewBagMaSach(km.MaSach);
            setViewBagMaKM(km.MaKhuyenMai);
            SetAlert("Cập nhật thất bại", "error");
            return View("Update");
        }
        [HttpDelete]
        public ActionResult Delete(int idS, int idKM)
        {
            new kmSachModels().Delete(idS, idKM);
            return RedirectToAction("Index");
        }

        public void setViewBagMaSach(int? selectedId = null)
        {
            var drowdoad = new Book();
            ViewBag.MaSach = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);// hiện thị droplisst theo Name
        }


        public void setViewBagMaKM(int? selectedId = null)
        {
            var drowdoad = new kmSachModels();
            ViewBag.MaKhuyenMai = new SelectList(drowdoad.ListAll(), "ID", "TenKhuyenMai", selectedId);// hiện thị droplisst theo Name
        }














    }
}