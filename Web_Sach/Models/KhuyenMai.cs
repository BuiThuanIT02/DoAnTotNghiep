namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhuyenMai")]
    public partial class KhuyenMai
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhuyenMai()
        {
            KhuyenMai_Sach = new HashSet<KhuyenMai_Sach>();
        }

        public int ID { get; set; }
        [Display(Name ="Tên khuyến mại")]
        [Required]
        [StringLength(250)]
        public string TenKhuyenMai { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [Required]
        public DateTime? NgayBatDau { get; set; }
        [Display(Name = "Ngày kết thúc")]
        [Required]
        public DateTime? NgayKeThuc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KhuyenMai_Sach> KhuyenMai_Sach { get; set; }
    }
}
