using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Areas.Admin.Data
{
    public class TacGiaAdmin
    {
        public int MaTacGia { get; set; }
        public int MaSach { get; set; }
        [Display(Name = "Tên tác giả")]
        [Required(ErrorMessage = "Mời bạn nhập tên tác giả ")]
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

        public string TenSach { get; set; }

    }
}