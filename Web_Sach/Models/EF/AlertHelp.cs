using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web_Sach.Models.EF
{
    public static class AlertHelp
    {
        public static void SetAlert(Controller controller,string content, string message)
        {
            controller.TempData["AlertContent"] = content;

            if (message == "success")
            {
                controller.TempData["AlertMessage"] = "alert-success";
            }
            else if (message == "warning")
            {
                controller.TempData["AlertMessage"] = "alert-warning";
            }
            else if (message == "error")
            {
                controller.TempData["AlertMessage"] = "alert-danger";
            }

        }


    }
}