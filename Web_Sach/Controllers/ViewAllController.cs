using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;

namespace Web_Sach.Controllers
{
   
    public class ViewAllController : Controller
    { 
        WebSachDb db = new WebSachDb();
        // sach-moi
        public ActionResult Index( string sort_by = "", int page = 1, int pageSize = 3)
        {
            IQueryable<Sach> model = db.Saches;
            ViewBag.sortBy = sort_by;// để lưu giá trị vào từng trang
           
            var query = model.Where(x => x.NgayCapNhat <= DateTime.Now);
            switch (sort_by)
            {
                case "tang":
                    query = query.OrderBy(x => x.Price);
                    break;
                case "giam":
                    query = query.OrderByDescending(x => x.Price);
                    break;
                //case "moinhat":
                //    query = query.OrderBy(x => x.TopHot > DateTime.Now);
                //    break;
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }


            model = query;

            var totalItem = model.Count(); // số lượng bản ghi

            // lấy ra danh sách danh mục
            var category = from dm in db.DanhMucSPs
                           select dm;
            ViewBag.Category = category.ToList();

          

            
            // tổng số trang
            var totalPage = (int)Math.Ceiling((double)totalItem / pageSize);// lamf tron leen
            int maxPage = 10;
            // danh sách phân trang
            model = model.Skip((page - 1) * pageSize).Take(pageSize);
            // Skip((page - 1) * pageSize): bỏ qua các phẩn tử 
            // ví dụ page =2 thì sẽ bỏ qua 3 phẩn tử trang 1

            var product = model.ToList();


            //truyền vào view
            ViewBag.totalRecord = totalItem;
            ViewBag.PageSize = pageSize;

            ViewBag.maxPage = maxPage;
            ViewBag.page = page;
            ViewBag.totalPage = totalPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return View(product);
          
        }







        //public ActionResult TopHot(string sort_by = "", int page = 1, int pageSize = 3)
        //{
        //    IQueryable<Sach> model = db.Saches;
        //    ViewBag.sortBy = sort_by;// để lưu giá trị vào từng trang

        //    var query = model.Where(x => x.TopHot > DateTime.Now && x.TopHot != null);
        //    switch (sort_by)
        //    {
        //        case "tang":
        //            query = query.OrderBy(x => x.Price);
        //            break;
        //        case "giam":
        //            query = query.OrderByDescending(x => x.Price);
        //            break;
        //        case "moinhat":
        //            query = query.OrderBy(x => x.TopHot > DateTime.Now);
        //            break;
        //        default:
        //            query = query.OrderBy(x => x.Name);
        //            break;
        //    }


        //    model = query;

        //    var totalItem = model.Count(); // số lượng bản ghi

        //    // lấy ra danh sách danh mục
        //    var category = from dm in db.DanhMucSPs
        //                   select dm;
        //    ViewBag.Category = category.ToList();




        //    // tổng số trang
        //    var totalPage = (int)Math.Ceiling((double)totalItem / pageSize);// lamf tron leen
        //    int maxPage = 10;
        //    // danh sách phân trang
        //    model = model.Skip((page - 1) * pageSize).Take(pageSize);
        //    // Skip((page - 1) * pageSize): bỏ qua các phẩn tử 
        //    // ví dụ page =2 thì sẽ bỏ qua 3 phẩn tử trang 1

        //    var product = model.ToList();


        //    //truyền vào view
        //    ViewBag.totalRecord = totalItem;
        //    ViewBag.PageSize = pageSize;

        //    ViewBag.maxPage = maxPage;
        //    ViewBag.page = page;
        //    ViewBag.totalPage = totalPage;
        //    ViewBag.First = 1;
        //    ViewBag.Last = totalPage;
        //    ViewBag.Next = page + 1;
        //    ViewBag.Prev = page - 1;

        //    return View(product);

        //}









    }
}