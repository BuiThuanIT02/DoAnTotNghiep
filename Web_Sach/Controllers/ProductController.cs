using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Web_Sach.Models;
using Web_Sach.Models.Dao;
using Web_Sach.Models.EF;
using Web_Sach.Session;

namespace Web_Sach.Controllers
{
    public class ProductController : Controller
    {
        WebSachDb db = new WebSachDb();
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }
        // gửi lên từ đg dẫn chi-tiet-/metatile-id
        //[Route("chi-tiet/{metatilte}-{detailId}")]
        public ActionResult Product(int detailId)
        {
            // bình luận sản phẩm
            var sessionUser = (UserLoginSession)Session[SessionHelper.USER_KEY];
            if(sessionUser != null )
            {
                ViewBag.UserId = sessionUser.UserID;
                ViewBag.ListComment = new CommentDao().ListCommentViewModel(0,detailId);
            // bình luận sản phẩm
            }
          
            // lấy nhiều ảnh từ bảng Images
            var imagesBook = from img in db.Images
                             join s in db.Saches on img.MaSP equals s.ID
                             where img.MaSP == detailId
                             select img;
            ViewBag.listImageBook = imagesBook.ToList();
            var listVoucher = db.Vouchers.ToList();
            ViewBag.listVoucher = listVoucher;
            // lấy tên tác giả
            var tacgia = (from s in db.Saches
                          join tg in db.ThamGias on s.ID equals tg.MaSach
                          join outhur in db.TacGias on tg.MaTacGia equals outhur.ID
                          where s.ID == detailId
                          select new 
                          {
                              Names = outhur.TenTacGia,
                              IDs = outhur.ID
                          }).AsEnumerable().Select(x=> new ThamGiaViewModels
                          {
                              Name= x.Names,
                              ID = x.IDs
                          });

            ViewBag.NameOuthur = tacgia.ToList();
            // sách cùng tác giả
            var sachCungTacGia = ViewBag.NameOuthur as List<ThamGiaViewModels>;
            if (sachCungTacGia != null && sachCungTacGia.Count > 0)
            {
                // laays danh sach ID tac gia
                var id = sachCungTacGia.Select(x => x.ID).ToList();
                var result = from s in db.Saches
                             join t in db.ThamGias on s.ID equals t.MaSach
                             join tg in db.TacGias on t.MaTacGia equals tg.ID
                             where id.Contains(tg.ID) && s.ID != detailId
                             select s;
                ViewBag.SachCungTacGia = result.ToList();
            }


            // lấy chi tiết sách

            var query = (from dt in db.Saches
                         where dt.ID == detailId
                         select dt);
            if(query == null)
            {
                return RedirectToAction("Index", "Error");
            }
            var productDetail = query.FirstOrDefault();

            if (productDetail != null)
            {

                var idDm = productDetail.DanhMucID;
                var danhmuc = from dm in db.DanhMucSPs
                              where dm.ID == idDm
                              select dm.Name;
                // lấy sách cx loại 
                var sachDM = from s in db.Saches
                             where s.DanhMucID == idDm
                             select s;
                ViewBag.SachDM = sachDM.ToList();
                ViewBag.NameDM = danhmuc.FirstOrDefault();

            }

            return View(productDetail);

        }



        public ActionResult findsimilarbooks(string keyword)
        {
            List<Sach> books = (from s in db.Saches
                                select s).ToList();
            List<Sach> similarbooks = new List<Sach>();

            foreach (var book in books)
            {
                int distance = levenshteindistance(keyword.ToLower(), book.Name.ToLower());
                // hoặc bạn có thể tính khoảng cách với author hoặc thông tin khác.
                double similarity = 0;
                // điều này có thể cần một ngưỡng để quyết định xem đâu sách có đủ giống.
                // ví dụ: nếu khoảng cách nhỏ hơn hoặc bằng 3 thì coi là tương tự.
                if (distance != 0)
                {
                    similarity = Math.Round((double)((book.Name.Length - distance) * 100) / book.Name.Length);
                }

                if (distance == 0)
                {
                    similarbooks.Add(book);
                    break;
                }
                else if (similarity >= 60)
                {
                    similarbooks.Add(book);
                }
            }

            return View(similarbooks);
        }


        public int levenshteindistance(string S1, string S2)
        {
            if (S1.Length < S2.Length)
            {
                string temp = S1;
                S1 = S2;
                S2 = temp;
            }

            int[,] KQ = new int[S1.Length + 1, S2.Length + 1];

            for (int i = 0; i <= S1.Length; i++)
            {
                KQ[i, 0] = i;
            }

            for (int i = 0; i <= S2.Length; i++)
            {
                KQ[0, i] = i;
            }

            for (int j = 1; j <= S1.Length; j++)
            {
                for (int i = 1; i <= S2.Length; i++)
                {
                    int x = (S1[j - 1] == S2[i - 1]) ? 0 : 1;

                    KQ[j, i] = Math.Min(
                        KQ[j - 1, i - 1] + x,
                        Math.Min(KQ[j, i - 1] + 1, KQ[j - 1, i] + 1)
                    );
                }
            }

            int min = 0;
            int min2;
            for (int j = 1; j <= S1.Length; j++)
            {
                min = S1.Length + 1;
                min2 = S2.Length + 1;

                for (int i = 1; i <= S2.Length; i++)
                {
                    if (KQ[j, i] < min2)
                    {
                        min2 = KQ[j, i];
                    }
                }

                if (min > min2)
                {
                    min = min2;
                }
            }
            return min;
        }


        //tìm kiếm Home
        public ActionResult Search(string keyword, int page = 1, int pageSize = 6)
        {
            if (keyword == "")
            {
                return Redirect("/");
            }
            var productList = db.Saches.Where(x => x.Name.ToLower().Replace(" ", "").Contains(keyword.ToLower().Replace(" ", "")));
            ViewBag.Keyword = keyword;
            ViewBag.Count = productList.Count();
            var totalItem = productList.Count();
            var totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
            var maxPage = 20;
            // danh sách phân trang
            productList = productList.OrderBy(x => x.Name).Skip((page - 1) * pageSize).Take(pageSize);
            // Skip((page - 1) * pageSize): bỏ qua các phẩn tử 
            // ví dụ page =2 thì sẽ bỏ qua 3 phẩn tử trang 1
            var model = productList.ToList();
            //truyền vào view
            ViewBag.totalRecord = totalItem;
            ViewBag.maxPage = maxPage;
            ViewBag.page = page;
            ViewBag.totalPage = totalPage;
            ViewBag.First = 1;
            ViewBag.Last = totalPage;
            ViewBag.Next = page + 1;
            ViewBag.Prev = page - 1;


            return View(model);



        }






















    }
}