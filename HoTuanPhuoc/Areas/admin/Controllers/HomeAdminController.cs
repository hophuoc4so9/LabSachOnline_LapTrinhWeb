using HoTuanPhuoc.Models;
using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        public ActionResult _PartialLogin()
        {
            return PartialView();
        }

        public ActionResult DangXuat()
        {
            Session["Admin"] = null;
            return RedirectToAction("DangNhap", "HomeAdmin");
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến
            var sTenDN = f["UserName"];
            var sMatKhau = f["Password"];
            // Gán giá trị cho đối tượng được tạo mới (ad)
            //
            string hashedPassword = GetMD5(sMatKhau);
            ADMIN ad = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && (n.MatKhau == hashedPassword || n.MatKhau == sMatKhau));
            if (ad != null)
            {
                Session["Admin"] = ad;
                return RedirectToAction("Index", "Sach");
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ADMIN kh)
        {
            if (db.ADMINs.SingleOrDefault(n => n.TenDN == kh.TenDN) != null)
            {
                ModelState.AddModelError("TenDN", "Tên đăng nhập đã được sử dụng");
            }



            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                try
                {
                    //Gán giá trị cho đối tượng được tạo mới (kh)
                    string hashedPassword = GetMD5(kh.MatKhau);

                    kh.MatKhau = hashedPassword;
                    db.ADMINs.Add(kh);
                    db.SaveChanges();

                    return RedirectToAction("DangNhap");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            // Log the error message
                            System.Diagnostics.Debug.WriteLine($"Property: {validationError.PropertyName} Error: {validationError.ErrorMessage}");
                            // Optionally, add the error message to the ModelState to display it in the view
                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }

            }

            return this.DangKy();
        }
        public static string GetMD5(string str)
        {

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            byte[] bHash = md5.ComputeHash(Encoding.UTF8.GetBytes(str));

            StringBuilder sbHash = new StringBuilder();

            foreach (byte b in bHash)
            {

                sbHash.Append(String.Format("{0:x2}", b));

            }

            return sbHash.ToString();

        }

    }
}