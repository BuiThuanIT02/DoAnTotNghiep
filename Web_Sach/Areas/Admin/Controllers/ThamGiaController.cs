using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class ThamGiaController : BaseController
    {
      
        public ActionResult Index( int page = 1, int pageSize = 50)
        {
            var tg = new thamGiaModels().listPage( page, pageSize);      
            return View(tg);
        }
        [HttpGet]
        public ActionResult Create()
        {
            setViewBagMaSach();
            setViewBagMaTacGia();
            return View();
        }

        [HttpPost]

        public ActionResult Create(ThamGia tg)
        {
            if (ModelState.IsValid)
            {

                var tgIndex = new thamGiaModels().Insert(tg);
                if (tgIndex)
                {
                    SetAlert("Thêm thành công", "success");
                    return RedirectToAction("Index");
                }
            }
           
                setViewBagMaSach();
                setViewBagMaTacGia();
                SetAlert("Thêm  thất bại", "error");
            

            return View(tg);

        }

        [HttpGet]
        public ActionResult Update(int maSach, int maTacGia)
        {
            var tgItem = new thamGiaModels().Edit(maSach,maTacGia);
            setViewBagMaSach();
            setViewBagMaTacGia();
            TempData["maSach"] = maSach;
            TempData["maTacGia"] = maTacGia;
            return View(tgItem);
        }

        [HttpPost]
        public ActionResult UpdatePost(ThamGia km)
        {
            if (ModelState.IsValid)
            {
                TempData.Keep("maSach");
                TempData.Keep("maTacGia");
                var tgModel = new thamGiaModels();
                int maSach = (int)TempData["maSach"] ;
                var maTacGia = (int)TempData["maTacGia"];
                if (tgModel.Update(maSach,maTacGia,km))
                {
                    SetAlert("Cập nhật thành công", "success");
                    return RedirectToAction("Index", "ThamGia");

                }              
            }
            setViewBagMaSach();
            setViewBagMaTacGia();
            SetAlert("Cập nhật thất bại", "error");
            return View("Update");
        }


        [HttpDelete]
        public ActionResult Delete(int idS,int idT)
        {
            new thamGiaModels().Delete(idS,idT);
            return RedirectToAction("Index");
        }


        public void setViewBagMaSach(int? selectedId = null)
        {
            var drowdoad = new Book();
            ViewBag.MaSach = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);// hiện thị droplisst theo Name
        }


        public void setViewBagMaTacGia(int? selectedId = null)
        {
            var drowdoad = new TacGiaModels ();
            ViewBag.MaTacGia = new SelectList(drowdoad.ListAll(), "ID", "TenTacGia", selectedId);// hiện thị droplisst theo Name
        }





    }
}