using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class Supplier
    {
        private WebSachDb db = null;
        public Supplier()
        {
            db = new WebSachDb();

        }

        public List<NhaCungCap> ListAll()
        {
            return db.NhaCungCaps.ToList();
        }
        // Phân trang bảng ncc
        public IEnumerable<NhaCungCap> listPage(string searchString, int page, int pageSize)
        {
            IQueryable<NhaCungCap> query = db.NhaCungCaps;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.Name.Contains(searchString));
            }
            return query.OrderBy(x => x.Name).ToPagedList(page, pageSize);

        }

        // kiểm tra tính duy nhất tên ncc

        public NhaCungCap Compare(string tk)
        {
            string trimmedTk = tk.ToLower().Replace(" ", ""); // Loại bỏ tất cả các khoảng trắng
            return db.NhaCungCaps.Where(x => x.Name.ToLower().Replace(" ", "") == trimmedTk).FirstOrDefault();

        }
        // thêm mới ncc
        public int InsertNCC(NhaCungCap tg)
        {
            db.NhaCungCaps.Add(tg);
            db.SaveChanges();
            return tg.ID;

        }

        //update
        public bool Compare(NhaCungCap tk)
        {

            var ncc = db.NhaCungCaps.FirstOrDefault(x => x.ID != tk.ID && x.Name.ToLower().Replace(" ", "") == tk.Name.ToLower().Replace(" ", ""));
            if (ncc != null)
            {// đã tồn tại
                return true;
            }
            else
            {// chưa tồn tại
                return false;
            }
        }

        public bool Update(NhaCungCap tk)
        {
            try
            {
                var author = db.NhaCungCaps.Find(tk.ID);
                author.Name = tk.Name;
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