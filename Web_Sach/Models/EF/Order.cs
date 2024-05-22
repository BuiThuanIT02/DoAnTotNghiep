using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class Order
    {
        private WebSachDb db = null;
        public string TenKH { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string mavoucher { get; set; }
        public int total { get; set; }
        public string TypePayment { get; set; }
        public string TypePaymentVN { get; set; }
        public Order()
        {
            db = new WebSachDb();
        }

        // phan tran order
        public IEnumerable<DonHang> listPage(int page, int pageSize)
        {
            return db.DonHangs.OrderBy(x => x.NgayDat).Where(x => x.Status == 1).ToPagedList(page, pageSize);


        }

        // phan tran order
        public IEnumerable<DonHang> listPagePackAd(int page, int pageSize)
        {
            return db.DonHangs.OrderBy(x => x.NgayDat).Where(x => x.Status == 2).ToPagedList(page, pageSize);


        }
        // phan tran order
        public IEnumerable<DonHang> listPageTransportAd(int page, int pageSize)
        {
            return db.DonHangs.OrderBy(x => x.NgayDat).Where(x => x.Status == 3).ToPagedList(page, pageSize);


        }

        // phan tran order
        public IEnumerable<DonHang> listPageSuccessAd(int page, int pageSize)
        {
            return db.DonHangs.OrderBy(x => x.NgayDat).Where(x => x.Status == 4).ToPagedList(page, pageSize);


        }

        // phan tran order
        public IEnumerable<DonHang> listPageFailureAd(int page, int pageSize)
        {
            return db.DonHangs.OrderBy(x => x.NgayDat).Where(x => x.Status == 5).ToPagedList(page, pageSize);


        }










    }
}