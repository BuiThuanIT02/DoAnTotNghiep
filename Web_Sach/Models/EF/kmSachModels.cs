using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class kmSachModels
    {
        private WebSachDb db = null;
        public kmSachModels()
        {
            db = new WebSachDb();

        }

        public IEnumerable<KhuyenMai_Sach> listPage(int page, int pageSize)
        {
            IQueryable<KhuyenMai_Sach> model = db.KhuyenMai_Sach;

            return model.OrderBy(x => x.MaSach).ToPagedList(page, pageSize);
        }
        public List<KhuyenMai> ListAll()
        {
            return db.KhuyenMais.ToList();
        }

        public bool Insert(KhuyenMai_Sach tg)
        {
            try
            {
                db.KhuyenMai_Sach.Add(tg);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public KhuyenMai_Sach Edit(int maSach, int maKM)
        {
            return db.KhuyenMai_Sach.Where(t => t.MaKhuyenMai == maKM && t.MaSach == maSach).FirstOrDefault();
        }


        public bool Update(int maSach, int maKM, KhuyenMai_Sach dm)
        {
            try
            {
                var km = db.KhuyenMai_Sach.Where(t => t.MaKhuyenMai == maKM && t.MaSach == maSach).FirstOrDefault();
                if (km != null)
                {
                    // Mới chưa tồn tại, cũ đã tồn tại
                    if (km.MaSach != dm.MaSach || km.MaKhuyenMai != dm.MaKhuyenMai)
                    {
                        // Tạo một đối tượng mới với giá trị mới
                        var newKm_Sach = new KhuyenMai_Sach
                        {
                            MaSach = dm.MaSach,
                            MaKhuyenMai = dm.MaKhuyenMai,
                            Sale = dm.Sale,
                            // Các thuộc tính khác nếu có
                        };
                        // Xóa đối tượng cũ
                        db.KhuyenMai_Sach.Remove(km);
                        // Thêm đối tượng mới vào cơ sở dữ liệu
                        db.KhuyenMai_Sach.Add(newKm_Sach);



                        // Lưu thay đổi vào cơ sở dữ liệu
                        db.SaveChanges();

                        return true;
                    }
                    else
                    {// sách mới  == sách cũ
                        if (km.Sale != dm.Sale)
                        {
                            km.Sale = dm.Sale;
                            db.SaveChanges();
                            return true;
                        }
                        return true;
                    }
                }


            }
            catch (Exception)
            {
                return false;
            }

            return false;


        }

        public bool Delete(int idS, int idT)
        {
            try
            {
                var km = db.KhuyenMai_Sach.Where(x => x.MaKhuyenMai == idT && x.MaSach == idS).FirstOrDefault();
                db.KhuyenMai_Sach.Remove(km);
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