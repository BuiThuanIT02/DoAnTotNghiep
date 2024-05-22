namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhaXuatBan")]
    public partial class NhaXuatBan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaXuatBan()
        {
            Saches = new HashSet<Sach>();
        }

        public int ID { get; set; }
        [Display(Name ="Tên nhà xuất bản")]
        [Required]
        [StringLength(50)]
        public string TenNXB { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required]
        [StringLength(50)]
        public string SDT { get; set; }

        [Display(Name = "Địa chỉ")]
       [Required]
        [StringLength(250)]
        public string DiaChi { get; set; }

        [Display(Name = "Email")]
        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sach> Saches { get; set; }
    }
}
