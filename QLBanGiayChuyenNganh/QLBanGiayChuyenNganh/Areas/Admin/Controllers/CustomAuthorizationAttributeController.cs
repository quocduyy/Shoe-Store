using QLBanGiayChuyenNganh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;


namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    internal class CustomAuthorizationAttribute : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            dbShoesDataContext data = new dbShoesDataContext();

            TAIKHOANKH db = new TAIKHOANKH();
            KHACHHANG kh = (KHACHHANG)HttpContext.Current.Session["Taikhoan"];
            QUANTRI qt = (QUANTRI)HttpContext.Current.Session["QuanTri"];
            
            if (kh == null && qt == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "Login", area = "" }));
            }
            else
            {
                TAIKHOANKH tk;
                if (kh != null)
                {
                    tk = data.TAIKHOANKHs.FirstOrDefault(p => p.MaTaiKhoanKH == kh.MaTaiKhoanKH);
                }
                else
                {
                    tk = data.TAIKHOANKHs.FirstOrDefault(p => p.MaTaiKhoanKH == qt.MaTaiKhoanKH);
                }
                var hasRole = tk.MaVaiTro;
                if (hasRole > Order)
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "User", action = "NotAuthorize", area = "" }));
                }
            }
        }
    }
}