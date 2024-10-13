using HoTuanPhuoc.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Linq.Dynamic;
using System.Linq.Expressions;
namespace HoTuanPhuoc.Controllers
{
    public class HoTuanPhuocSearchController : Controller
    {
        // GET: Search
        SachOnlineEntities db=new SachOnlineEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Search(string strSearch)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {

                int parsedMaCD;
                bool isParsed = int.TryParse(strSearch, out parsedMaCD);

                List<SACH> kq = db.SACHes.Where(c => (isParsed && c.MaCD == parsedMaCD)
                    || c.TenSach.Contains(strSearch)
                    || c.MoTa.Contains(strSearch)
                    || c.CHUDE.TenChuDe.Contains(strSearch)
                    || c.NHAXUATBAN.TenNXB.Contains(strSearch)).ToList();

                kq = kq.OrderBy(c => c.SoLuongBan).ThenByDescending(c => c.NgayCapNhat).ToList();
                return View(kq);
            }
            return View();
        }
        public ActionResult SearchCoPhanTrang(int? page,string strSearch)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = page ?? 1;
                int parsedMaCD;
                bool isParsed = int.TryParse(strSearch, out parsedMaCD);

                List<SACH> kq = db.SACHes.Where(c => (isParsed && c.MaCD == parsedMaCD)
                    || c.TenSach.Contains(strSearch)
                    || c.MoTa.Contains(strSearch)
                    || c.CHUDE.TenChuDe.Contains(strSearch)
                    || c.NHAXUATBAN.TenNXB.Contains(strSearch)).ToList();

                kq = kq.OrderBy(c => c.SoLuongBan).ThenByDescending(c => c.NgayCapNhat).ToList();
                return View(kq.ToPagedList(iPageNumber,iSize));
            }
            return View();
        }
        public ActionResult SearchCoPhanTrangTuyChon(int? size,int? page, string strSearch)
        {
            

            ViewBag.Search = strSearch;
            ViewBag.size = size ?? 5;
            if (!string.IsNullOrEmpty(strSearch))
            {
                
                int iPageNumber = page ?? 1;
                int parsedMaCD;
                bool isParsed = int.TryParse(strSearch, out parsedMaCD);

                List<SACH> kq = db.SACHes.Where(c => (isParsed && c.MaCD == parsedMaCD)
                    || c.TenSach.Contains(strSearch)
                    || c.MoTa.Contains(strSearch)
                    || c.CHUDE.TenChuDe.Contains(strSearch)
                    || c.NHAXUATBAN.TenNXB.Contains(strSearch)).ToList();

                kq = kq.OrderBy(c => c.SoLuongBan).ThenByDescending(c => c.NgayCapNhat).ToList();
                return View(kq.ToPagedList(iPageNumber,size ?? 5));
            }
            return View();
        }
        public ActionResult SearchTheoDanhMuc(string strSearch,int maCD = 0)
        {
            ViewBag.Search = strSearch;
            List<SACH> kq = db.SACHes.ToList();

            if (!string.IsNullOrEmpty(strSearch))
            {
                strSearch=strSearch.ToLower();
                kq=kq.Where(c=>c.TenSach.ToLower().Contains(strSearch)).ToList();
            }
            if(maCD != 0)
            {
                kq=kq.Where(b=>b.CHUDE.MaCD == maCD).ToList();  

            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe");

            return View(kq);
        }
        public ActionResult Group()
        {
       
            var kq = db.SACHes.GroupBy(s=>s.MaCD).ToList();
            
            return View(kq);
        }
        public ActionResult ThongKe()
        {
            var data = db.SACHes
                .GroupBy(s => s.MaCD)
                .Select(g => new
                {
                    Id = g.Key.ToString(),
                    Count = g.Count(),
                    Sum = g.Sum(n => n.SoLuongBan),
                    Max = g.Max(n => n.SoLuongBan),
                    Min = g.Min(n => n.SoLuongBan),
                    Avg = g.Average(n => (double?)n.SoLuongBan) ?? 0
                })
                .ToList();

            var kq = data.Select(d => new ReportInfo
            {
                Id = d.Id.ToString(),
                Count = d.Count,
                Sum = d.Sum,
                Max = d.Max,
                Min = d.Min,
                Avg = (decimal)d.Avg
            }).ToList();

            return View(kq);
        }
        public  ActionResult SearchPhanTrangSapXep(int? page,string sortProperty,string sortOrder="", string strSearch=null)
        {
            ViewBag.Search = strSearch;
            if (!string.IsNullOrEmpty(strSearch))
            {
                int iSize = 3;
                int iPageNumber = page ?? 1;
                int parsedMaCD;
               
                bool isParsed = int.TryParse(strSearch, out parsedMaCD);

                List<SACH> kq = db.SACHes.Where(c => (isParsed && c.MaCD == parsedMaCD)
                    || c.TenSach.Contains(strSearch)
                    || c.MoTa.Contains(strSearch)
                    || c.CHUDE.TenChuDe.Contains(strSearch)
                    || c.NHAXUATBAN.TenNXB.Contains(strSearch)).ToList();
                if (string.IsNullOrEmpty(sortOrder))
                {
                    sortOrder = "asc";
                    ViewBag.SortOrder = "desc";
                }
                else
                if (sortOrder == "desc")
                {
                    ViewBag.SortOrder = "asc";
                }
                else if (sortOrder == "asc")
                {
                    ViewBag.SortOrder = "desc";

                }
                if(string.IsNullOrEmpty(sortProperty)) 
                    {
                    sortProperty = "TenSach";
                }
                ViewBag.SortProperty = sortProperty;

                kq = kq.OrderBy(sortProperty + " " + sortOrder).ToList();

                return View(kq.ToPagedList(iPageNumber, iSize));
            }
            return View();
        }
    }
}