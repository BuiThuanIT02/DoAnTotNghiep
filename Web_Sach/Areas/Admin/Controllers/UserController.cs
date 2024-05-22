using Antlr.Runtime.Misc;
using Microsoft.AspNet.Identity;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.Dao;
using Web_Sach.Models.EF;
using Web_Sach.Session;

namespace Web_Sach.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        WebSachDb db = new WebSachDb();

        public ActionResult Index(string searchString, int page = 1, int pageSize = 20)
        {
            var listPage = new TaiKhoanModels().listPage(searchString, page, pageSize);
            ViewBag.SearchString = searchString;
            return View(listPage);
        }

        ////Thêm mới User
        //[HttpGet]
        //public ActionResult Create()
        //{
        //    setViewBagRole();
        //    return View();
        //}

        //public void setViewBagRole(string selectedId = null)
        //{
        //    if (string.IsNullOrEmpty(selectedId))
        //    {
        //        selectedId = null;
        //    }
        //    var roles = new List<SelectListItem>(){
        //       new SelectListItem {Text="Client", Value="0"},
        //       new SelectListItem{Text = "Admin", Value = "1"},
        //    };

        //    ViewBag.Role = new SelectList(roles, "Value", "Text", selectedId);

        //}


        //[HttpPost]

        //public ActionResult Create(UserAdmin user)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userAdd = new TaiKhoan()
        //        {
        //            TaiKhoan1 = user.TaiKhoan1,
        //            Password = user.Password,
        //            Address = user.Address,
        //            Phone = user.Phone,
        //            Email = user.Email,
        //            GioiTinh = user.GioiTinh,
        //            NgaySinh = user.NgaySinh,
        //            Status = user.Status,
        //            FullName = user.FullName,
        //            Role = user.Role,


        //        };
        //        var userUnique = new TaiKhoanModels().UniqueUserName(userAdd.TaiKhoan1);
        //        if (userUnique != null)
        //        {
        //            ModelState.AddModelError("TaiKhoan1", "Tài khoản đã tồn tại");

        //            setViewBagRole();
        //            return View(user);
        //        }
        //        if (user.Password != null)
        //        {
        //            var password = user.Password;

        //            if (!password.Any(char.IsUpper) || !password.Any(char.IsDigit) || password.Length < 8)
        //            {
        //                ModelState.AddModelError("Password", "Mật khẩu có độ dài lớn hơn 8 và chứa ít nhất 1 chữ viết hoa và 1 số");
        //                setViewBagRole();
        //                return View(user);
        //            }

        //        }



        //        var addUser = new TaiKhoanModels().InserUser(userAdd);
        //        if (addUser > 0)
        //        {// thêm user thành công
        //            SetAlert("Thêm User thành công", "success");
        //            return RedirectToAction("Index", "User");
        //        }

        //    }
        //    else
        //    {
        //        SetAlert("Thêm User thất bại", "error");
        //    }
        //    setViewBagRole();
        //    return View("Create");
        //}



        ///*Sửa bản ghi User*/

        //[HttpGet]

        //public ActionResult Update(int id)
        //{
        //    var userEdit = new TaiKhoanModels().EditUser(id);
        //    setViewBagRole((userEdit.Role).ToString());
        //    return View(userEdit);
        //}


        //[HttpPost]

        //public ActionResult Update(UserAdmin user)//tk
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var userAdd = new TaiKhoan()
        //        {
        //            ID = user.ID,
        //            TaiKhoan1 = user.TaiKhoan1,
        //            Password = user.Password,
        //            Address = user.Address,
        //            Phone = user.Phone,
        //            Email = user.Email,
        //            GioiTinh = user.GioiTinh,
        //            NgaySinh = user.NgaySinh,
        //            Status = user.Status,
        //            FullName = user.FullName,
        //            Role = user.Role,


        //        };



        //        var userUpdate = new TaiKhoanModels();
        //        // so sánh mật khẩu cũ và mật khẩu mới
        //        var pasOld = userUpdate.EditUser(userAdd.ID).Password;
        //        if (userUpdate.Compare(userAdd))
        //        {
        //            ModelState.AddModelError("TaiKhoan1", "Tên tài khoản đã tồn tại");
        //            setViewBagRole(userAdd.Role.ToString());
        //            return View(user);
        //        }

        //        if (pasOld != userAdd.Password)
        //        {
        //            var password = userAdd.Password;

        //            if (!password.Any(char.IsUpper) || !password.Any(char.IsDigit) || password.Length < 8)
        //            {
        //                ModelState.AddModelError("Password", "Mật khẩu có độ dài lớn hơn 8 và chứa ít nhất 1 chữ viết hoa và 1 số");
        //                setViewBagRole(userAdd.Role.ToString());
        //                return View(user);
        //            }

        //        }
        //        if (userUpdate.UpdateUser(userAdd))
        //        {
        //            SetAlert("Update bản ghi thành công", "success");
        //            return RedirectToAction("Index", "User");
        //        }


        //    }
        //    else
        //    {
        //        SetAlert("Update thất bại", "error");
        //        setViewBagRole(user.Role.ToString());
        //        return View(user);
        //    }
        //    setViewBagRole(user.Role.ToString());
        //    return View("Update");
        //}


        // End sửa bản ghi

        // Xóa bản ghi
        [HttpDelete]

        public ActionResult Delete(int id)
        {
            new TaiKhoanModels().Delete(id);
            return RedirectToAction("Index", "User");

        }


        // thay đổi trạng thái user
        [HttpPost]

        public JsonResult ChangeStatus(int id)
        {
            var userChange = new TaiKhoanModels().ChangeStatus(id);

            return Json(new
            {
                status = userChange

            });

        }

        // End thay đổi trạng thái user

        public void ExportExcel_EPPLUS()
        {
            var list = db.TaiKhoans.ToList();
            int Out_TotalRecord = list.Count();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            ExcelPackage Ep = new ExcelPackage();

            ExcelWorksheet Sheet = Ep.Workbook.Worksheets.Add("Report");
            Sheet.Cells["A1"].Value = "ID";
            Sheet.Cells["A1"].Style.Font.Bold = true;
            Sheet.Cells["A1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["B1"].Value = "Tên tài khoản";
            Sheet.Cells["B1"].Style.Font.Bold = true;
            Sheet.Cells["B1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["C1"].Value = "SDT";
            Sheet.Cells["C1"].Style.Font.Bold = true;
            Sheet.Cells["C1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["D1"].Value = "Email";
            Sheet.Cells["D1"].Style.Font.Bold = true;
            Sheet.Cells["D1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["E1"].Value = "Địa chỉ";
            Sheet.Cells["E1"].Style.Font.Bold = true;
            Sheet.Cells["E1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["F1"].Value = "Giới tính";
            Sheet.Cells["F1"].Style.Font.Bold = true;
            Sheet.Cells["F1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["G1"].Value = "Ngày sinh";
            Sheet.Cells["G1"].Style.Font.Bold = true;
            Sheet.Cells["G1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            Sheet.Cells["H1"].Value = "Họ và tên";
            Sheet.Cells["H1"].Style.Font.Bold = true;
            Sheet.Cells["H1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            int row = 2;
            if (Out_TotalRecord > 0)
            {
                foreach (var item in list)
                {
                    Sheet.Cells[string.Format("A{0}", row)].Value = item.ID;
                    Sheet.Cells[string.Format("A{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("B{0}", row)].Value = item.TaiKhoan1;
                    Sheet.Cells[string.Format("B{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    Sheet.Cells[string.Format("C{0}", row)].Value = item.Phone;
                    Sheet.Cells[string.Format("C{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Sheet.Cells[string.Format("D{0}", row)].Value = item.Email;
                    Sheet.Cells[string.Format("D{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Sheet.Cells[string.Format("E{0}", row)].Value = item.Address;
                    Sheet.Cells[string.Format("E{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Sheet.Cells[string.Format("F{0}", row)].Value = item.GioiTinh;
                    Sheet.Cells[string.Format("F{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Sheet.Cells[string.Format("G{0}", row)].Value = item.NgaySinh?.ToString("dd/MM/yyyy");
                    Sheet.Cells[string.Format("G{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Sheet.Cells[string.Format("H{0}", row)].Value = item.FullName;
                    Sheet.Cells[string.Format("H{0}", row)].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;


                    row++;
                }
            }
            Sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "Report.xlsx");
            Response.BinaryWrite(Ep.GetAsByteArray());
            Response.End();





        }
    }
}