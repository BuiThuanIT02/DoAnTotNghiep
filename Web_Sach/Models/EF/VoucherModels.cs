using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class VoucherModels
    {
        private WebSachDb db = null;
        public VoucherModels()
        {
            db = new WebSachDb();

        }

        public IEnumerable<Voucher> listPage(string searchString, int page, int pageSize)
        {
            IQueryable<Voucher> model = db.Vouchers;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.MaVoucher.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        // tìm tên danh mục để tạo danh mục xem có trùng tên không
        public Voucher uniqueName(string name)
        {
            return db.Vouchers.FirstOrDefault(x => x.MaVoucher.ToLower().Replace(" ", "") == name.ToLower().Replace(" ", ""));
        }
        // thêm mới danh mục
        public int Insert(Voucher dm)
        {
            dm.SoLanDaSuDung = 0;
            db.Vouchers.Add(dm);
            db.SaveChanges();
            return dm.ID;
        }
        public Voucher Edit(int id)
        {
            return db.Vouchers.Find(id);
        }

        public bool Compare(Voucher dm)
        {
            var voucher = db.Vouchers.FirstOrDefault(x => x.ID != dm.ID && x.MaVoucher.ToLower().Replace(" ", "") == dm.MaVoucher.ToLower().Replace(" ", ""));
            if (voucher != null)
            {// đã tồn tại
                return true;
            }
            else
            {// chưa  tồn tại
                return false;
            }

        }

        public bool Update(Voucher dm)
        {
            try
            {
                var voucher = db.Vouchers.Find(dm.ID);
                voucher.MaVoucher = dm.MaVoucher;
                voucher.NgayTao = dm.NgayTao;
                voucher.NgayHetHan = dm.NgayHetHan;
                voucher.DonGiaToiThieu = dm.DonGiaToiThieu;
                //voucher.SoLanDaSuDung = dm.SoLanDaSuDung;
                voucher.SoLanSuDung = dm.SoLanSuDung;
                voucher.SoTienGiam = dm.SoTienGiam;
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
                var voucher = db.Vouchers.Find(id);
                if (voucher != null)
                {
                    var order = db.DonHangs.Where(x => x.MaVoucher == id);
                    if (order.Any())
                    {
                        foreach (var item in order)
                        {
                            var detail = db.ChiTietDonHangs.Where(i => i.MaDonHang == item.ID);
                            if (detail.Any())
                            {
                                db.ChiTietDonHangs.RemoveRange(detail);
                            }
                        }
                        db.DonHangs.RemoveRange(order);
                    }


                    db.Vouchers.Remove(voucher);
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



    }
}