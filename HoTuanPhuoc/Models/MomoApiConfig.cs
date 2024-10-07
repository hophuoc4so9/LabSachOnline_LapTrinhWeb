using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HoTuanPhuoc.Models
{
    public class MomoApiConfig
    {
        public string MomoApiUrl { get; set; }
        public string SecretKey { get; set; }
        public string AccessKey { get; set; }
        public string ReturnUrl { get; set; }
        public string NotifyUrl { get; set; }
        public string PartnerCode { get; set; }
        public string RequestType { get; set; }
    }

}