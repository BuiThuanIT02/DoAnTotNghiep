using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.EF;
using Web_Sach.Session;

namespace Web_Sach.Controllers
{
   
    public class VouchersController : Controller
    { 
        private WebSachDb db = new WebSachDb();
        // GET: Vouchers
        //public ActionResult Index()
        //{
        //    if (Session[SessionHelper.USER_KEY] == null)
        //    {// user chưa tồn tại
        //        Session[SessionHelper.VOUCHER_KEY] = null;
        //        return RedirectToAction("VoucherEmpty", "Vouchers");

        //    }
        //    // user đã tồn tại
        //    var voucher = Session[SessionHelper.VOUCHER_KEY];
        //    var list = new List<voucherSession>();
        //    if (voucher != null)
        //    {
        //        list = (List<voucherSession>)voucher;
        //    }


        //    return View(list);
         
        //}
     public ActionResult listVoucher()
        {
            //if (Session[SessionHelper.USER_KEY] == null)
            //{// user chưa tồn tại
            //    Session[SessionHelper.VOUCHER_KEY] = null;
            //    return RedirectToAction("VoucherEmpty", "Vouchers");

            //}
            // user đã tồn tại
            var voucher = Session[SessionHelper.VOUCHER_KEY];
            var list = new List<voucherSession>();
            if (voucher != null)
            {
                list = (List<voucherSession>)voucher;
            }

            return PartialView(list);
        }
        public ActionResult VoucherEmpty()
        {
            return View();
        }

   
        public JsonResult AddVoucher(int voucherId)
        {
            if (Session[SessionHelper.USER_KEY] == null)
            {// user chưa tồn tại
                Session[SessionHelper.VOUCHER_KEY] = null;
                return Json(new { status = false, role = 0 });

            }
            var voucherSession = Session[SessionHelper.VOUCHER_KEY]; // lấy voucher
            var list = new List<voucherSession>();
            // lấy 1 đối tượng sách
            var voucherItem = from v in db.Vouchers 
                              where v.ID == voucherId
                              select v;

            if (voucherSession != null)
            {// đã tồn tại voucher
                list = (List<voucherSession>)voucherSession;
                // kiểm tra xem voucher đã có trong giỏ  chưa
                if (list.Exists(x => x.voucher.ID == voucherId))
                {
                    return Json(new { status = false, voucherNotEmpty=1 });
                }
                else
                { // voucher đó chưa có trong giỏ  
                    var item = new voucherSession();
                    item.voucher = voucherItem.FirstOrDefault();             
                    list.Add(item);

                }

                Session[SessionHelper.VOUCHER_KEY] = list;

            }
            else
            { // chưa tồn tại voucher               
                var item = new voucherSession();
                item.voucher = voucherItem.FirstOrDefault();
                list.Add(item);
                Session[SessionHelper.VOUCHER_KEY] = list;
            }
          
            return Json(new { status = true });
        }

    }
}