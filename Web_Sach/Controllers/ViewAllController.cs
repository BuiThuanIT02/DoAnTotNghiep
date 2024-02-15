using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web_Sach.Models;

namespace Web_Sach.Controllers
{
   
    public class ViewAllController : Controller
    { 
        WebSachDb db = new WebSachDb();
        // sach-moi
        public ActionResult Index( string filterOb,int page = 1, int pageSize = 3)
        {
            IQueryable<Sach> model = db.Saches;
            var query = FilterByPrice(filterOb);// lấy ra ds sản phẩm theo danh mục
            if (!query.Any())
            {// Any -> true có phần tử
                return RedirectToAction("Index", "Home");
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

        public ActionResult _FillerProductViewAll(string filterOb, string sortBy, int page = 1, int pageSize = 3)
        {// lọc sản phẩm xong rồi sắp xếp
            var query = FilterByPrice(filterOb);// lấy ds sản phẩm đã lọc
            IQueryable<Sach> sortProduct = db.Saches;
            IQueryable<Sach> model = db.Saches;
            var totalItem = 0;
            if (string.IsNullOrEmpty(sortBy))
            {
                model = query;
                totalItem = model.Count();
                model = model.OrderByDescending(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize);
            }
            else
            {
                sortProduct = SortProductViewAll(sortBy, query);// gán query cho sortProduct
                model = sortProduct;
                totalItem = model.Count();
                model = model.Skip((page - 1) * pageSize).Take(pageSize);
            }

            var totalPage = 0;
           
            var product = new List<Sach>();
            int maxPage = 10;
           
            totalPage = (int)Math.Ceiling((double)totalItem / pageSize);// lamf tron leen
            ViewBag.filterOb = filterOb;
            ViewBag.sortBy = sortBy;
            ViewBag.totalRecord = totalItem;
            ViewBag.PageSize = pageSize;
            ViewBag.maxPage = maxPage;
            ViewBag.page = page;
            ViewBag.totalPage = totalPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;

            return PartialView("_FillerProductViewAll", model.ToList());
        }
      
        public IQueryable<Sach> SortProductViewAll(string sort_by, IQueryable<Sach> filter)
        {
            var query = filter;
            switch (sort_by)
            {
                case "a-z":

                    query = query.OrderBy(x => x.Name);
                    break;
                case "z-a":

                    query = query.OrderByDescending(x => x.Name);
                    break;
             
                case "cunhat":

                    query = query.OrderBy(x => x.NgayCapNhat);
                    break;

            }





            return query;
        }

        [HttpGet]
        public IQueryable<Sach> FilterByPrice(string filterOb)
        {
            IQueryable<Sach> query = db.Saches.OrderByDescending(x => x.NgayCapNhat);

            if (filterOb != null)
            {
                var filter = new JavaScriptSerializer().Deserialize<List<string>>(filterOb);

                foreach (var item in filter)
                {
                    switch (item)
                    {
                        case "100000":
                            {
                                query = query.Where(x => x.Price <= 100000);
                                break;
                            }

                        case "100000-200000":
                            {
                                query = query.Where(x => x.Price > 100000 && x.Price <= 200000);
                                break;
                            }
                        case "200000-300000":
                            {
                                query = query.Where(x => x.Price > 200000 && x.Price <= 300000);
                                break;

                            }
                        case "300000-400000":
                            {
                                query = query.Where(x => x.Price > 300000 && x.Price <= 400000);
                                break;

                            }
                        case "400000":
                            {
                                query = query.Where(x => x.Price > 400000);
                                break;
                            }

                    }
                }
            }
            return query;
        }

        public ActionResult TopHot(string sort_by = "", int page = 1, int pageSize = 3)
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
                case "moinhat":
                    //query = query.OrderBy(x => x.TopHot > DateTime.Now);
                    break;
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









    }
}