namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Tin_Tuc
    {
        public int ID { get; set; }

        [StringLength(250)]
        [Display(Name="Tên tin tức")]
        [Required(ErrorMessage ="Tên tin tức không để trống")]
        public string Name { get; set; }

        [StringLength(250)]
        [Display(Name = "MetaTitle")]
        [Required(ErrorMessage = "MetaTitle không để trống")]
        public string MetaTitle { get; set; }
        [Display(Name = "Mô tả")]
        [Required(ErrorMessage = "Mô tả không để trống")]
        public string Description { get; set; }

        [StringLength(250)]
        [Display(Name = "Hình ảnh")]
        [Required(ErrorMessage = "Hình ảnh không để trống")]
        public string Image { get; set; }
        [Display(Name = "Ngày tạo")]
        
        public DateTime? CreatedDate { get; set; }
    }
}
