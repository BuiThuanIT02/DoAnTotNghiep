using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Web_Sach.Models;
using Web_Sach.Models.Dao;
using Web_Sach.Models.EF;
namespace Web_Sach.Controllers
{
    public class ProductCategoryController : Controller
    {
        WebSachDb db = new WebSachDb();
        // GET: ProductCategory
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult _MenuLef(int? id)
        {
            var cate = db.DanhMucSPs.ToList();
            if (id != null)
            {
                ViewBag.cateId = id;

            }
            return PartialView(cate);
        }

        public ActionResult ProductCategory(string filterOb,string reviewOb, int? cateID, int page = 1, int pageSize = 12)
        {
            IQueryable<Sach> model = db.Saches;
            ViewBag.cateID = cateID;
            var query = FilterByPrice(filterOb,reviewOb, cateID);// lấy ra ds sản phẩm theo danh mục
            if (!query.Any())
            {// Any -> true có phần tử
                return RedirectToAction("Index", "Home");
            }
            model = query;
            // Lấy tổng số lượng sản phẩm

            var totalItem = model.Count(); // số lượng bản ghi
                                           // lấy danh mục hiện tại; 1 bản ghi

            var curentCategory = db.DanhMucSPs.Find(cateID);
            //FillerProductCategory(cateID, null);
            ViewBag.curentCategory = curentCategory;
            // tổng số trang
            var totalPage = (int)Math.Ceiling((double)totalItem / pageSize);// lamf tron leen
            int maxPage = 10;
            // danh sách phân trang
            model = model.OrderByDescending(x => x.Price).Skip((page - 1) * pageSize).Take(pageSize);
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
        public IQueryable<Sach> SortProductCategory(string sort_by, IQueryable<Sach> filter)
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
                case "moinhat":

                    query = query.OrderByDescending(x => x.NgayCapNhat);
                    break;
                case "cunhat":

                    query = query.OrderBy(x => x.NgayCapNhat);
                    break;

            }
            return query;
        }

        [HttpGet]
        public IQueryable<Sach> FilterByPrice(string filterOb,string reviewOb, int? cateID)
        {
            var query = from s in db.Saches
                        join dm in db.DanhMucSPs on s.DanhMucID equals dm.ID
                        where dm.ID == cateID
                        select s;
             List<Sach> result = new List<Sach>();
            bool isFiltered = false;
            if (filterOb != null && reviewOb != null)
            {//lọc theo cả giá và đánh giá
                var filter = new JavaScriptSerializer().Deserialize<List<string>>(filterOb);
                var filterReview = new JavaScriptSerializer().Deserialize<List<int>>(reviewOb);
                
                if (filter.Count() > 0 && filterReview.Count() > 0)
                {
                    foreach (var item in filter)
                    {
                        switch (item)
                        {
                            case "100000":
                                {

                                    //  tempQuery = query.Where(x => x.Price <= 100000);
                                    result.AddRange(query.Where(x => x.Price <= 100000).ToList());
                                    break;
                                }
                            case "100000-200000":
                                {

                                    //  tempQuery = query.Where(x => x.Price > 100000 && x.Price <= 200000);
                                    result.AddRange(query.Where(x => x.Price > 100000 && x.Price <= 200000).ToList());
                                    break;
                                }
                            case "200000-300000":
                                {

                                    // tempQuery = query.Where(x => x.Price > 200000 && x.Price <= 300000);
                                    result.AddRange(query.Where(x => x.Price > 200000 && x.Price <= 300000).ToList());
                                    break;

                                }
                            case "300000-400000":
                                {
                                    result.AddRange(query.Where(x => x.Price > 300000 && x.Price < 400000).ToList());
                                    break;
                                }
                            case "400000":
                                {
                                    result.AddRange(query.Where(x => x.Price > 400000).ToList());
                                    break;
                                }
                        }
                    }

                    var reviewQuery = from s in db.Saches
                                      join dm in db.DanhMucSPs on s.DanhMucID equals dm.ID
                                      join cm in db.Comments on s.ID equals cm.MaSach
                                      where s.DanhMucID == cateID && cm.Rate != 0
                                      group cm by new { s.ID } into g
                                      let avgRate = (int)g.Average(x => x.Rate)
                                      where filterReview.Any(fr => fr == avgRate)
                                      select new
                                      {
                                          SachID = g.Key.ID,
                                          Rate = avgRate
                                      };

                    //danh sách id sách có sao đánh giá
                    var sachIds = reviewQuery.Select(s => s.SachID).ToList();
                    var rates = reviewQuery.Select(s => s.Rate).ToList();

                    foreach (var item in filterReview)
                    {
                        var a = query.Where(x => sachIds.Contains(x.ID) && rates.Contains((int)x.Comments.Where(k => k.Rate != 0).Average(k => k.Rate)));
                        if (a.Any())
                        {
                            result.AddRange(a);
                        }
                    }
                    // Loại bỏ các mục trùng lặp
                    result = result.Distinct().ToList();
                    isFiltered = true;
                }
              
            }

            if (filterOb != null)
            {//lọc theo giá
                var filter = new JavaScriptSerializer().Deserialize<List<string>>(filterOb);
                if (filter.Count() > 0)
                {
                    foreach (var item in filter)
                    {
                        switch (item)
                        {
                            case "100000":
                                {

                                    //  tempQuery = query.Where(x => x.Price <= 100000);
                                    result.AddRange(query.Where(x => x.Price <= 100000).ToList());
                                    break;
                                }
                            case "100000-200000":
                                {

                                    //  tempQuery = query.Where(x => x.Price > 100000 && x.Price <= 200000);
                                    result.AddRange(query.Where(x => x.Price > 100000 && x.Price <= 200000).ToList());
                                    break;
                                }
                            case "200000-300000":
                                {

                                    // tempQuery = query.Where(x => x.Price > 200000 && x.Price <= 300000);
                                    result.AddRange(query.Where(x => x.Price > 200000 && x.Price <= 300000).ToList());
                                    break;

                                }
                            case "300000-400000":
                                {
                                    result.AddRange(query.Where(x => x.Price > 300000 && x.Price < 400000).ToList());
                                    break;
                                }
                            case "400000":
                                {
                                    result.AddRange(query.Where(x => x.Price > 400000).ToList());
                                    break;
                                }
                        }
                    }
                    isFiltered = true;
                }
               
            }//kết thúc lọc theo giá sp

            if (reviewOb != null)
            {// lọc theo đánh giá
                var filterReview = new JavaScriptSerializer().Deserialize<List<int>>(reviewOb);
                if (filterReview.Count() > 0)
                {
                    var reviewQuery = from s in db.Saches
                                      join dm in db.DanhMucSPs on s.DanhMucID equals dm.ID
                                      join cm in db.Comments on s.ID equals cm.MaSach
                                      where s.DanhMucID == cateID && cm.Rate != 0
                                      group cm by new { s.ID } into g
                                      let avgRate = (int)g.Average(x => x.Rate)
                                      where filterReview.Any(fr => fr == avgRate)
                                      select new
                                      {
                                          SachID = g.Key.ID,
                                          Rate = avgRate
                                      };

                    //danh sách id sách có sao đánh giá
                    var sachIds = reviewQuery.Select(s => s.SachID).ToList();
                    var rates = reviewQuery.Select(s => s.Rate).ToList();

                    foreach (var item in filterReview)
                    {
                        var resultReview = query.Where(x => sachIds.Contains(x.ID) && rates.Contains((int)x.Comments.Where(k => k.Rate != 0).Average(k => k.Rate)));
                        if (resultReview.Any())
                        {
                            result.AddRange(resultReview);
                        }
                    }
                    // Loại bỏ các mục trùng lặp
                    result = result.Distinct().ToList();
                    isFiltered = true;
                }
               
            }
            if (!isFiltered)
            {
                result = query.ToList();
            }

            return result.AsQueryable();
        }

        public ActionResult _FillerProductCategory(string filterOb,string reviewOb, int? cateId, string sortBy, int page = 1, int pageSize = 12)
        {// lọc sản phẩm xong rồi sắp xếp
            var query = FilterByPrice(filterOb,reviewOb, cateId);// lấy ds sản phẩm đã lọc
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
                sortProduct = SortProductCategory(sortBy, query);// gán query cho sortProduct
                model = sortProduct;
                totalItem = model.Count();
                model = model.Skip((page - 1) * pageSize).Take(pageSize);
            }
            var totalPage = 0;
            var curentCategory = db.DanhMucSPs.Find(cateId);
            var product = new List<Sach>();
            int maxPage = 10;
            ViewBag.curentCategory = curentCategory;
            totalPage = (int)Math.Ceiling((double)totalItem / pageSize);// lamf tron leen
            ViewBag.filterOb = filterOb;
            ViewBag.reviewOb = reviewOb;
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
            return PartialView("_FillerProductCategory", model.ToList());
        }
    }
}