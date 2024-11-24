using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class HomeController : Controller
    {
        // GET: admin/Home
        public ActionResult Index()
        {
            return RedirectToAction("DangNhap", "HomeAdmin");
        }
        public ActionResult DangKy()
        {
            return RedirectToAction("DangKy", "HomeAdmin");
        }
        public ActionResult DangNhap()
        {
            return RedirectToAction("DangNhap", "HomeAdmin");
        }
    }
}