using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Web_Sach.Models.EF;

namespace Web_Sach.Models.Dao
{
    public class CommentDao
    {
        WebSachDb db = null;
        public CommentDao()
        {
            db = new WebSachDb();
        }
        public bool Insert(Comment entity)
        {
            db.Comments.Add(entity);
            db.SaveChanges();
            return true;
        }
        public List<Comment> ListComment(int parentId, int productId)
        {
            return db.Comments.Where(x=>x.parentId == parentId && x.MaSach == productId).ToList();
        }

        public List<CommentViewModel> ListCommentViewModel(int parentId, int productId)
        {
            var model = (from a in db.Comments
                         join b in db.TaiKhoans on a.MaKH equals b.ID
                         where a.parentId == parentId && a.MaSach == productId

                         select new
                         {
                             IDs = a.ID,
                             Contents = a.Content,
                             CreatedDates = a.CreatedDate,
                             MaSachs = a.MaSach,
                             MaKHs = a.MaKH,
                             FullNames = b.FullName,
                             parentIds = a.parentId,
                             Rates = a.Rate,
                         }).AsEnumerable().Select(x => new CommentViewModel
                         {
                             ID= x.IDs,
                             Content = x.Contents,
                             MaSach= x.MaSachs,
                             MaKH = x.MaKHs,
                             FullName= x.FullNames,
                             CreatedDate= x.CreatedDates,
                             parentId=x.parentIds,
                             Rate= x.Rates,
                         });

            return model.OrderByDescending(y => y.ID).ToList();   
                      
        }
    }
}