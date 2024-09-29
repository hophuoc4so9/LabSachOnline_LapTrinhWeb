using HoTuanPhuoc.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoTuanPhuoc.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG nd)
        {
            // Retrieve form values
            var hoten = collection["HoTen"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var nhaplaimk = collection["MatKhauL"];
            var email = collection["Email"];
            var dienthoai = collection["DienThoai"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            // Error validation logic
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["err1"] = "Họ tên không được để trống";
            }
            else if (String.IsNullOrEmpty(tendn))
            {
                ViewData["err2"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["err3"] = "Mật khẩu không được để trống";
            }
            else if (String.IsNullOrEmpty(nhaplaimk))
            {
                ViewData["err4"] = "Nhập lại mật khẩu không được để trống";
            }
            else if (matkhau != nhaplaimk)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["err5"] = "Email không được để trống";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tendn || n.Email == email) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc email đã được sử dụng";
            }
            else
            {
                // Populate and save user data to the database
                nd.HoTen = hoten;
                nd.TaiKhoan = tendn;
                nd.MatKhau = matkhau;
                nd.Email = email;
                nd.DienThoai = dienthoai;
                nd.NgaySinh = DateTime.Parse(ngaysinh);
                db.KHACHHANGs.Add(nd);
                db.SaveChanges();
                ViewBag.ThongBao = "Đã đăng ký thành công";
            }

            return this.DangKy();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            string stenDN= collection["TenDN"];
            string shatkhau =collection["MatKhau"];
            if (string.IsNullOrEmpty(stenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (string.IsNullOrEmpty(shatkhau))
            {
                ViewData["Err2"] = "Phát nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == stenDN && n.MatKhau == shatkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }


            }
                return View();

        }


    }
}