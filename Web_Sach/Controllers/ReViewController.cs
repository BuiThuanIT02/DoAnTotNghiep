using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sach.Models;
using Web_Sach.Models.Dao;
using Web_Sach.Models.EF;
using Web_Sach.Session;


namespace Web_Sach.Controllers
{
    

    public class ReViewController : Controller
    {

        // GET: ReView
        public ActionResult Index()
        {
            return View();
        }
     
        public PartialViewResult _ReView(int productId)
        {
            using (WebSachDb db = new WebSachDb())
            {
                var sessionUser = Session[SessionHelper.USER_KEY];
                var item = new reViewModel();
                var user = (UserLoginSession)Session[SessionHelper.USER_KEY];    
                ViewBag.ProductId = productId;
                ViewBag.UserId = user.UserID;
                if (sessionUser != null)
                { 
                    item.MaKH = user.UserID;
                    item.MaSach= productId;

                    return PartialView(item);
                }
            }

            return PartialView();
        }
 
        public PartialViewResult _Load_Review(int productId)
        {
            using (WebSachDb db = new WebSachDb())
            {
                var session = Session[SessionHelper.USER_KEY];
                var user = (UserLoginSession)Session[SessionHelper.USER_KEY];
                if (session != null)
                {
                    var item = db.ReViews.Where(x => x.MaSach == productId && x.MaKH == user.UserID).OrderByDescending(x => x.ID).ToList();
                    return PartialView(item);
                }
                return null;
            }
        }

        [ChildActionOnly]   
        public ActionResult _ChildComment(int parentId, int productId)
        {
            var data = new CommentDao().ListCommentViewModel(parentId, productId);
            var user = (UserLoginSession)Session[SessionHelper.USER_KEY];
            var count = data.Count();
            for (int k = 0; k < count; k++)
            {
                data[k].MaKH = user.UserID;
            }

            return PartialView("~/Views/Shared/_ChildComment.cshtml",data);
        }

        [HttpPost]
        public JsonResult PostReView(reViewModel req)
        {

            var session = Session[SessionHelper.USER_KEY];

            if (session != null)
            {
                using (WebSachDb db = new WebSachDb())
                {
                   var dao = new CommentDao();
                    Comment comment= new Comment();
                    comment.parentId = req.parentId;
                    comment.MaKH= req.MaKH;
                    comment.MaSach = req.MaSach;
                    comment.Content = req.Content;
                    comment.Rate = req.Rate;
                    comment.CreatedDate= DateTime.Now;
                    comment.Status = 0;
                    bool addComment = dao.Insert(comment);
                    if (addComment)
                    {
                        return Json(new { success = true });
                    }
                }
            }
            return Json(new { success = false, session = 0 });


        }
      
        [HttpGet]
        public ActionResult GetComment(int productId)
        {
            var data = new CommentDao().ListCommentViewModel(0, productId);
            return PartialView("~/Views/Shared/_ChildComment.cshtml", data);
        }

    }
}