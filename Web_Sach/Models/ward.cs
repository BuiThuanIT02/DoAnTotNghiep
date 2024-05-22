using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Models
{
    public partial class ward
    {
        [Key]
        [StringLength(20)]
        public string code { get; set; }
        [StringLength(255)]
        public string full_name { get; set; }
        [StringLength(20)]
        public string district_code { get; set; }

        public virtual district district { get; set; }
    }
}