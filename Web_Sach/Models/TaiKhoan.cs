namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            DonHangs = new HashSet<DonHang>();
          
            Comments = new HashSet<Comment>();
        }

        public int ID { get; set; }

        [Column("TaiKhoan")]
        [StringLength(50)]
        [Display(Name ="Tên tài khoản")]
        [Required]
        public string TaiKhoan1 { get; set; }
        [Display(Name = "Số điện thoại")]
      //  [Required]
        [StringLength(50)]
        public string Phone { get; set; }
        [Display(Name = "Email")]
        [Required]
        [StringLength(150)]
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
       // [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Display(Name = "Địa chỉ")]
      //  [Required]
        [StringLength(250)]
        public string Address { get; set; }
        [Display(Name = "Giới tính")]
       // [Required]
        [StringLength(3)]
        public string GioiTinh { get; set; }
        [Display(Name = "Ngày sinh")]
     //   [Required]
        public DateTime? NgaySinh { get; set; }
        [Display(Name = "Trạng thái")]
       
        public bool Status { get; set; }
        [Display(Name = "Họ và tên")]
        [Required]
        [StringLength(50)]
        public string FullName { get; set; }
        [Display(Name = "Quyền")]
       
        public int? Role { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonHang> DonHangs { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

    }
}
