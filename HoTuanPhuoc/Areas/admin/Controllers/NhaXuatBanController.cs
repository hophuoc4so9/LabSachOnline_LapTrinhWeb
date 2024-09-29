using HoTuanPhuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class NhaXuatBanController : Controller
    {
        SachOnlineEntities db = new SachOnlineEntities();
        // GET: admin/NhaXuatBan
        public ActionResult Index()
        {
      
            List < NHAXUATBAN >  model= db.NHAXUATBANs.ToList();
            return View(model);
        }
        public ActionResult ChiTietNhaXuatBan()
        {
            int id = int.Parse(Request["id"]);
         
            NHAXUATBAN model = db.NHAXUATBANs.Where( c=> c.MaNXB == id).FirstOrDefault();
            
            return View(model);
        }
        [HttpGet]
        public ActionResult SuaNXB(int id)
        {
            NHAXUATBAN model = db.NHAXUATBANs.Where(c => c.MaNXB == id).SingleOrDefault();

            return View(model);
        }
        [HttpGet]
        public ActionResult XoaNXB(int id)
        {
            NHAXUATBAN model = db.NHAXUATBANs.Where(c => c.MaNXB == id).SingleOrDefault();
            //db.NHAXUATBANs.Remove(model);
            //db.SaveChanges();
            return View(model);
        }
        [HttpPost]
        public ActionResult XoaNXB(FormCollection f)
        {
            int id = int.Parse(f["MaNXB"]);
            NHAXUATBAN kq = db.NHAXUATBANs.Where(c => c.MaNXB == id).SingleOrDefault();
            db.NHAXUATBANs.Remove(kq);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ThemMoiNhaXuatBan()
        {
            NHAXUATBAN model = new NHAXUATBAN();
           
            return View(model);
        }
        [HttpPost]
        public ActionResult ThemMoiNhaXuatBan(NHAXUATBAN model)
        {
         
            db.NHAXUATBANs.Add(model);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public ActionResult SuaNXB(FormCollection f)
        {
            int maNXB = int.Parse(f["MaNXB"]);
            NHAXUATBAN kq = db.NHAXUATBANs.Where(c => c.MaNXB == maNXB).SingleOrDefault();
            kq.TenNXB = f["TenNXB"];
            kq.DiaChi = f["DiaChi"];
            kq.DienThoai = f["DienThoai"];
            db.SaveChanges();
            return View(kq);
        }
        public NHAXUATBAN getNXB(int id)
        {
            return db.NHAXUATBANs.Where(c => c.MaNXB == id).FirstOrDefault();
        }
    }
}