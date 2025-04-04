﻿using HoTuanPhuoc.Models;
using System;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
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
            var s = collection["NgaySinh"];
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
                kh.MatKhauNL = hashedPassword;
                kh.Email = sEmail;
                kh.DienThoai = sDienThoai;
                kh.DiaChi = sDiaChi;
                kh.NgaySinh = DateTime.Parse(sNgaySinh);

                db.KHACHHANGs.Add(kh);
                try
                {
                    SendMail(sEmail, sTenDN);
                }
                catch (Exception ex)
                {
                    ViewBag.ThongBao = ex.Message;
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    // Catch validation errors
                    foreach (var validationError in ex.EntityValidationErrors)
                    {
                        foreach (var error in validationError.ValidationErrors)
                        {
                            Console.WriteLine($"Property: {error.PropertyName}, Error: {error.ErrorMessage}");
                        }
                    }
                }
                catch (DbUpdateException ex)
                {
                    // Handle update exceptions, such as foreign key violations
                    Console.WriteLine("An error occurred while saving changes: " + ex.Message);

                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                }
                catch (Exception ex)
                {
                    // Catch any other exceptions
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine("Inner Exception: " + ex.InnerException.Message);
                    }
                }

                return RedirectToAction("DangNhap");

            }

            return this.DangKy();
        }
        public ActionResult DangKy_kiemloi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy_kiemloi(KHACHHANG kh)
        {
            if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == kh.TaiKhoan) != null)
            {
                ModelState.AddModelError("TaiKhoan", "Tên đăng nhập đã được sử dụng");
            }
            else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == kh.Email) != null)
            {
                ModelState.AddModelError("Email", "Email đã được sử dụng");

            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)
                try
                {
                    //Gán giá trị cho đối tượng được tạo mới (kh)
                    string hashedPassword = GetMD5(kh.MatKhau);
                    kh.MatKhauNL = hashedPassword;
                    kh.MatKhau = hashedPassword;
                    db.KHACHHANGs.Add(kh);
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
                Credentials = new NetworkCredential("2224802010872@student.tdmu.edu.vn", "kjff rmzc ujwd vjrt"),
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