using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class NXB
    {
        private WebSachDb db = null;
        public NXB()
        {
            db = new WebSachDb();

        }


        //drowload
        public List<NhaXuatBan> ListAll()
        {
            return db.NhaXuatBans.ToList();
        }

        //end  drowdoad
        // Phân trang bảng nxb
        public IEnumerable<NhaXuatBan> listPage(string searchString, int page, int pageSize)
        {
            IQueryable<NhaXuatBan> query = db.NhaXuatBans;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.TenNXB.Contains(searchString));
            }
            return query.OrderBy(x => x.TenNXB).ToPagedList(page, pageSize);

        }

        // kiểm tra tính duy nhất tên nxb

        public NhaXuatBan Compare(string tk)
        {
            string trimmedTk = tk.ToLower().Replace(" ", ""); // Loại bỏ tất cả các khoảng trắng
            return db.NhaXuatBans.Where(x => x.TenNXB.ToLower().Replace(" ", "") == trimmedTk).FirstOrDefault();

        }

        // thêm mới nxb
        public int InsertNXB(NhaXuatBan tg)
        {
            db.NhaXuatBans.Add(tg);
            db.SaveChanges();
            return tg.ID;

        }

        //update
        public bool Compare(NhaXuatBan tk)
        {

            var nxb = db.NhaXuatBans.FirstOrDefault(x => x.ID != tk.ID && x.TenNXB.ToLower().Replace(" ", "") == tk.TenNXB.ToLower().Replace(" ", ""));
            if (nxb != null)
            {// đã tồn tại
                return true;
            }
            else
            {// chưa tồn tại
                return false;
            }
        }

        public bool Update(NhaXuatBan tk)
        {
            try
            {
                var author = db.NhaXuatBans.Find(tk.ID);
                author.TenNXB = tk.TenNXB;
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