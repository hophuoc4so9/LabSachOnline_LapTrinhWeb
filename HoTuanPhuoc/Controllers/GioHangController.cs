using HoTuanPhuoc.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace HoTuanPhuoc.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        public ActionResult Index()
        {
            return View();
        }
        SachOnlineEntities db = new SachOnlineEntities();

        // GET: GioHang
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                // Khởi tạo Giỏ hàng (giỏ hàng chưa tồn tại)
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }

        // Cập nhật phương thức ThemGioHang()
        public ActionResult ThemGioHang(int ms, string url)
        {
            // Lấy giỏ hàng hiện tại
            List<GioHang> lstGioHang = LayGioHang();
            // Kiểm tra nếu sản phẩm chưa có trong giỏ thì thêm vào, nếu có thì tăng số lượng
            GioHang sp = lstGioHang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }

            return Redirect(url);
        }
        // Tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        // Tính tổng tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }

        // Action trả về view GioHang
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "HoTuanPhuoc");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        //Tạo partial view để hiển thị thông tin giỏ hàng
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return PartialView();
        }
        //Xóa sản phẩm khỏi giỏ hàng
        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();

            //Kiểm tra sách đã có trong Session["GioHang"]
            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);

            //Xóa sp khỏi giỏ hàng
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);

                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "HoTuanPhuoc");
                }
            }

            return RedirectToAction("GioHang");
        }
        [HttpPost]
        //Cập nhật giỏ hàng
        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();

            GioHang sp = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSach);

            //Nếu tồn tại thì cho sửa số lượng
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }

            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "HoTuanPhuoc");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            //Kiểm tra đăng nhập chưa
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }

            //Kiểm tra có hàng trong giỏ chưa
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "SachOnline");
            }

            //Lấy hàng từ Session
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();

            return View(lstGioHang);
        }
        // test so 2 
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            // Thêm đơn hàng
            DONDATHANG ddh = new DONDATHANG();

            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<GioHang> lstCart = LayGioHang();

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            SendMail(kh.Email);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.DaThanhToan = false;
           
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();
            string phuongthucthanhtoan = f["paymentMethod"];
          
            // Thêm chi tiết đơn hàng
            foreach (var item in lstCart)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();

                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;

                db.CHITIETDATHANGs.Add(ctdh);
            }

            db.SaveChanges();
            if (phuongthucthanhtoan=="momo")
            {

            }    
            else
            {

            }    
                Session["GioHang"] = null;

            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();

        }





        public ActionResult SendMail(string recipientEmail)
        {
            List<GioHang> lstGioHang = LayGioHang();
            double soluong = TongSoLuong();
            double soTien = TongTien();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            // Cấu hình thông tin Gmail
            string Body = $@"
<html>
<head>
    <style>
        table {{
            width: 100%;
            border-collapse: collapse;
        }}
        th, td {{
            border: 1px solid #dddddd;
            text-align: center;
            padding: 8px;
        }}
        th {{
            background-color: #f2f2f2;
        }}
        .total {{
            text-align: right;
            color: red;
            font-weight: bold;
            padding-right: 10px;
        }}
    </style>
</head>
<body>
  <h1>Chào {kh.HoTen}</h1>
    <p>Cảm ơn bạn đã đặt hàng của chúng tôi. Chi tiết đơn hàng của bạn ở trên.</p>
    <h2 style='text-align:center; font-weight:bold;'>THÔNG TIN ĐƠN HÀNG</h2>
    <table>
        <tr>
            <th>Sách</th>
            
            <th>Số lượng</th>
            <th>Thành tiền</th>
        </tr>";

            foreach (var item in lstGioHang)
            {
                Body += $@"
        <tr>
            <td>{@item.iMaSach}</td>
            <td>{@item.iSoLuong}</td>
            <td>{string.Format("{0:#,##0,0}", @item.dThanhTien)}</td>
        </tr>";
            }

            Body += $@"
        <tr>
            <td colspan='4' class='total'>
                Tổng tiền: {string.Format("{0:#,##0,0}", soTien)} VND
            </td>
        </tr>
    </table>
  
</body>
</html>";

            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential("2224802010872@student.tdmu.edu.vn", "qupv gfrx jqwi zrqv"),
                EnableSsl = true
            };
           
            // Tạo email
            var message = new MailMessage
            {
                From = new MailAddress("2224802010872@student.tdmu.edu.vn"), // Replace with your email
                Subject = "Xác nhận đơn hàng",
                Body = Body,
            IsBodyHtml = true // Set to true if you want to format the body with HTML
            };

            message.To.Add(new MailAddress(recipientEmail));

            // Gửi email
            mail.Send(message);

            return View("DangNhap"); // Redirect or return a different view if needed
        }
    }
}