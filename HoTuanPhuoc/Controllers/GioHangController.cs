using HoTuanPhuoc.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
            TempData["Message"] = "Sản phẩm đã được thêm vào giỏ hàng.";
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
        // test 12324123124
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


            if (phuongthucthanhtoan == "momo")
            {
                return RedirectToAction("PaymentVNPay", "GioHang", new { id = ddh.MaDonHang, ngayGiao = NgayGiao });

            }
            else if (phuongthucthanhtoan == "vnpay")
            {
                return RedirectToAction("PaymentVNPay", "GioHang", new { id = ddh.MaDonHang, ngayGiao = NgayGiao });
            }
            Session["GioHang"] = null;

            return RedirectToAction("XacNhanDonHang", "GioHang");
        }
        public ActionResult XacNhanDonHang()
        {
            return View();

        }





        public ActionResult SendMail(string recipientEmail, string ngayGiao)
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
    <h2 style='text-align:center;'>Ngày giao: {ngayGiao}</h2>
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
            <td>{@item.sTenSach}</td>
            
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
                Credentials = new NetworkCredential("2224802010872@student.tdmu.edu.vn", "kjff rmzc ujwd vjrt"),
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
        public ActionResult PaymentVNPay(int? id, string ngayGiao)
        {
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();

            pay.AddRequestData("vnp_Version", "2.1.0"); // Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); // Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); // Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_Amount", TongTien().ToString() + "00"); // Số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) -> 1000000
                                                                               // TotalAmount() là phương thức trả về tổng tiền của đơn hàng.
            pay.AddRequestData("vnp_BankCode", "NCB"); // Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); // Ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); // Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); // Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); // Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toán đơn hàng"); // Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); // topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); // URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); // Mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            LuuDonHang(ngayGiao, false);
            return Redirect(paymentUrl);
        }
        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"];
                var vnpayData = Request.QueryString;

                PayLib pay = new PayLib();

                // Lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); // Mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); // Mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); // response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; // hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); // check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        DateTime t = new DateTime();
                        LuuDonHang(t.ToString(), true);
                        // Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                    }
                    else
                    {
                        // Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View("XacNhanDonHang");
        }
        private void LuuDonHang(string NgayGiao, Boolean daThanhToan)
        {
            // Thêm đơn hàng
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstCart = LayGioHang();

            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            if(daThanhToan==false)
            {
                var ngayGiao = String.Format("{0:MM/dd/yyyy}", NgayGiao);
                ddh.NgayGiao = DateTime.Parse(ngayGiao);
            }    
          

            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = daThanhToan;

            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();

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
            if(daThanhToan==true)
            {
                string ngayGiao = ddh.NgayGiao.HasValue ? ddh.NgayGiao.Value.ToString("dd/MM/yyyy") : string.Empty;
                SendMail(kh.Email, ngayGiao);
                Session["GioHang"] = null;
            }    

          
        }

    }
}