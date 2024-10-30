using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;

namespace HoTuanPhuoc.Controllers
{
    public class IMomoService : Controller
    {
        // GET: IMomoService
        private readonly string _momoApiUrl;
        private readonly string _secretKey;
        private readonly string _accessKey;
        private readonly string _returnUrl;
        private readonly string _notifyUrl;
        private readonly string _partnerCode;
        private readonly string _requestType;

        public IMomoService()
        {
            _momoApiUrl = ConfigurationHelper.GetSetting("MomoAPI", "MomoApiUrl");
            _secretKey = ConfigurationHelper.GetSetting("MomoAPI", "SecretKey");
            _accessKey = ConfigurationHelper.GetSetting("MomoAPI", "AccessKey");
            _returnUrl = ConfigurationHelper.GetSetting("MomoAPI", "ReturnUrl");
            _notifyUrl = ConfigurationHelper.GetSetting("MomoAPI", "NotifyUrl");
            _partnerCode = ConfigurationHelper.GetSetting("MomoAPI", "PartnerCode");
            _requestType = ConfigurationHelper.GetSetting("MomoAPI", "RequestType");
        }


        private string ComputeHmacSha256(string message, string secretKey)
        {
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);
            var messageBytes = Encoding.UTF8.GetBytes(message);

            byte[] hashBytes;

            using (var hmac = new HMACSHA256(keyBytes))
            {
                hashBytes = hmac.ComputeHash(messageBytes);
            }

            var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            return hashString;
        }
        //private double TongTien()
        //{
        //    double dTongTien = 0;
        //    List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
        //    if (lstGioHang != null)
        //    {
        //        dTongTien = lstGioHang.Sum(n => n.dThanhTien);
        //    }
        //    return dTongTien;
        //}
        //public async Task CreatePaymentAsync(DONDATHANG model)
        //{
        //    SachOnlineEntities db= new SachOnlineEntities();

        //    string OrderInfo = "khach hang mua sach" + db.KHACHHANGs.Where(m => m.MaKH == model.MaKH).SingleOrDefault().MaKH + "tien" + TongTien();

        //    var rawData = $"partnerCode={_partnerCode}&accessKey={_accessKey}&requestId={model.MaDonHang}&amount={model.Amount}&orderId={model.MaDonHang}&orderInfo={model.OrderInfo}&returnUrl={_returnUrl}&notifyUrl={_notifyUrl}&extraData=";

        //    var signature = ComputeHmacSha256(rawData, _secretKey);

        //    var client = new RestClient(_momoApiUrl);
        //    var request = new RestRequest() { Method = Method.Post };
        //    request.AddHeader("Content-Type", "application/json; charset=UTF-8");

        //    // Create an object representing the request data
        //    var requestData = new
        //    {
        //        accessKey = _accessKey,
        //        partnerCode = _partnerCode,
        //        requestType = _requestType,
        //        notifyUrl = _notifyUrl,
        //        returnUrl = _returnUrl,
        //        orderId = model.MaDonHang,
        //        amount = TongTien(),
        //        orderInfo = OrderInfo,
        //        requestId = model.MaDonHang,
        //        extraData = "",
        //        signature = signature
        //    };

        //    request.AddParameter("application/json", JsonConvert.SerializeObject(requestData), ParameterType.RequestBody);

        //    var response = await client.ExecuteAsync(request);

        //    return JsonConvert.DeserializeObject(response.Content);
        //}

        //public MomoExecuteResponseModel PaymentExecuteAsync(FormCollection collection)
        //{
        //    var amount = collection.First(s => s.Key == "amount").Value;
        //    var orderInfo = collection.First(s => s.Key == "orderInfo").Value;
        //    var orderId = collection.First(s => s.Key == "orderId").Value;
        //    return new MomoExecuteResponseModel()
        //    {
        //        Amount = amount,
        //        OrderId = orderId,
        //        OrderInfo = orderInfo
        //    };
        //}


    }
}