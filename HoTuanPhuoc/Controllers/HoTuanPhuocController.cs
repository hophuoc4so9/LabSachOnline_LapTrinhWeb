using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HoTuanPhuoc.Models;
namespace HoTuanPhuoc.Controllers
{
    public class HoTuanPhuocController : Controller
    {
        // GET: HoTuanPhuoc
        
        public ActionResult Index()
        {
            return View(LaySachMoi(6));
        }
        public List<SACH> LaySachMoi(int count)
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> kq= db.SACHes.OrderByDescending(a=>a.NgayCapNhat).Take(count).ToList();
            return kq;
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
            return PartialView("_PartialNav");
        }
        [ChildActionOnly]
        public ActionResult PartialChuDe()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<CHUDE> Model = db.CHUDEs.ToList();
          
            return PartialView("_PartialChuDe",Model);
        }
        [ChildActionOnly]
        public ActionResult _PartialSachBanNhieu()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> kq = db.SACHes.OrderByDescending(a => a.SoLuongBan).Take(6).ToList();
            return PartialView(kq);
        }
        public ActionResult sachtheochude()
        {
            int id = int.Parse(Request["id"]);
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> Model = db.SACHes.Where(item => item.MaCD == id).ToList();
            string tenChuDeSach = db.CHUDEs.Where(item => item.MaCD == id).FirstOrDefault().TenChuDe;
            ViewBag.tenChuDeSach = tenChuDeSach;
            return View(Model);
        }
        public ActionResult sachtheonhaxuatban()
        {
            int id = int.Parse(Request["id"]);
            SachOnlineEntities db = new SachOnlineEntities();
            List<SACH> Model = db.SACHes.Where(item => item.MaNXB == id).ToList();
            string tenNXB = db.NHAXUATBANs.Where(item => item.MaNXB == id).FirstOrDefault().TenNXB;
            ViewBag.tenNXB = tenNXB;
            return View(Model);
        }
        public ActionResult BookDetail(int id)
        {
            SachOnlineEntities db = new SachOnlineEntities();
            SACH sACH = db.SACHes.Where(item => item.MaSach == id).ToList().SingleOrDefault();
            return View(sACH);
        }
        [ChildActionOnly]
        public ActionResult _PartialNhaXuatBan()
        {
            SachOnlineEntities db = new SachOnlineEntities();
            List<NHAXUATBAN> Model = db.NHAXUATBANs.ToList();
    
            return PartialView( Model);
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

    }
}