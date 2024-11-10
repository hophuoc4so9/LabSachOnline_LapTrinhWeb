using HoTuanPhuoc.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
namespace HoTuanPhuoc.Controllers
{
    public class HoTuanPhuocController : Controller
    {
        // GET: HoTuanPhuoc
        SachOnlineEntities data = new SachOnlineEntities();

        public ActionResult Index(int? page)
        {
            int iSize = 6;
            int iPageNumber = page ?? 1;
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> kq = db.SACHes.OrderByDescending(a => a.NgayCapNhat).ToList();
            return View(kq.ToPagedList(iPageNumber, iSize));
        }

        public ActionResult _PartialLogin()
        {
            return PartialView("_PartialLogin");
        }

        public ActionResult ChuDe()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<CHUDE> Model = db.CHUDEs.ToList();
            return View(Model);
        }
        [ChildActionOnly]
        public ActionResult PartialNav()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<MENU> lst = db.MENUs.Where(m=>m.ParentId==null).OrderBy(n=>n.OrderNumber).ToList();
            int[] a= new int[lst.Count];
            for(int i=0;i<lst.Count();i++)
            {
                int id=lst[i].Id;
                List<MENU> l = db.MENUs.Where(m => m.ParentId == id).ToList();
                int k = l.Count();
                a[i] = k;
            }
            ViewBag.lst = a;

            return PartialView("_PartialNav",lst);
        }

        [ChildActionOnly]
        public ActionResult LoadChildMenu(int parentId)
        {
            List < MENU > lst = new List<MENU>();
            lst = data.MENUs.Where(m => m.ParentId == parentId).OrderBy(m => m.OrderNumber).ToList();
            ViewBag.Count = lst.Count();
            int[] a = new int[lst.Count()];
            for (int i = 0; i < lst.Count; i++)
            {
                int id = lst[i].Id;
                List<MENU> l = data.MENUs.Where(m => m.ParentId == id).ToList();
                int k = l.Count();
                a[i] = k;
            }
                ViewBag.lst=a;
                return PartialView("LoadChildMenu", lst);
         }
                [ChildActionOnly]
        public ActionResult PartialChuDe()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<CHUDE> Model = db.CHUDEs.ToList();

            return PartialView("_PartialChuDe", Model);
        }
        [ChildActionOnly]
        public ActionResult _PartialSachBanNhieu()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> kq = db.SACHes.OrderByDescending(a => a.SoLuongBan).Take(6).ToList();

            return PartialView(kq);

        }
        public ActionResult TrangTin(string metatitle)
        {
            if (string.IsNullOrEmpty(metatitle)) return RedirectToAction("DangNhap","User");
            TRANGTIN tt = data.TRANGTINs.Where(t => t.MetaTitle == metatitle).SingleOrDefault();
            return View("TrangTin", tt);
        }
        public ActionResult sachtheochude(int? page)
        {
            int id = int.Parse(Request["id"]);
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> Model = db.SACHes.Where(item => item.MaCD == id).ToList();
            int iSize = 6;
            int iPageNumber = page ?? 1;
            string tenChuDeSach = db.CHUDEs.Where(item => item.MaCD == id).FirstOrDefault().TenChuDe;
            ViewBag.tenChuDeSach = tenChuDeSach;
            return View(Model.ToPagedList(iPageNumber, iSize));
        }
        public ActionResult sachtheochude2(int? page,int MaCD)
        {
            int id = MaCD;
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> Model = db.SACHes.Where(item => item.MaCD == id).ToList();
            int iSize = 6;
            int iPageNumber = page ?? 1;
            string tenChuDeSach = db.CHUDEs.Where(item => item.MaCD == id).FirstOrDefault().TenChuDe;
            ViewBag.tenChuDeSach = tenChuDeSach;
            return View("sachtheochude",Model.ToPagedList(iPageNumber, iSize));
        }
        public ActionResult sachtheonhaxuatban(int? page)
        {
            int id = int.Parse(Request["id"]);
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> Model = db.SACHes.Where(item => item.MaNXB == id).ToList();
            int iSize = 6;
            int iPageNumber = page ?? 1;
            string tenNXB = db.NHAXUATBANs.Where(item => item.MaNXB == id).FirstOrDefault().TenNXB;
            ViewBag.tenNXB = tenNXB;
            return View(Model.ToPagedList(iPageNumber, iSize));
        }
        public ActionResult sachtheonhaxuatban2(int? page,int MaNXB)
        {
            int id = MaNXB;
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> Model = db.SACHes.Where(item => item.MaNXB == id).ToList();
            int iSize = 6;
            int iPageNumber = page ?? 1;
            string tenNXB = db.NHAXUATBANs.Where(item => item.MaNXB == id).FirstOrDefault().TenNXB;
            ViewBag.tenNXB = tenNXB;
            return View("sachtheonhaxuatban", Model.ToPagedList(iPageNumber, iSize));
        }
        public ActionResult BookDetail(int id)
        {
            SachOnlineEntities db = new SachOnlineEntities();
            SACH sACH = db.SACHes.Where(item => item.MaSach == id).ToList().SingleOrDefault();
            ViewBag.MaSach = id;
            return View(sACH);
        }
        [ChildActionOnly]
        public ActionResult _PartialNhaXuatBan()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<NHAXUATBAN> Model = db.NHAXUATBANs.ToList();

            return PartialView(Model);
        }
        public ActionResult _PartialBinhLuan(int maSach)
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<BinhLuan> Model = db.BinhLuans.Where(a => a.MaSach == maSach).ToList();
            ViewBag.MaSach = maSach;
            return PartialView(Model);
        }
        [ChildActionOnly]
        public ActionResult _PartialFooter()
        {
            return PartialView();
        }
        [ChildActionOnly]
        public ActionResult _PartialCarousel()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult ThemMoiBinhLuan(FormCollection collection, BinhLuan bl)
        {
            var noidung = collection["NoiDung"];
            if (String.IsNullOrEmpty(noidung))
            {
                ViewBag.ThongBao = "Nội dung không được rỗng";
                return this.BookDetail(int.Parse(bl.MaSach.ToString()));
            }


            bl.NgayTao = DateTime.Now;
            bl.MaKH = Session["Taikhoan"] == null ? 1 : (Session["Taikhoan"] as KHACHHANG).MaKH;
            bl.MaSach = collection["MaSach"] == null ? 1 : int.Parse(collection["MaSach"]);
            SachOnlineEntities db = new SachOnlineEntities();
            db.BinhLuans.Add(bl);
            db.SaveChanges();
            return RedirectToAction("BookDetail", new { id = int.Parse(collection["MaSach"]) });

        }
    }
}