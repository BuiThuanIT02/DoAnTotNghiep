using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Sach.Areas.Admin.Data;

namespace Web_Sach.Models.EF
{
    public class TacGiaModels
    {
        private WebSachDb db = null;

        public TacGiaModels()
        {
            db = new WebSachDb();
        }

        public bool WriteBook(ThamGia tg)
        {
            try
            {
                db.ThamGias.Add(tg);
                db.SaveChanges(); 
                return true;

            }
            catch
            {
                return false;
            }
        }
        public List<TacGia> ListAll()
        {
            return db.TacGias.ToList();
        }


        // Phân trang bảng tác giả
        public IEnumerable<TacGiaAdmin> listPage(string searchString, int page, int pageSize)
        {
            var model = from tg in db.TacGias
                        join tt in db.ThamGias on tg.ID equals tt.MaTacGia
                        join s in db.Saches on tt.MaSach equals s.ID
                        select new TacGiaAdmin
                        {
                            MaTacGia = tg.ID,
                            MaSach = s.ID,
                            TenTacGia = tg.TenTacGia,
                            Address = tg.Address,
                            Phone = tg.Phone,
                            TieuSu = tg.TieuSu,
                            TenSach = s.Name,
                        };

            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.TenTacGia.Contains(searchString));
            }


            return model.OrderBy(x => x.TenTacGia).ToPagedList(page, pageSize);

        }
        // End  Phân trang bảng tác giả

        // kiểm tra tính duy nhất tên tác giả

        public TacGia Compare(string tk)
        {
            string trimmedTk = tk.ToLower().Replace(" ", ""); // Loại bỏ tất cả các khoảng trắng
            return db.TacGias.Where(x => x.TenTacGia.ToLower().Replace(" ", "") == trimmedTk).FirstOrDefault();

        }

        // thêm mới tác giả
        public int InsertAuthor(TacGia tg)
        {
            db.TacGias.Add(tg);
            db.SaveChanges();
            return tg.ID;

        }
        //update
        public bool Compare(TacGia tk)
        {

            var user = db.TacGias.FirstOrDefault(x => x.ID != tk.ID && x.TenTacGia.ToLower().Replace(" ", "") == tk.TenTacGia.ToLower().Replace(" ", ""));
            if (user != null)
            {// đã tồn tại
                return true;
            }
            else
            {// chưa tồn tại
                return false;
            }
        }

        public bool Update(TacGia tk)
        {
            try
            {
                var author = db.TacGias.Find(tk.ID);
                author.TenTacGia = tk.TenTacGia;
                author.TieuSu = tk.TieuSu;

                db.SaveChanges();


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public TacGiaAdmin Edit(int maSach, int maTacGia)
        {
            var model = from tg in db.TacGias
                        join tt in db.ThamGias on tg.ID equals tt.MaTacGia
                        join s in db.Saches on tt.MaSach equals s.ID
                        where tg.ID == maTacGia && s.ID == maSach
                        select new TacGiaAdmin
                        {
                            MaTacGia = tg.ID,
                            MaSach = s.ID,
                            TenTacGia = tg.TenTacGia,
                            Address = tg.Address,
                            Phone = tg.Phone,
                            TieuSu = tg.TieuSu,
                        };

            return model.FirstOrDefault();
        }
        public bool Delete( int matg)
        {
            try
            {
              
                var kmS = db.ThamGias.Where(x=> x.MaTacGia == matg);
                if (kmS.Any())
                {
                    db.ThamGias.RemoveRange(kmS);
                }
                var km = db.TacGias.Find(matg);
                if (km != null)
                {
                    db.TacGias.Remove(km);
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