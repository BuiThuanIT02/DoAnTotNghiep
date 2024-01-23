using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class kmModels
    {

        private WebSachDb db = null;
        public kmModels()
        {
            db = new WebSachDb();

        }

        public IEnumerable<KhuyenMai> listPage(string searchString, int page, int pageSize)
        {
            IQueryable<KhuyenMai> model = db.KhuyenMais;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TenKhuyenMai.Contains(searchString));
            }
            return model.OrderBy(x => x.TenKhuyenMai).ToPagedList(page, pageSize);
        }

        public KhuyenMai uniqueName(string name)
        {
            return db.KhuyenMais.FirstOrDefault(x => x.TenKhuyenMai.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", ""));
        }

        public int Insert(KhuyenMai km)
        {         
            db.KhuyenMais.Add(km);
            db.SaveChanges();
            return km.ID;
        }

        public KhuyenMai Edit(int id)
        {
            return db.KhuyenMais.Find(id);
        }

        public bool Compare(KhuyenMai dm)
        {
            var km = db.KhuyenMais.FirstOrDefault(x => x.ID != dm.ID && x.TenKhuyenMai.ToLower().Replace(" ", "") == dm.TenKhuyenMai.ToLower().Replace(" ", ""));
            if (km != null)
            {// đã tồn tại
                return true;
            }
            else
            {// chưa  tồn tại
                return false;
            }

        }


        public bool Update(KhuyenMai dm)
        {
            try
            {
                var km = db.KhuyenMais.Find(dm.ID);
                km.TenKhuyenMai = dm.TenKhuyenMai;
                km.NgayBatDau = dm.NgayBatDau;
                km.NgayKeThuc = dm.NgayKeThuc;
               
                db.SaveChanges();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var km = db.KhuyenMais.Find(id);
                db.KhuyenMais.Remove(km);
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