using HoTuanPhuoc.Models;
using System.Linq;
using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class HomeAdminController : Controller
    {
        // GET: admin/Home
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            // Gán giá trị cho đối tượng được tạo mới (ad)
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

    }
}