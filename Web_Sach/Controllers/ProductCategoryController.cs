using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using Web_Sach.Models;
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

        public ActionResult ProductCategory(string filterOb, int? cateID, int page = 1, int pageSize = 3)
        {
            IQueryable<Sach> model = db.Saches;
            ViewBag.cateID = cateID;
            var query = FilterByPrice(filterOb, cateID);// lấy ra ds sản phẩm theo danh mục
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
                     
                        query = query.OrderBy(x => x.Name);
                        break;
                    case "moinhat":
                      
                        query = query.OrderBy(x => x.Name);
                        break;
                    case "cunhat":
                      
                        query = query.OrderBy(x => x.Name);
                        break;

                }
            
        



            return query;
        }

        [HttpGet]
        public IQueryable<Sach> FilterByPrice(string filterOb, int? cateID)
        {
            var query = from s in db.Saches
                        join dm in db.DanhMucSPs on s.DanhMucID equals dm.ID
                        where dm.ID == cateID
                        select s;

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

        public ActionResult _FillerProductCategory(string filterOb, int? cateId, string sortBy, int page = 1, int pageSize = 3)
        {// lọc sản phẩm xong rồi sắp xếp
            var query = FilterByPrice(filterOb, cateId);// lấy ds sản phẩm đã lọc
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