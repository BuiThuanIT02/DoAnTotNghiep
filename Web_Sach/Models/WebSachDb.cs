using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Web_Sach.Models
{
    public partial class WebSachDb : DbContext
    {
        public WebSachDb()
            : base("name=WebSachDb")
        {
        }

        public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<DanhMucSP> DanhMucSPs { get; set; }
        public virtual DbSet<DonHang> DonHangs { get; set; }
        public virtual DbSet<FeedBack> FeedBacks { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<KhuyenMai> KhuyenMais { get; set; }
        public virtual DbSet<KhuyenMai_Sach> KhuyenMai_Sach { get; set; }
      
        public virtual DbSet<NhaXuatBan> NhaXuatBans { get; set; }
        public virtual DbSet<Sach> Saches { get; set; }
        public virtual DbSet<Silde> Sildes { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TacGia> TacGias { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<Tin_Tuc> Tin_Tuc { get; set; }
        public virtual DbSet<Voucher> Vouchers { get; set; }
        public virtual DbSet<ThamGia> ThamGias { get; set; }
        public virtual DbSet<ReView> ReViews { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        { 
            modelBuilder.Entity<DanhMucSP>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<DanhMucSP>()
                .HasMany(e => e.Saches)
                .WithOptional(e => e.DanhMucSP)
                .HasForeignKey(e => e.DanhMucID);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.TongTien)
                .HasPrecision(19, 4);

            modelBuilder.Entity<DonHang>()
                .Property(e => e.Moblie)
                .IsUnicode(false);

            modelBuilder.Entity<DonHang>()
                .HasMany(e => e.ChiTietDonHangs)
                .WithRequired(e => e.DonHang)
                .HasForeignKey(e => e.MaDonHang)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<KhuyenMai>()
                .HasMany(e => e.KhuyenMai_Sach)
                .WithRequired(e => e.KhuyenMai)
                .HasForeignKey(e => e.MaKhuyenMai)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sach>()
                .Property(e => e.Price)
                .HasPrecision(18, 3);

            modelBuilder.Entity<Sach>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Sach>()
                .HasMany(e => e.ChiTietDonHangs)
                .WithRequired(e => e.Sach)
                .HasForeignKey(e => e.MaSach)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sach>()
                .HasMany(e => e.Images)
                .WithOptional(e => e.Sach)
                .HasForeignKey(e => e.MaSP);

            modelBuilder.Entity<Sach>()
                .HasMany(e => e.KhuyenMai_Sach)
                .WithRequired(e => e.Sach)
                .HasForeignKey(e => e.MaSach)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Sach>()
              .HasMany(e => e.ReViews)
              .WithOptional(e => e.Sach)
              .HasForeignKey(e => e.MaSach);

            modelBuilder.Entity<Sach>()
             .HasMany(e => e.Comments)
             .WithOptional(e => e.Sach)
             .HasForeignKey(e => e.MaSach);

            //modelBuilder.Entity<Sach>()
            //    .HasMany(e => e.TacGias)
            //    .WithMany(e => e.Saches)
            //    .Map(m => m.ToTable("ThamGia").MapLeftKey("MaSach").MapRightKey("MaTacGia"));

            modelBuilder.Entity<Sach>()
                .HasMany(e => e.ThamGias)
                .WithRequired(e => e.Sach)
                .HasForeignKey(e => e.MaSach)
                .WillCascadeOnDelete(false);


         

            modelBuilder.Entity<TacGia>()
                .HasMany(e => e.ThamGias)
                .WithRequired(e => e.TacGia)
                .HasForeignKey(e => e.MaTacGia)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.TaiKhoan1)
                .IsUnicode(true);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.Phone)
                .IsUnicode(false);

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.MaKH);

            modelBuilder.Entity<TaiKhoan>()
              .HasMany(e => e.ReViews)
              .WithOptional(e => e.TaiKhoan)
              .HasForeignKey(e => e.MaKH);
            modelBuilder.Entity<TaiKhoan>()
            .HasMany(e => e.Comments)
            .WithOptional(e => e.TaiKhoan)
            .HasForeignKey(e => e.MaKH);

            modelBuilder.Entity<Tin_Tuc>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Voucher>()
                .HasMany(e => e.DonHangs)
                .WithOptional(e => e.Voucher)
                .HasForeignKey(e => e.MaVoucher);
        }

        //public System.Data.Entity.DbSet<Web_Sach.Models.DTO.KhuyenMaiModel> KhuyenMaiModels { get; set; }
    }
}
