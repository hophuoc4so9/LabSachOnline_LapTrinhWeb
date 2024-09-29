using HoTuanPhuoc.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace HoTuanPhuoc.Areas.admin.Controllers
{
    public class FileAndMailController : Controller
    {
        // GET: admin/FileAndMail
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SendMail(Mail model)
        {
            // Cấu hình thông tin Gmail (khai báo thư viện System.Net)
            var mail = new SmtpClient("smtp.gmail.com", 587)
            {
                // Khai báo thư viện System.Net
                Credentials = new NetworkCredential("2224802010872@student.tdmu.edu.vn", "coce jxbe bmql smty"),
                // Your email (abc@gmail.com) and your password (****)
                EnableSsl = true
            };

            // Tạo email
            var message = new MailMessage();
            message.From = new MailAddress(model.From);
            message.ReplyToList.Add(new MailAddress(model.From));
            message.To.Add(new MailAddress(model.To));
            message.Subject = model.Subject; // Hồ Tuán Phước - 2224802010872
            message.Body = model.Notes; // tự ghi

            var f = Request.Files["Attachment"];
            var path = Path.Combine(Server.MapPath("~/UploadFile"), f.FileName);
            if (!System.IO.File.Exists(path))
            {
                f.SaveAs(path);
            }

            // Khai báo thư viện System.Net.Mime
            Attachment data = new Attachment(Server.MapPath("~/UploadFile/" + f.FileName), MediaTypeNames.Application.Octet);
            message.Attachments.Add(data);

            mail.Send(message);
            return View("Index");
        }

    }
}