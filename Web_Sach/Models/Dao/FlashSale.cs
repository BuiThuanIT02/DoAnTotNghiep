using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.Dao
{
    public class FlashSale
    {
        public Sach sach { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKeThuc { get; set; }
       
    }
}