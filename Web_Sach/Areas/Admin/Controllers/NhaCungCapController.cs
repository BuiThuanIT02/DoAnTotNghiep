using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class NhaCungCapController : BaseController
    {
        private WebSachDb db = new WebSachDb();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var listPage = new Supplier().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }

        public ActionResult Create()
        {
            return View();

        }

        [HttpPost]

        public ActionResult Create(NhaCungCap ncc)
        {
            if (ModelState.IsValid)
            {
                var itemNCC = new Supplier();
                if (itemNCC.Compare(ncc.Name) != null)
                {// đã tồn tại tên  nxb
                    ModelState.AddModelError("Name", "Tên nhà cung cấp đã tồn tại!");
                    return View(ncc);

                }
                // tên nxb chưa tồn tại

                var indexNCC = itemNCC.InsertNCC(ncc);
                if (indexNCC > 0)
                {// thêm user thành công
                    SetAlert("Thêm nhà cung cấp thành công", "success");
                    return RedirectToAction("Index", "NhaCungCap");
                }




            }
            else
            {
                SetAlert("Thêm cung cấp thất bại", "error");
            }

            return View(ncc);

        }

        // update

        public ActionResult Update(int id)
        {
            var ncc = db.NhaCungCaps.Find(id);
            return View(ncc);
        }


        [HttpPost]

        public ActionResult Update(NhaCungCap ncc)
        {

            if (ModelState.IsValid)
            {
                var nccUpdate = new Supplier();


                if (nccUpdate.Compare(ncc))
                {
                    ModelState.AddModelError("Name", "Tên nhà cung cấp đã tồn tại");
                    return View(ncc);
                }


                if (nccUpdate.Update(ncc))
                {
                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "NhaCungCap");
                }


            }
            else
            {
                SetAlert("Update thất bại", "error");

            }
            return View(ncc);
        }

        [HttpDelete]
        public ActionResult Delete(int id)
        {
            var ncc = db.NhaCungCaps.Find(id);
            db.NhaCungCaps.Remove(ncc);
            db.SaveChanges();
            return RedirectToAction("Index", "NhaCungCap");
        }





    }
}