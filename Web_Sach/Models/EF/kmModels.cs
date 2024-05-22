using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Web_Sach.Areas.Admin.Data;


namespace Web_Sach.Models.EF
{
    public class kmModels
    {

        private WebSachDb db = null;
        public kmModels()
        {
            db = new WebSachDb();

        }
        public List<KhuyenMai> ListAll()
        {
            return db.KhuyenMais.ToList();
        }
        public bool SaleMany(KhuyenMai_Sach tg)
        {
            try
            {
                db.KhuyenMai_Sach.Add(tg);
                db.SaveChanges();
                return true;

            }
            catch
            {
                return false;
            }
        }
        public IEnumerable<KhuyenMaiModel> listPage(string searchString, int page, int pageSize)
        {
            //IQueryable<KhuyenMai> model = db.KhuyenMais;
            var model = from k in db.KhuyenMais
                        join kms in db.KhuyenMai_Sach on k.ID equals kms.MaKhuyenMai
                        join s in db.Saches on kms.MaSach equals s.ID
                        select new KhuyenMaiModel
                        {
                            MaKM = k.ID,
                            MaSach = s.ID,
                            MaDM = s.DanhMucSP.ID,
                            TenSach = s.Name,
                            TenKhuyenMai = k.TenKhuyenMai,
                            NgayBatDau = k.NgayBatDau,
                            NgayKeThuc = k.NgayKeThuc,
                            Sale = kms.Sale,
                        };

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TenKhuyenMai.Contains(searchString));
            }


            return model.OrderByDescending(x => x.MaKM).ToPagedList(page, pageSize);
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

        public KhuyenMaiModel Edit(int maDM, int maKM)
        {
            var sach = db.Saches.Where(x => x.DanhMucID == maDM).AsEnumerable().Select(x => x.ID);
            var listId = sach.FirstOrDefault();
            var km = from k in db.KhuyenMais
                     join kms in db.KhuyenMai_Sach on k.ID equals kms.MaKhuyenMai
                     join s in db.Saches on kms.MaSach equals s.ID
                     where s.ID == listId && k.ID == maKM /*s.ID == maSach && k.ID == maKM*/
                     select new KhuyenMaiModel
                     {
                         MaKM = k.ID,
                         MaSach = s.ID,
                         MaDM = s.DanhMucSP.ID,
                         TenKhuyenMai = k.TenKhuyenMai,
                         NgayBatDau = k.NgayBatDau,
                         NgayKeThuc = k.NgayKeThuc,
                         Sale = kms.Sale,
                     };

            return km.FirstOrDefault();
        }

        public bool Compare(int id,string dm)
        {
            var km = db.KhuyenMais.FirstOrDefault(x => x.ID != id && x.TenKhuyenMai.ToLower().Replace(" ", "") == dm.ToLower().Replace(" ", ""));
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

        public bool Delete(int makm)
        {
            try
            {
                var kmS = db.KhuyenMai_Sach.Where(x => x.MaKhuyenMai == makm);
                if (kmS.Any())
                {
                    db.KhuyenMai_Sach.RemoveRange(kmS);
                }
                var km = db.KhuyenMais.Find(makm);
                if (km != null)
                {
                    db.KhuyenMais.Remove(km);
                }

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