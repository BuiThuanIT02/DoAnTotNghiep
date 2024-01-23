using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class thamGiaModels
    {

        private WebSachDb db = null;
        public thamGiaModels()
        {
            db = new WebSachDb();

        }

        public IEnumerable<ThamGia> listPage(int page, int pageSize)
        {
            IQueryable<ThamGia> model = db.ThamGias;

            return model.OrderBy(x => x.MaSach).ToPagedList(page, pageSize);
        }

        public bool Insert(ThamGia tg)
        {
            try
            {
                db.ThamGias.Add(tg);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public ThamGia Edit(int maSach, int maTacGia)
        {
            return db.ThamGias.Where(t => t.MaTacGia == maTacGia && t.MaSach == maSach).FirstOrDefault();
        }


        public bool Update(int maSach, int maTacGia, ThamGia dm)
        {
            try
            {

                var km = db.ThamGias.Where(t => t.MaTacGia == maTacGia && t.MaSach == maSach).FirstOrDefault();
                if (km != null)
                {
                    km.MaSach = dm.MaSach;
                    km.MaTacGia = dm.MaTacGia;

                    db.SaveChanges();
                   

                } 
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            


        }
        public bool Delete(int idS,int idT)
        {
            try
            {
                var km = db.ThamGias.Where(x=>x.MaTacGia==idT && x.MaSach == idS).FirstOrDefault();
                db.ThamGias.Remove(km);
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