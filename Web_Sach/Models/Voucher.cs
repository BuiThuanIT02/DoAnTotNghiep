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

        public double? DonGiaToiThieu { get; set; }

        public DateTime? NgayTao { get; set; }

        public DateTime? NgayHetHan { get; set; }

        [StringLength(150)]
        public string MaVoucher { get; set; }

        public double? SoTienGiam { get; set; }

        public int? SoLanSuDung { get; set; }

        public int? SoLanDaSuDung { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
    }
}
