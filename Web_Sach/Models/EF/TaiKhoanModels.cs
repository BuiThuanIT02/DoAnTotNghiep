using Microsoft.AspNet.Identity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_Sach.Models.Dao;

namespace Web_Sach.Models.EF
{
    public class TaiKhoanModels
    {
        private WebSachDb db = null;

        public TaiKhoanModels()
        {
            db = new WebSachDb();
        }


        // Tìm tk  trùng tên userName
        public TaiKhoan GetUserName(string userName)
        {
            return db.TaiKhoans.FirstOrDefault(x => x.TaiKhoan1 == userName);

        }
        // End Tìm tk  trùng tên userName


        // Phân trang bảng Tài Khoản
        public IEnumerable<TaiKhoan> listPage(string searchString, int page, int pageSize)
        {
            IQueryable<TaiKhoan> query = db.TaiKhoans;
            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(x => x.TaiKhoan1.Contains(searchString));
            }
            return query.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);

        }
        // End  Phân trang bảng Tài Khoản



        //Thêm mới user
        public int InserUser(TaiKhoan user)
        {
            db.TaiKhoans.Add(user);
            db.SaveChanges();
            return user.ID;

        }
        public int InserForFaceBook(TaiKhoan user)
        {
            db.TaiKhoans.Add(user);
                db.SaveChanges();
                return user.ID;
            //var entity = db.TaiKhoans.SingleOrDefault(x => x.TaiKhoan1 == user.TaiKhoan1);
            //if (entity == null)
            //{
            //    db.TaiKhoans.Add(user);

            //    try
            //    {  db.SaveChanges();
            //        return user.ID;
            //    }
            //    catch (Exception ex)
            //    {

            //        Console.WriteLine(ex);
            //    }
            //    return user.ID;
            //}
            //else
            //{
            //    return entity.ID;
            //}


        }

        //End Thêm mới user


        //tìm theo tên tài khoản
        public TaiKhoan UniqueUserName(string userName)
        {
            return db.TaiKhoans.FirstOrDefault(x => x.TaiKhoan1.ToLower().Replace(" ", "") == userName.ToLower().Replace(" ", ""));

        }
        // end tìm theo tên tài khoản


        // tìm theo id user
        public UserAdmin EditUser(int id)
        {
            var user = db.TaiKhoans.Find(id);
            var userModel = new UserAdmin()
            {
                TaiKhoan1 = user.TaiKhoan1,
                Password = user.Password,
                Address = user.Address,
                Phone = user.Phone,
                Email = user.Email,
                GioiTinh = user.GioiTinh,
                NgaySinh = user.NgaySinh,
                Status = user.Status,
                FullName = user.FullName,
                Role = user.Role,


            };
            return userModel;
        }
        // end tìm id user
        //  tm khi update không cho trùng tên
        public bool Compare(TaiKhoan tk)
        {
            var user = db.TaiKhoans.FirstOrDefault(x => x.ID != tk.ID && x.TaiKhoan1.ToLower().Replace(" ", "") == tk.TaiKhoan1.ToLower().Replace(" ", ""));
            if (user != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //ten không trùng

        //update khi sửa
        public bool UpdateUser(TaiKhoan tk)
        {
            try
            {
                var user = db.TaiKhoans.Find(tk.ID);

                user.TaiKhoan1 = tk.TaiKhoan1;
                user.Password = tk.Password;
                user.GioiTinh = tk.GioiTinh;
                user.Phone = tk.Phone;
                user.Address = tk.Address;
                user.FullName = tk.FullName;
                user.Email = tk.Email;
                user.NgaySinh = tk.NgaySinh;
                user.Status = tk.Status;
                user.Role = 0;
                db.SaveChanges();
                return true;
            }

            catch (Exception)
            {
                return false;
            }


        }
        //end update khi sửa


        // xóa 1 bản ghi user
        public bool Delete(int id)
        {
            try
            {
                var userDelete = db.TaiKhoans.Find(id);
                if (userDelete != null)
                {
                    var commets = userDelete.Comments.Where(x => x.MaKH == id);
                    var order = userDelete.DonHangs.Where(x => x.MaKH == id);
                    foreach (var item in order)
                    {
                        var orderDetail = item.ChiTietDonHangs.Where(x => x.MaDonHang == item.ID);
                        db.ChiTietDonHangs.RemoveRange(orderDetail);
                    }
                    if (commets.Any())
                    {
                        db.Comments.RemoveRange(commets);
                    }
                    if (order.Any())
                    {
                        db.DonHangs.RemoveRange(order);
                    }
                    db.TaiKhoans.Remove(userDelete);
                    db.SaveChanges();
                    return true;
                }
                return false;


            }
            catch (Exception)
            {
                return false;
            }
        }

        // end 1 bảng ghi user



        //thay đổi trạng thái user
        public bool ChangeStatus(int id)
        {

            var userChange = db.TaiKhoans.Find(id);
            userChange.Status = !userChange.Status;
            db.SaveChanges();
            return userChange.Status;

        }

        // End thay đổi trạng thái user

















    }
}