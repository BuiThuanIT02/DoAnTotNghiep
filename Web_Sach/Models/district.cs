using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Models
{
    public partial class district
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public district()
        {
            wards = new HashSet<ward>();
        }

        [Key]
        [StringLength(20)]
        public string code { get; set; }

        [StringLength(255)]
        public string full_name { get; set; }

        [StringLength(20)]
        public string province_code { get; set; }

        public virtual province province { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ward> wards { get; set; }
    }
}