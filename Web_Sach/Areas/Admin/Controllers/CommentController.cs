using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using System.Data.Entity;
using Web_Sach.Models.EF;
using Web_Sach.Session;
using Web_Sach.Models.Dao;// thêm vào mới sd đc entity

namespace Web_Sach.Areas.Admin.Controllers
{
    public class CommentController : Controller
    {
        // GET: Admin/Comment
        public ActionResult Index( int page = 1, int pageSize = 20)
        {
            using (WebSachDb db = new WebSachDb())
            {   
                var comments =new CommentDao().listPage(page,pageSize);
                return View(comments);
            }

        }

        public ActionResult Retry(int maSach, int maComment)
        {
            using (WebSachDb db = new WebSachDb())
            {
                var sachSql = db.Saches.Find(maSach);
                var comment = db.Comments.Include(c => c.TaiKhoan).Include(x => x.Sach).FirstOrDefault(x => x.ID == maComment);
                var sachComment = new Book()
                {
                    MaSach = maSach,
                    TenSP = sachSql.Name,
                    Image = sachSql.Images.FirstOrDefault(x => x.IsDefault).Image1,
                };
                ViewBag.sachComment = sachComment;
                return View(comment);
            }
        }
        [HttpPost]
        public ActionResult RetryPost(string content, int parentId, int maSach)
        {
            using (WebSachDb db = new WebSachDb())
            {
                var sessionAdmin = (UserLoginSession)Session[SessionHelper.USER_KEY];
                if (sessionAdmin != null)
                {
                    var comment = new Comment()
                    {
                        MaKH = sessionAdmin.UserID,
                        MaSach = maSach,
                        parentId = parentId,
                        Content = content,
                        CreatedDate = DateTime.Now,
                        Rate = 0,
                        Status=1,
                    };
                    db.Comments.Add(comment);
                    var commentParent = db.Comments.Find(parentId);
                  
                    if (commentParent != null)
                    {
                        // Đính kèm (Attach) và cập nhật trạng thái của một đối tượng
                        db.Entry(commentParent).State = EntityState.Modified;
                        commentParent.Status = 1;
                    }
                    db.SaveChanges();
                   
                    return Json(new { Status = true });

                }

                return Json(new { Status = false });
            }


        }
    }
}