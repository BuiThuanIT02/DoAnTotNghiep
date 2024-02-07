using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Web_Sach.Common
{
    public class common
    {
        public static string HtmlRate(int? rate)
        {
            var str = "";
            if (rate == 1)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 2)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 3)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 4)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-regular fa-star'></i></li>";
            }
            if (rate == 5)
            {
                str += @"<li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>
                         <li><i class='fa-solid fa-star'></i></li>";
            }
            return str;

        }

    }
}