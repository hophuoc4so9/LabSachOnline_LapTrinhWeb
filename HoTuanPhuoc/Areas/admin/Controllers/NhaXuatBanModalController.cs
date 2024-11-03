using HoTuanPhuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class NhaXuatBanModalController : Controller
    {
        // GET: admin/NhaXuatBanModal
        SachOnlineEntities db = new SachOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult DsNhaXuatBan(int page = 1, int pageSize = 5, string search = "")
        {
            try
            {
                var dsCD = (from cd in db.NHAXUATBANs
                            where cd.TenNXB.Contains(search)
                            select new
                            {
                                MaNXB = cd.MaNXB,
                                TenNXB = cd.TenNXB,
                                DiaChi = cd.DiaChi,
                                DienThoai = cd.DienThoai
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
                    msg = "Lấy danh sách nhà xuất bản thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy danh sách NXB thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult Detail(int MaNXB)
        {
            try
            {
                var cd = (from s in db.NHAXUATBANs
                          where (s.MaNXB == MaNXB)
                          select new
                          {
                              MaNXB = s.MaNXB,
                              TenNXB = s.TenNXB,
                              DiaChi = s.DiaChi,
                              DienThoai = s.DienThoai
                          }).SingleOrDefault();
                //db.CHUDEs.SingleOrDefault(c => c.MaCD == maCD);
                return Json(new { code = 200, cd = cd, msg = "Lấy thông tin nxb thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg
                =
                "Lấy thông tin nxb thất bại." + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddNXB(string strTenCD,string DiaChi,string DienThoai)
        {
            try
            {
                var cd = new NHAXUATBAN();
                cd.TenNXB = strTenCD;
                cd.DiaChi = DiaChi;
                cd.DienThoai = DienThoai;
                db.NHAXUATBANs.Add(cd); db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm nhà xuất bản thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm nhà xuất bản thất bại. Lỗi " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Update(int maCD, string strTenCD, string DiaChi, string DienThoai)
        {
            try
            {
                var cd = db.NHAXUATBANs.SingleOrDefault(c => c.MaNXB == maCD);
                cd.TenNXB = strTenCD;
                cd.DiaChi = DiaChi;
                cd.DienThoai = DienThoai;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Sửa nhà xuất bản thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Sửa nhà xuất bản thất bại. Lỗi" + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult Delete(int maCD)
        {
            try
            {

                var cd = db.NHAXUATBANs.SingleOrDefault(c => c.MaNXB == maCD);
                db.NHAXUATBANs.Remove(cd);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa nhà xuất bản thành công." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    code = 500,
                    msg = "Xóa xuất bản thất bại. Lỗi" + ex.Message
                }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}