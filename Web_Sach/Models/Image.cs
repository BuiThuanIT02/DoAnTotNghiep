namespace Web_Sach.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Image
    {
        public int ID { get; set; }

        public int? MaSP { get; set; }

        [Column("Image")]
        [StringLength(250)]
        public string Image1 { get; set; }

        public bool IsDefault { get; set; }

        public virtual Sach Sach { get; set; }
    }
}
