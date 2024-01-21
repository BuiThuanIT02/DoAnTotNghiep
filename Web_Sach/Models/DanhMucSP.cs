namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DanhMucSP")]
    public partial class DanhMucSP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DanhMucSP()
        {
            Saches = new HashSet<Sach>();
        }

        public int ID { get; set; }
        [Display(Name="Tên danh mục")]
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
        [Display(Name = "MetaTitle")]
        [Required]
        [StringLength(250)]
        public string MetaTitle { get; set; }
        [Display(Name = "Ngày tạo")]
       
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Trạng thái")]
     
        public bool Status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Sach> Saches { get; set; }
    }
}
