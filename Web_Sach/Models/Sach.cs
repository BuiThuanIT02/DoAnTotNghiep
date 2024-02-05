namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Sach")]
    public partial class Sach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Sach()
        {
            ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            Images = new HashSet<Image>();
            KhuyenMai_Sach = new HashSet<KhuyenMai_Sach>();
            //TacGias = new HashSet<TacGia>();
            ThamGias = new HashSet<ThamGia>();
            ReViews = new HashSet<ReView>();
        }
        [Display(Name="Mã sách")]
       
        public int ID { get; set; }
        [Display(Name = "Danh mục")]
       
        public int? DanhMucID { get; set; }
        //[Display(Name = "Nhà cung cấp")]
      
        //public int? NhaCungCapID { get; set; }
        [Display(Name = "Mã xuất bản")]
      
        public int? NhaXuatBanID { get; set; }
        [Display(Name = "Tên sách")]
        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Display(Name = "Giá nhập")]
        [Required]
        public decimal? GiaNhap { get; set; }
        [Display(Name = "Giá sách bán")]
        [Required]
        public decimal? Price { get; set; }
        [Display(Name = "Số lượng")]
        [Required]
        public int? Quantity { get; set; }
        [Display(Name = "Mô tả")]
        [Required]
        public string MoTa { get; set; }
        [Display(Name = "Kích thước")]
        [Required]
        [StringLength(50)]
        public string KichThuoc { get; set; }
        [Display(Name = "Trọng lượng")]
        [Required]
        [StringLength(10)]
        public string TrongLuong { get; set; }
        [Display(Name = "Số trang")]
        [Required]
        public int? SoTrang { get; set; }
        [Display(Name = "Ngày cập nhật")]
       
        [Required]
        public DateTime? NgayCapNhat { get; set; }
        [Display(Name = "MetaTitle")]
        [Required]
        [StringLength(250)]
        public string MetaTitle { get; set; }
        [Display(Name = "Trạng thái")]
        [Required]
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }

        public virtual DanhMucSP DanhMucSP { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Image> Images { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhuyenMai_Sach> KhuyenMai_Sach { get; set; }

        //public virtual NhaCungCap NhaCungCap { get; set; }

        public virtual NhaXuatBan NhaXuatBan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<TacGia> TacGias { get; set; }
        public virtual ICollection<ThamGia> ThamGias { get; set; }

        public virtual ICollection<ReView> ReViews { get; set; }
    }
}
