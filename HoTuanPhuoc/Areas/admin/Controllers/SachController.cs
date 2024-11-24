using HoTuanPhuoc.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class SachController : BaseAdminController
    {
        // GET: admin/Sach
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index(int? page, string strSearch)
        {
            int iSize = 7;
            int iPageNumber = page ?? 1;
            ViewBag.Search = strSearch ?? "";
            if (string.IsNullOrEmpty(strSearch))
            {
                List<SACH> kq = db.SACHes.OrderBy(a => a.MaSach).ToList();
                return View(kq.ToPagedList(iPageNumber, iSize));

            }
            else
            {
                List<SACH> kq = db.SACHes.Where(c => c.TenSach.Contains(strSearch)
                    || c.MoTa.Contains(strSearch)
                    || c.CHUDE.TenChuDe.Contains(strSearch)
                    || c.NHAXUATBAN.TenNXB.Contains(strSearch)).OrderBy(a => a.MaSach).ToList();
                return View(kq.ToPagedList(iPageNumber, iSize));

            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(a => a.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(a => a.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(FormCollection f, SACH sach, HttpPostedFileBase fFileUpload)
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(a => a.TenChuDe), "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(a => a.TenNXB), "MaNXB", "TenNXB");
            if (fFileUpload == null)
            {
                //Nội dung thông báo yêu cầu chọn ảnh bìa
                ViewBag.ThongBao = "Hãy chọn ảnh bìa.";
                //Lưu thông tin để khi load lại trang do yêu cầu chọn ảnh bìa sẽ hiển thị   các thông tin này lên trang
                ViewBag.TenSach = f["sTenSach"];
                ViewBag.MoTa = f["sMoTa"];
                ViewBag.SoLuong = int.Parse(f["iSoLuong"]);
                ViewBag.GiaBan = decimal.Parse(f["mGiaBan"]);
                ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", int.Parse(f["MaCD"]));
                ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", int.Parse(f["MaNXB"]));
                return View();
            }
            else
            {

                if (ModelState.IsValid)
                {
                    //Lấy tên file (Khai báo thư viện: System.IO)
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    sFileName = RemoveDiacritics2(sFileName);

                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName); //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    using (var img = System.Drawing.Image.FromStream(fFileUpload.InputStream))
                    {
                        var resizedImg = new Bitmap(img, new Size(400, 500));
                        resizedImg.Save(path, img.RawFormat);
                    }

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);

                    }
                    //Lưu Sạch vào CSDL
                    sach.TenSach = f["sTenSach"];
                    sach.MoTa = f["sMoTa"];
                    sach.AnhBia = sFileName;
                    DateTime ngayCapNhat;
                    if (DateTime.TryParse(f["dNgayCapNhat"], out ngayCapNhat))
                    {
                        sach.NgayCapNhat = ngayCapNhat;
                    }
                    sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                    sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                    sach.MaCD = int.Parse(f["MaCD"]);
                    sach.MaNXB = int.Parse(f["MaNXB"]);
                    db.SACHes.Add(sach);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                return View();
            }
        }
        public ActionResult Details(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }
        public ActionResult Delete(int id)
        {
            SACH sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sach);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaSach == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Sách này đang có trong bảng Chi tiết đặt hàng <br> Nếu muốn xóa thì phải xóa hết mã sách này trong bảng Chi tiết đặt hàng";
                return View(sach);
            }

            var vietsach = db.VIETSACHes.Where(vs => vs.MaSach == id).ToList();
            if (vietsach != null)
            {
                db.VIETSACHes.RemoveRange(vietsach);

                db.SaveChanges();
            }


            db.SACHes.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var sach = db.SACHes.SingleOrDefault(n => n.MaSach == id);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);

            return View(sach);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f, HttpPostedFileBase fFileUpload)
        {
            int masach = int.Parse(f["MaSach"]);
            var sach = db.SACHes.SingleOrDefault(n => n.MaSach == masach);
            ViewBag.MaCD = new SelectList(db.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChuDe", sach.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB", sach.MaNXB);
            if (ModelState.IsValid)
            {
                if (fFileUpload != null)
                {
                    var sFileName = Path.GetFileName(fFileUpload.FileName);
                    sFileName = RemoveDiacritics2(sFileName);

                    //Lấy đường dẫn lưu file
                    var path = Path.Combine(Server.MapPath("~/Images"), sFileName); //Kiểm tra ảnh bìa đã tồn tại chưa để lưu lên thư mục
                    using (var img = System.Drawing.Image.FromStream(fFileUpload.InputStream))
                    {
                        var resizedImg = new Bitmap(img, new Size(400, 500));
                        resizedImg.Save(path, img.RawFormat);
                    }

                    if (!System.IO.File.Exists(path))
                    {
                        fFileUpload.SaveAs(path);
                    }
                    sach.AnhBia = sFileName;
                }

                sach.TenSach = f["TenSach"];
                sach.MoTa = f["MoTa"];
                sach.NgayCapNhat = Convert.ToDateTime(f["dNgayCapNhat"]);
                sach.SoLuongBan = int.Parse(f["iSoLuong"]);
                sach.GiaBan = decimal.Parse(f["mGiaBan"]);
                sach.MaCD = int.Parse(f["MaCD"]);
                sach.MaNXB = int.Parse(f["MaNXB"]);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sach);
        }
        private string RemoveDiacritics2(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }


    }
}