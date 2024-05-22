using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Web_Sach.Models.Dao
{
    public class TaiKhoanExtension
    {
        public int ID { get; set; }

      
        public string TaiKhoan1 { get; set; }
       
        public string Phone { get; set; }
       
        public string Email { get; set; }
      
        public string Password { get; set; }
       
        public string Address { get; set; }
      
        public string GioiTinh { get; set; }
      
        public DateTime? NgaySinh { get; set; }
       
        public bool Status { get; set; }
       
        public string FullName { get; set; }
     

        public int? Role { get; set; }
    }
}