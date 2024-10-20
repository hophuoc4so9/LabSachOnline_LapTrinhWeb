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
using System.Security.Cryptography;
using System.Text;
using System.Data.Entity.Validation;

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
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            string sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sNhapLaiMatKhau = collection["NhapLaiMatKhau"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var sDiaChi = collection["DiaChi"];
            var sNgaySinh = String.Format("{0:MM/dd/yyyy}", collection["NgaySinh"]);

            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Mật khẩu không được rỗng";
            }
            else if (String.IsNullOrEmpty(sNhapLaiMatKhau))
            {
                ViewData["err4"] = "Nhập mật khẩu lại";
            }
            else if (sMatKhau != sNhapLaiMatKhau)
            {
                ViewData["err4"] = "Mật khẩu phải giống nhau";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã được sử dụng";
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                string hashedPassword = GetMD5(sMatKhau);

                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN;
                kh.MatKhau = hashedPassword;
                kh.Email = sEmail;
                kh.DienThoai = sDienThoai;
                kh.DiaChi = sDiaChi;
                kh.NgaySinh = DateTime.Parse(sNgaySinh);

                db.KHACHHANGs.Add(kh);
                db.SaveChanges();
                SendMail(sEmail, sTenDN);
                return RedirectToAction("DangNhap");

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

        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string tenDangNhap = f["TenDN"];
            string matKhau = f["MatKhau"];
            ViewBag.ThongBao = "";
            if (string.IsNullOrEmpty(tenDangNhap) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.ThongBao = "Tên đăng nhập và mật khảu không được trống !";
            }
            else
            {
                string hashedPassword = GetMD5(matKhau);
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == tenDangNhap && n.MatKhau == hashedPassword);
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


        public ActionResult SendMail(string recipientEmail, string sTenDN)
        {
            // Cấu hình thông tin Gmail
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("2224802010872@student.tdmu.edu.vn", "yjrf goqc ufaq gurk"),
                EnableSsl = true
            };

            // Tạo email
            var message = new MailMessage
            {
                From = new MailAddress("2224802010872@student.tdmu.edu.vn"), // Replace with your email
                Subject = "Chúc mừng bạn đã đăng ký thành công tài khoản vào hệ thống",
                Body = "Chúc mừng! Chúc mừng bạn đã đăng ký thành công tài khoản " + sTenDN + " vào hệ thống sách",
                //IsBodyHtml = true // Set to true if you want to format the body with HTML
            };

            message.To.Add(new MailAddress(recipientEmail));

            // Gửi email
            mail.Send(message);

            return View("DangNhap"); // Redirect or return a different view if needed
        }



    }
}