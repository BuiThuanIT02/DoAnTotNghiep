using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web_Sach.Models;

namespace Web_Sach.Controllers
{
    public class IntroduceController : Controller
    {
        WebSachDb db = new WebSachDb();
        public ActionResult Index()
        {
          
            return View();
        }
        public ActionResult News(int page = 1, int pageSize = 3)
        {
            var news = db.Tin_Tuc.AsEnumerable();
            // phân trang
            var totalItem = news.Count();
            var totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
            var maxPage = 20;
            // danh sách phân trang
            news = news.OrderBy(x => x.CreatedDate).Skip((page - 1) * pageSize).Take(pageSize);
            // Skip((page - 1) * pageSize): bỏ qua các phẩn tử 
            // ví dụ page =2 thì sẽ bỏ qua 3 phẩn tử trang 1
         //  ViewBag.ListSach = sach.ToList();
            //truyền vào view
            ViewBag.totalRecord = totalItem;
            ViewBag.maxPage = maxPage;
            ViewBag.page = page;
            ViewBag.totalPage = totalPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;
            return View(news.ToList());
        }
        public ActionResult NewDetail(int newID)
        {
            var tinTuc = db.Tin_Tuc.Find(newID);
            return View(tinTuc);
        }
    }
}