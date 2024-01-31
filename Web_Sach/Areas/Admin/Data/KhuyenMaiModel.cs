using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace Web_Sach.Areas.Admin.Data
{
    public class KhuyenMaiModel
    {
        public int MaKM { get; set; }
        public int MaSach { get; set; }
        [Display(Name = "Tên khuyến mại")]
        [Required(ErrorMessage = "Tên khuyến mại trống!")]
        public string TenKhuyenMai { get; set; }
        public string TenSach { get; set; }
        [Display(Name = "Ngày bắt đầu")]
        [Required(ErrorMessage = "Ngày bắt đầu trống!")]
        public DateTime? NgayBatDau { get; set; }
        [Display(Name = "Ngày kết thúc")]
        [Required(ErrorMessage = "Ngày kết thúc trống!")]
        public DateTime? NgayKeThuc { get; set; }
        [Display(Name = "Sale")]
        [Required(ErrorMessage = "Sale trống!")]
        public int? Sale { get; set; }

      

   
    }
}