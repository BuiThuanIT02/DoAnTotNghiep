using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.EF
{
    public class CommentViewModel
    {
        public int ID { get; set; }

      
        public string Content { get; set; }

        public int? Rate { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? MaSach { get; set; }

        public int? MaKH { get; set; }
        public string FullName { get; set; }
        public int? parentId { get; set; }
    }
}