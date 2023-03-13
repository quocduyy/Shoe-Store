using QLBanGiayChuyenNganh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class AdminController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: Admin/Admin
        public ActionResult List()
        {
            var L_Admin = from r in data.QUANTRIs select r;
            return View(L_Admin);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(FormCollection collection, QUANTRI qt, TAIKHOANKH tk)
        {
            //gan gia tri vao form
            var TenQuanTri = collection["HotenQT"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var sdt = collection["DienThoai"];
            if (string.IsNullOrEmpty(TenQuanTri) || string.IsNullOrEmpty(tendn) || string.IsNullOrEmpty(matkhau)
                 || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(sdt))
            {
                ViewData["Error"] = "Vui lòng nhập đầy đủ thông tin";
            }
            else
            {
                //Them Tai Khoan
                tk.MaTaiKhoanKH = tendn;
                tk.MatKhau = matkhau;
                tk.Email = email;
                tk.MaVaiTro = 1;
                data.TAIKHOANKHs.InsertOnSubmit(tk);
                data.SubmitChanges();

                //Them Quan Tri
                qt.TenQT = TenQuanTri;
                qt.DiachiQT = diachi;
                qt.SDTQT = sdt;
                qt.MaTaiKhoanKH = tendn;
                data.QUANTRIs.InsertOnSubmit(qt);
                data.SubmitChanges();
                return RedirectToAction("List", "Admin");
            }
            return this.Register();
        }

        public ActionResult Logout()
        {
            Session["QuanTri"] = null;
            return RedirectToAction("Login", "User", new { area = "" });
        }

        public ActionResult Thongtintaikhoan(int id)
        {
            var D_detail = data.QUANTRIs.Where(m => m.MaQT == id).First();
            return View(D_detail);
        }

        public ActionResult Delete(int id)
        {
            var MaQuanTri = data.QUANTRIs.First(m => m.MaQT == id);
            return View(MaQuanTri);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var MaQuanTri = data.QUANTRIs.First(m => m.MaQT == id);
            data.QUANTRIs.DeleteOnSubmit(MaQuanTri);
            data.SubmitChanges();
            return RedirectToAction("List", "Admin");
        }
        public ActionResult Update(int id)
        {
            var MaQuanTri = data.QUANTRIs.First(m => m.MaQT == id);
            return View(MaQuanTri);
        }
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            var QuanTri = data.QUANTRIs.First(m => m.MaQT == id);
            var TaiKhoan = data.TAIKHOANKHs.First(m => m.MaTaiKhoanKH == QuanTri.MaTaiKhoanKH);
            //gan gia tri vao form
            var TenQuanTri = collection["TenQT"];
            var DiachiQT = collection["DiachiQT"];
            var DienThoaiQT = collection["SDTQT"];
            var MatKhau = collection["TAIKHOANKH.MatKhau"];
            var Email = collection["TAIKHOANKH.Email"];

            TaiKhoan.MatKhau = MatKhau.ToString();
            TaiKhoan.Email = Email.ToString();
            QuanTri.TenQT = TenQuanTri.ToString();
            QuanTri.DiachiQT = DiachiQT.ToString();
            QuanTri.SDTQT = DienThoaiQT.ToString();

            UpdateModel(TaiKhoan);
            UpdateModel(QuanTri);
            data.SubmitChanges();
            return RedirectToAction("List", "Admin");
        }
    }
}