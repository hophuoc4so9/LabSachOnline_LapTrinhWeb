using HoTuanPhuoc.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class TrangTinController : BaseAdminController
    {
        // GET: admin/TrangTin
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            return View(db.TRANGTINs.ToList());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(TRANGTIN tt)
        {
            if (ModelState.IsValid)
            {
                tt.MetaTitle = tt.TenTrang.RemoveDiacritics().Replace(" ", "-"); tt.NgayTao = DateTime.Now;
                db.TRANGTINs.Add(tt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();

        }
        public ActionResult Edit(int id)
        {
            TRANGTIN tt = db.TRANGTINs.Where(n => n.MaTT == id).FirstOrDefault();
            return View(tt);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {

            if (ModelState.IsValid)
            {
                int mtt = int.Parse(f["MaTT"]);
                var tt = db.TRANGTINs.Where(t => t.MaTT == mtt).SingleOrDefault();
                tt.TenTrang = f["TenTrang"];
                tt.NoiDung = f["NoiDung"];
                tt.NgayTao = Convert.ToDateTime(f["NgayTao"]);
                tt.MetaTitle = f["TenTrang"].RemoveDiacritics().Replace(" ", "-");
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit");

            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            TRANGTIN tt = db.TRANGTINs.Where(n => n.MaTT == id).FirstOrDefault();
            return View(tt);
        }
        [HttpPost]
        public ActionResult Delete(FormCollection f)
        {
            int id = int.Parse(f["id"]);
            TRANGTIN tt = db.TRANGTINs.Where(n => n.MaTT == id).FirstOrDefault();

            db.TRANGTINs.Remove(tt);
            return RedirectToAction("Index");

        }
    }
}