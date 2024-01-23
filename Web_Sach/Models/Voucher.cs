namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Voucher")]
    public partial class Voucher
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Voucher()
        {
            DonHangs = new HashSet<DonHang>();
        }

        public int ID { get; set; }
        [Display(Name = "Đơn giá tối thiểu")]
        [Required]
        public double? DonGiaToiThieu { get; set; }
        [Display(Name = "Ngày tạo")]
        [Required]
        public DateTime? NgayTao { get; set; }
        [Display(Name = "Ngày hết hạn")]
        [Required]
        public DateTime? NgayHetHan { get; set; }
        [Display(Name = "Mã voucher")]
        [Required]
        [StringLength(150)]
        public string MaVoucher { get; set; }
        [Display(Name = "Số tiền giảm")]
        [Required]
        public double? SoTienGiam { get; set; }
        [Display(Name = "Số lần sử dụng")]
        [Required]
        public int? SoLanSuDung { get; set; }
        [Display(Name = "Số lần đã sử dụng")]
        
        public int? SoLanDaSuDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
