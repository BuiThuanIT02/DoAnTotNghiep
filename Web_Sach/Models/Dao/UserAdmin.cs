using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.Dao
{
    public class UserAdmin
    {
        public int ID { get; set; }

        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessage ="Tên tài khoản trống")]
        public string TaiKhoan1 { get; set; }
        [Display(Name = "Số điện thoại")]
        [Required(ErrorMessage ="Số điện thoại trống")]
     
        public string Phone { get; set; }
        [Display(Name = "Email")]
        [Required(ErrorMessage ="Email trống")]
      
        public string Email { get; set; }
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage ="Mật khẩu trống")]
 
        public string Password { get; set; }
        [Display(Name = "Địa chỉ")]
        [Required(ErrorMessage ="Địa chỉ trống")]
        [StringLength(250)]
        public string Address { get; set; }
        [Display(Name = "Giới tính")]
   
      
        public string GioiTinh { get; set; }
        [Display(Name = "Ngày sinh")]
       [Required(ErrorMessage ="Ngày sinh trống")]
        public DateTime? NgaySinh { get; set; }
        [Display(Name = "Trạng thái")]

        public bool Status { get; set; }
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage ="Họ tên trống")]
     
        public string FullName { get; set; }
        [Display(Name = "Quyền")]
        public int? Role { get; set; }
    }
}