using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class SildeController : BaseController
    {
        // GET: Admin/Silde
        public ActionResult Index(int page =1, int pageSize = 20)
        {
            var listPage = new SlideModel().listPage(page, pageSize);

            return View(listPage);

        }
        //Thêm mới Silde
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public ActionResult Create(Silde silde)
        {
            if (ModelState.IsValid)
            {
                var imgUnique = new SlideModel().Unique(silde.Image);
                if (imgUnique)
                {
                    ModelState.AddModelError("Image", "Hình ảnh đã tồn tại");
                    return View(silde);
                }
                var addSilde = new SlideModel().InserSilde(silde);
                if (addSilde > 0)
                {// thêm silde thành công
                    SetAlert("Thêm Silde thành công", "success");
                    return RedirectToAction("Index", "Silde");
                }
                else
                {
                    SetAlert("Thêm Slide thất bại", "error");
                }

            }
            return View("Index", "Silde");

        }

        [HttpGet]
        public ActionResult Update(int id)
        {
            var sildeEdit = new SlideModel().EditSilde(id);

            return View(sildeEdit);


        }
        [HttpPost]

        public ActionResult Update(Silde sl)
        {
            if (ModelState.IsValid)
            {
                var slideUp = new SlideModel();
                if (slideUp.Compare(sl))
                {
                    ModelState.AddModelError("Image", "Hình ảnh  đã tồn tại");

                    return View(sl);
                }
                if (slideUp.UpdateSilde(sl))
                {
                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "Silde");
                }
            }

            SetAlert("Update thất bại", "error");
            return View(sl);


        }
        // Xóa bản ghi
        [HttpDelete]

        public ActionResult Delete(int id)
        {
            new SlideModel().Delete(id);


            return RedirectToAction("Index", "Silde");

        }

        [HttpPost]

        public JsonResult ChangeStatus(int id)
        {
            var slideChange = new SlideModel().ChangeStatus(id);

            return Json(new
            {
                status = slideChange

            });

        }




    }    //end
}
