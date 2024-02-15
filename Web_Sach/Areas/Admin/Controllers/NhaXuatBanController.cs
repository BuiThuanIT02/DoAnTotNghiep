using System;
using System.Collections.Generic;
using System.EnterpriseServices.CompensatingResourceManager;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class NhaXuatBanController : BaseController
    {
        private WebSachDb db = new WebSachDb();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var listPage = new NXB().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }

        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]

        public ActionResult Create(NhaXuatBan nxb)
        {
            if (ModelState.IsValid)
            {
                var itemNXB = new NXB();
                if (itemNXB.Compare(nxb.TenNXB) != null)
                {// đã tồn tại tên  nxb
                    ModelState.AddModelError("TenNXB", "Tên nhà xuất bản đã tồn tại!");
                    return View(nxb);

                }
                // tên nxb chưa tồn tại

                var indexNXB = itemNXB.InsertNXB(nxb);
                if (indexNXB > 0)
                {// thêm user thành công
                    SetAlert("Thêm nhà xuất bản thành công", "success");
                    return RedirectToAction("Index", "NhaXuatBan");
                }




            }
            else
            {
                SetAlert("Thêm nhà xuất bản thất bại", "error");
            }

            return View(nxb);

        }

        // update

        public ActionResult Update(int id)
        {
            var nxb = db.NhaXuatBans.Find(id);
            return View(nxb);
        }


        [HttpPost]

        public ActionResult Update(NhaXuatBan nxb)
        {

            if (ModelState.IsValid)
            {
                var nxbUpdate = new NXB();


                if (nxbUpdate.Compare(nxb))
                {
                    ModelState.AddModelError("TenNXB", "Tên nhà xuất bản đã tồn tại");
                    return View(nxb);
                }


                if (nxbUpdate.Update(nxb))
                {
                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "NhaXuatBan");
                }


            }
            else
            {
                SetAlert("Update thất bại", "error");

            }
            return View(nxb);
        }


        [HttpDelete]
        public ActionResult Delete(int id)
        {
            try
            {
                var nxb = db.NhaXuatBans.Find(id);
                db.NhaXuatBans.Remove(nxb);
                db.SaveChanges();
                SetAlert("Xóa bản ghi thành công", "success");
            }
            catch (Exception)
            {
                SetAlert("Xóa bản ghi thất bại", "error");
            }


            return RedirectToAction("Index", "NhaXuatBan");
        }










    }
}