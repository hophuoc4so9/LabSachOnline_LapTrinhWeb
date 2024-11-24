using System.Web.Mvc;
using System.Web.Routing;

namespace HoTuanPhuoc
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
               name: "",
               url: "trang-chu",
               defaults: new { controller = "HoTuanPhuoc", action = "ThanhVienNhom", id = UrlParameter.Optional }
           );
            routes.MapRoute(
              name: "Trang Chu",
              url: "",
              defaults: new { controller = "HoTuanPhuoc", action = "Index", id = UrlParameter.Optional }
          );

            routes.MapRoute(
               name: "Trang Chu 2",
               url: "HoTuanPhuoc",
               defaults: new { controller = "HoTuanPhuoc", action = "Index", id = UrlParameter.Optional }
           );
            routes.MapRoute(
                        name: "Sach theo Chu de",
            url: "sach-theo-chu-de-{MaCD}",
            defaults: new { controller = "HoTuanPhuoc", action = "sachtheochude2", MaCD = UrlParameter.Optional },
            namespaces: new string[] { "HoTuanPhuoc.Controllers" }
            );

            routes.MapRoute(
                name: "Sach theo NXB",
                url: "sach-theo-nxb-{MaNXB}",
                defaults: new { controller = "HoTuanPhuoc", action = "sachtheonhaxuatban2", MaNXB = UrlParameter.Optional },
                namespaces: new string[] { "HoTuanPhuoc.Controllers" }
          );

            routes.MapRoute(
             name: "Chi tiet sach",
            url: "chi-tiet-sach-{id}",
            defaults: new { controller = "HoTuanPhuoc", action = "BookDetail", id = UrlParameter.Optional },
             namespaces: new string[] { "HoTuanPhuoc.Controllers" }

            );
           

            routes.MapRoute(
                        name: "Dang ky",
                url: "dang-ky",
                defaults: new { controller = "User", action = "DangKy" },
                namespaces: new string[] { "HoTuanPhuoc.Controllers" }
            );
            routes.MapRoute(
                       name: "Dang nhap",
                url: "dang-nhap",
                defaults: new { controller = "User", action = "DangNhap", url = UrlParameter.Optional },
               namespaces: new string[] { "HoTuanPhuoc.Controllers" }
           );
            routes.MapRoute(
                     name: "Trang tin",
              url: "{metatitle}",
              defaults: new { controller = "HoTuanPhuoc", action = "TrangTin", metatitle = UrlParameter.Optional },
             namespaces: new string[] { "HoTuanPhuoc.Controllers" }
         );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
