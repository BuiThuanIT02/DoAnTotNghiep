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
    public class AuthorController : BaseController
    {
        // GET: Admin/Author
        WebSachDb db = new WebSachDb();
        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var listPage = new TacGiaModels().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }



        public ActionResult Create()
        {
            setViewBagMaSach();
            return View();

        }

        [HttpPost]

        public ActionResult Create(TacGiaAdmin inputTacGia)
        {
            if (ModelState.IsValid)
            {
                var tacGia = new TacGiaModels();
                var author = new TacGia()
                {
                    TenTacGia = inputTacGia.TenTacGia,
                    Address = inputTacGia.Address,
                    Phone = inputTacGia.Phone,
                    TieuSu = inputTacGia.TieuSu,
                };
                if (tacGia.Compare(author.TenTacGia) != null)
                {// đã tồn tại tên  tác giả
                    ModelState.AddModelError("TenTacGia", "Tên tác giả đã tồn tại!");
                    setViewBagMaSach();
                    return View(inputTacGia);

                }
                // tên tác giả chưa tồn tại

               var index = tacGia.InsertAuthor(author);
                if (index > 0)
                {// thêm user thành công
                    var thamGia = new ThamGia()
                    {
                        MaTacGia=index,
                        MaSach=inputTacGia.MaSach,
                    };
                    db.ThamGias.Add(thamGia);
                    db.SaveChanges();
                    SetAlert("Thêm tác giả thành công", "success");
                    return RedirectToAction("Index", "Author");
                }


            }
            else
            {
                setViewBagMaSach();
                SetAlert("Thêm tác giả thất bại", "error");
            }

            return View(inputTacGia);

        }


        // update

        public ActionResult Update(int maSach, int maTacGia)
        {
            var author = new TacGiaModels().Edit(maSach, maTacGia);
            TempData["maSach"] = maSach;
            setViewBagMaSach(author.MaSach);
            return View(author);
        }


        [HttpPost]

        public ActionResult Update(TacGiaAdmin inputTacGia)
        {

            if (ModelState.IsValid)
            {
                var authorUpdate = new TacGiaModels();
                var tk =new TacGia()
                {
                    ID=inputTacGia.MaTacGia,
                    TenTacGia = inputTacGia.TenTacGia,
                    Address = inputTacGia.Address,
                    Phone = inputTacGia.Phone,
                    TieuSu = inputTacGia.TieuSu,
                };

                if (authorUpdate.Compare(tk))
                {
                    ModelState.AddModelError("TenTacGia", "Tên tác giả đã tồn tại");
                    setViewBagMaSach(inputTacGia.MaSach);
                    return View(inputTacGia);
                }
                TempData.Keep("maSach");
                int maSachOld = (int)TempData["maSach"];

                if (authorUpdate.Update(tk))
                {

                    try
                    {
                        var km = db.ThamGias.Where(t => t.MaTacGia == inputTacGia.MaTacGia && t.MaSach == maSachOld).FirstOrDefault();
                        if (km != null)
                        {
                            // Mới chưa tồn tại, cũ đã tồn tại
                            if (km.MaSach != inputTacGia.MaSach )
                            {
                                // Tạo một đối tượng mới với giá trị mới
                                var newThamGia = new ThamGia
                                {
                                    MaSach = inputTacGia.MaSach,
                                    MaTacGia = inputTacGia.MaTacGia
                                    // Các thuộc tính khác nếu có
                                };

                             

                                // Xóa đối tượng cũ
                                db.ThamGias.Remove(km);
                                 // Thêm đối tượng mới vào cơ sở dữ liệu
                                db.ThamGias.Add(newThamGia);
                                // Lưu thay đổi vào cơ sở dữ liệu
                                db.SaveChanges();

                              
                            }
                           
                        }

                    }
                    catch (Exception)
                    {
                        setViewBagMaSach(inputTacGia.MaSach);
                        ModelState.AddModelError("MaSach", "Tên sách không hợp lệ!!.");
                        return View(inputTacGia);
                    }






                    SetAlert("Update bản ghi thành công", "success");
                    return RedirectToAction("Index", "Author");
                }


            }
            else
            {
                setViewBagMaSach(inputTacGia.MaSach);
                SetAlert("Update thất bại", "error");
                return View(inputTacGia);
            }
            return View("Index");
        }



        [HttpDelete]
        public ActionResult Delete(int maSach, int maTacGia)
        {
            var author = new TacGiaModels().Delete(maSach, maTacGia);
            return RedirectToAction("Index", "Author");
        }



        public void setViewBagMaSach(int? selectedId = null)
        {
            var drowdoad = new Book();
            ViewBag.MaSach = new SelectList(drowdoad.ListAll(), "ID", "Name", selectedId);// hiện thị droplisst theo Name
        }





    }
}