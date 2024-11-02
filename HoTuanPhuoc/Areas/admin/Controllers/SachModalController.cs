using HoTuanPhuoc.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class SachModalController : Controller
    {
        private readonly SachOnlineEntities db = new SachOnlineEntities();

        public ActionResult Index()
        {
            return View();
        }

        // GET: /Admin/SachModal/List
        public JsonResult List()
        {
            var books = db.SACHes
                .Select(book => new
                {
                    book.MaSach,
                    book.TenSach,
                    book.MoTa,
                    book.AnhBia,
                    NgayCapNhat = book.NgayCapNhat,
                    book.SoLuongBan,
                    book.GiaBan,
                    TenChuDe = book.CHUDE.TenChuDe,
                    TenNXB = book.NHAXUATBAN.TenNXB
                })
                .ToList()
                .Select(book => new
                {
                    book.MaSach,
                    book.TenSach,
                    book.MoTa,
                    book.AnhBia,
                    NgayCapNhat = book.NgayCapNhat?.ToString("dd/MM/yyyy") ?? string.Empty,
                    book.SoLuongBan,
                    book.GiaBan,
                    book.TenChuDe,
                    book.TenNXB
                })
                .ToList();

            return Json(new { books }, JsonRequestBehavior.AllowGet);
        }

        // GET: /Admin/SachModal/GetBook
        public JsonResult GetBook(int id)
        {
            var book = db.SACHes.Find(id);
            if (book == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }

            return Json(new
            {
                book.MaSach,
                book.TenSach,
                book.MoTa,
                book.AnhBia,
                book.SoLuongBan,
                book.GiaBan
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Add(SACH book, HttpPostedFileBase image, int soLuongBan, decimal giaBan)
        {
            try
            {
                if (image != null && image.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(image.FileName);
                    string uploadPath = Server.MapPath("~/Images/");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, fileName);
                    image.SaveAs(filePath);
                    book.AnhBia = fileName;
                }

                book.NgayCapNhat = DateTime.Now;
                book.SoLuongBan = soLuongBan;
                book.GiaBan = giaBan;

                db.SACHes.Add(book);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error in Add method: " + ex.Message);
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Update(SACH book, HttpPostedFileBase image)
        {
            try
            {
                var existingBook = db.SACHes.Find(book.MaSach);
                if (existingBook == null)
                {
                    return Json(new { success = false, message = "Book not found." });
                }

                if (image != null && image.ContentLength > 0)
                {
                    string fileName = Path.GetFileName(image.FileName);
                    string uploadPath = Server.MapPath("~/Images/");
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string filePath = Path.Combine(uploadPath, fileName);
                    image.SaveAs(filePath);
                    existingBook.AnhBia = fileName;
                }

                existingBook.TenSach = book.TenSach;
                existingBook.MoTa = book.MoTa;
                existingBook.SoLuongBan = book.SoLuongBan;
                existingBook.GiaBan = book.GiaBan;
                existingBook.NgayCapNhat = DateTime.Now;

                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Delete(int id)
        {
            try
            {
                var book = db.SACHes.Find(id);
                if (book == null)
                {
                    return Json(new { success = false, message = "Book not found." });
                }

                db.SACHes.Remove(book);
                db.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        public JsonResult DsChuDe()
        {
            var dsCD = db.CHUDEs
                .Select(cd => new
                {
                    MaCD = cd.MaCD,
                    TenCD = cd.TenChuDe
                })
                .ToList();

            return Json(new { code = 200, dsCD, msg = "Lấy danh sách chủ đề thành công" }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DsNXB()
        {
            var dsNXB = db.NHAXUATBANs
                .Select(nxb => new
                {
                    MaNXB = nxb.MaNXB,
                    TenNXB = nxb.TenNXB
                })
                .ToList();

            return Json(new { code = 200, dsNXB, msg = "Lấy danh sách nhà xuất bản thành công" }, JsonRequestBehavior.AllowGet);
        }
    }
}