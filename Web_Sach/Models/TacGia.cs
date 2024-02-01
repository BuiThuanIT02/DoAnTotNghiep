namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TacGia")]
    public partial class TacGia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TacGia()
        {
            //Saches = new HashSet<Sach>();
            ThamGias = new HashSet<ThamGia>();
        }

        public int ID { get; set; }
        [Display(Name ="Tên tác giả")]
        [Required(ErrorMessage = "Mời bạn nhập tên tác giả ")]
        [StringLength(50)]
        public string TenTacGia { get; set; }
        [Display(Name = "Tiểu sử")]
        [Required(ErrorMessage = "Mời bạn tiểu sử ")]
        public string TieuSu { get; set; }
        [StringLength(250)]
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage = "Mời bạn nhập địa chỉ ")]
        public string Address { get; set; }
        [StringLength(13)]
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage = "Mời bạn nhập SDT tác giả")]
        public string Phone { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        //public virtual ICollection<Sach> Saches { get; set; }
        public virtual ICollection<ThamGia> ThamGias { get; set; }
    }
}
