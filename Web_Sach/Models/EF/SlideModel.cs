using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class SlideModel
    {
        private WebSachDb db= null;

        public SlideModel()
        {
            db = new WebSachDb();
        }


        public List<Silde> getSilde()
        {
            return db.Sildes.Where(x => x.Status == true).ToList();

        }

        // Phân trang bảng Silde
        public IEnumerable<Silde> listPage(int page, int pageSize)
        {
            return db.Sildes.OrderByDescending(x => x.CreatedDate).ToPagedList(page, pageSize);

        }
        // ktra trùng
        public bool Unique(string url)
        {
            var item = db.Sildes.Where(u => u.Image.ToLower().Replace(" ", "") == url.ToLower().Replace(" ", "")).FirstOrDefault();
            if(item != null)
            {//đã tồn tại
                return true;
            }
            else
            {
                return false;
            }
        }
        // thêm
        public int InserSilde(Silde silde)
        {
            silde.CreatedDate = DateTime.Now;
            db.Sildes.Add(silde);
            db.SaveChanges();
            return silde.ID;
        }

        public Silde EditSilde(int id)
        {
            return db.Sildes.Find(id);
        }
        // check update
        public bool Compare(Silde sl)
        {
            var silde = db.Sildes.FirstOrDefault(x => x.ID != sl.ID && x.Image.ToLower().Replace(" ", "") == sl.Image.ToLower().Replace(" ", ""));
            if (silde != null)
            {// đã tồn tại
                return true;
            }
            else
            {
                return false;
            }
        }
        //update 
        public bool UpdateSilde(Silde sl)
        {
            try
            {
                var silde = db.Sildes.Find(sl.ID);
                silde.Image = silde.Image;
                silde.CreatedDate = DateTime.Now;
                silde.Status = sl.Status;
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
                var silde = db.Sildes.Find(id);
                db.Sildes.Remove(silde);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //thay đổi trạng thái user
        public bool ChangeStatus(int id)
        {

            var slideChange = db.Sildes.Find(id);
            slideChange.Status = !slideChange.Status;
            db.SaveChanges();
            return slideChange.Status;

        }

    }
}