using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class NewsController : BaseController
    {
        // GET: Admin/News
        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var listPage = new newModel().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }

        //Thêm mới 
        [HttpGet]
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Tin_Tuc news)
        {
            if (ModelState.IsValid)
            {
                var imgUnique = new newModel().Unique(news.Name);
                if (imgUnique)
                {
                    ModelState.AddModelError("Name", "Tên đã tồn tại");
                    return View(news);
                }
                var addNew = new newModel().InserNew(news);
                if (addNew > 0)
                {// thêm silde thành công
                    SetAlert("Thêm tin tức thành công", "success");
                    return RedirectToAction("Index", "News");
                }
                else
                {
                    SetAlert("Thêm tin tức thất bại", "error");
                }

            }
            return View("Create");

        }


        [HttpGet]
        public ActionResult Update(int id)
        {
            var newEdit = new newModel().EditNew(id);
            return View(newEdit);
        }
        [HttpPost]
        public ActionResult Update(Tin_Tuc news)
        {
            if (ModelState.IsValid)
            {
                var newUp = new newModel();
                if (newUp.Compare(news))
                {
                    ModelState.AddModelError("Name", "Tên đã tồn tại");

                    return View(news);
                }
                if (newUp.UpdateNew(news))
                {
                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "News");
                }
            }

            SetAlert("Update thất bại", "error");
            return View(news);


        }
        // Xóa bản ghi
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            new newModel().Delete(id);

            return RedirectToAction("Index", "News");

        }


    }
}