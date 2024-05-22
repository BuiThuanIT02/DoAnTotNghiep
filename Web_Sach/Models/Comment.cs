using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Models
{
    [Table("Comment")]
    public partial class Comment
    {
        public int ID { get; set; }

        [StringLength(250)]
        public string Content { get; set; }

        public int? Rate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? MaSach { get; set; }

        public int? MaKH { get; set; }
        public int? parentId { get; set; }
        public int? Status { get; set; }

        public virtual Sach Sach { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }
    }
}