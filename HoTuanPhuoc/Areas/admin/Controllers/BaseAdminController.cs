using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class BaseAdminController : Controller
    {
        // GET: admin/BaseAdmin
       
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName != "DangNhap" && Session["Admin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new System.Web.Routing.RouteValueDictionary(
                        new { controller = "HomeAdmin", action = "DangNhap" }
                    )
                );
            }
            base.OnActionExecuting(filterContext);
        }
    }
}