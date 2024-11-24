using HoTuanPhuoc.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class ChuDeController : BaseAdminController
    {
        // GET: admin/ChuDe
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsChuDe(int page = 1, int pageSize = 5, string search = "")
        {
            try
            {
                var dsCD = (from cd in db.CHUDEs
                            where cd.TenChuDe.Contains(search)
                            select new
                            {
                                MaCD = cd.MaCD,
                                TenCD = cd.TenChuDe
                            }).ToList();

                var totalItems = dsCD.Count();
                var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

                var paginatedDsCD = dsCD.Skip((page - 1) * pageSize).Take(pageSize).ToList();

                return Json(new
                {
                    code = 200,
                    dsCD = paginatedDsCD,
                    totalItems = totalItems,
                    totalPages = totalPages,
                    currentPage = page,
                    msg = "Lấy danh sách chủ đề thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách chủ đề thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult Detail(int maCD)
        {
            try
            {
                var cd = (from s in db.CHUDEs
                          where (s.MaCD == maCD)
                          select new
                          {
                              MaCD = s.MaCD,
                              TenChuDe = s.TenChuDe
                          }).SingleOrDefault();
                //db.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                return Json(new { code = 200, cd = cd, msg = "Lấy thông tin chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg
                =
                "Lấy thông tin chủ đề thất bại." + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddChuDe(string strTenCD)
        {
            try
            {
                var cd = new CHUDE();
                cd.TenChuDe = strTenCD; db.CHUDEs.Add(cd); db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm chủ đề thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(int maCD, string strTenCD)
        {
            try
            {
                var cd = db.CHUDEs.SingleOrDefault(c => c.MaCD == maCD); cd.TenChuDe = strTenCD;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sửa chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Sửa chủ đề thất bại. Lỗi" + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int maCD)
        {
            try
            {

                var cd = db.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                db.CHUDEs.Remove(cd);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa chủ đề thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Xóa chủ đề thất bại. Lỗi" + ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}