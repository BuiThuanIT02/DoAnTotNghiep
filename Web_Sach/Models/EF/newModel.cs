using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class newModel
    {
        private WebSachDb db = null;

        public newModel()
        {
            db = new WebSachDb();
        }
        // Phân trang bảng news
        public IEnumerable<Tin_Tuc> listPage(string searchString, int page, int pageSize)
        {
            IQueryable<Tin_Tuc> query = db.Tin_Tuc;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString));
            }
            return query.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

        }

        public bool Unique(string name)
        {
            var item = db.Tin_Tuc.Where(u => u.Name.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", "")).FirstOrDefault();
            if (item != null)
            {//đã tồn tại
                return true;
            }
            else
            {
                return false;
            }
        }
        public int InserNew(Tin_Tuc news)
        {
            news.CreatedDate = DateTime.Now;
            db.Tin_Tuc.Add(news);
            db.SaveChanges();
            return news.ID;
        }

        public Tin_Tuc EditNew(int id)
        {
            return db.Tin_Tuc.Find(id);
        }
        public bool Compare(Tin_Tuc sl)
        {
            var news = db.Tin_Tuc.FirstOrDefault(x => x.ID != sl.ID && x.Name.ToLower().Replace(" ", "") == sl.Name.ToLower().Replace(" ", ""));
            if (news != null)
            {// đã tồn tại
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdateNew(Tin_Tuc sl)
        {
            try
            {
                var tinTuc = db.Tin_Tuc.Find(sl.ID);
                tinTuc.Name = sl.Name;
                tinTuc.MetaTitle = sl.MetaTitle;
                tinTuc.Image = sl.Image;
                tinTuc.Description = sl.Description;
                tinTuc.CreatedDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //xóa
        public bool Delete(int id)
        {
            try
            {
                var news = db.Tin_Tuc.Find(id);
                db.Tin_Tuc.Remove(news);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


    }
}