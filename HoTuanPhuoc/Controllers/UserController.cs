using HoTuanPhuoc.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Infrastructure.Design;

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
            ViewBag.ThongBao = "";
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
        public ActionResult DangNhap(FormCollection f)
        {
            string tenDangNhap= f["TenDN"];
            string matKhau =f["MatKhau"];
            ViewBag.ThongBao = "";
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.ThongBao = "Tên đăng nhập và mật khảu không được trống !";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tenDangNhap && n.MatKhau == matKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["Taikhoan"] = kh;
                    if (f["remember"] == "true")
                    {
                        Response.Cookies["TenDN"].Value = tenDangNhap;
                        Response.Cookies["MatKhau"].Value = matKhau;
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(365);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(365);
                    }
                    else
                    {
                        Response.Cookies["TenDN"].Expires = DateTime.Now.AddDays(-1);
                        Response.Cookies["MatKhau"].Expires = DateTime.Now.AddDays(-1);
                    }
                    return RedirectToAction("Index", "HoTuanPhuoc");
                    //SendMail(kh.Email);
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }


            }
                return View();

        }
       public ActionResult DangXuat()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "HoTuanPhuoc");
        }


        public ActionResult SendMail(string recipientEmail)
        {
            // Cấu hình thông tin Gmail
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("2224802010872@student.tdmu.edu.vn", "coce jxbe bmql smty"),
                EnableSsl = true
            };

            // Tạo email
            var message = new MailMessage
            {
                From = new MailAddress("ten_tai_khoan@gmail.com"), // Replace with your email
                Subject = "Đăng Nhập Thành Công",
                Body = "Chúc mừng! Bạn đã đăng nhập thành công vào hệ thống.",
                IsBodyHtml = true // Set to true if you want to format the body with HTML
            };

            message.To.Add(new MailAddress(recipientEmail));

            // Gửi email
            mail.Send(message);

            return View("DangNhap"); // Redirect or return a different view if needed
        }



    }
}