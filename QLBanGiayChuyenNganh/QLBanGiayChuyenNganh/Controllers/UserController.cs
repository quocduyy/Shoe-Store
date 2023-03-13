using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Net.Mail;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Controllers
{
    public class UserController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]

        public ActionResult Register(FormCollection collection, KHACHHANG kh, TAIKHOANKH tk)
        {
            //gan gia tri vao form
            var TenKhachHang = collection["HotenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var nhaplaimatkhau = collection["NhaplaimatKhau"];
            var email = collection["Email"];
            var diachi = collection["DiaChi"];
            var sdt = collection["DienThoai"];
            if (string.IsNullOrEmpty(TenKhachHang) || string.IsNullOrEmpty(tendn) || string.IsNullOrEmpty(matkhau) || string.IsNullOrEmpty(nhaplaimatkhau) || string.IsNullOrEmpty(email)
                || string.IsNullOrEmpty(sdt))
            {
                ViewData["Error"] = "Vui lòng nhập đầy đủ thông tin";
            }
            else
            {
                //Them Tai Khoan
                tk.MaTaiKhoanKH = tendn;
                tk.MatKhau = matkhau;
                tk.Email = email;
                tk.MaVaiTro = 2;
                data.TAIKHOANKHs.InsertOnSubmit(tk);
                data.SubmitChanges();

                //Them Khach Hang
                kh.TenKhachHang = TenKhachHang;
                kh.DiachiKH = diachi;
                kh.SDTKH = sdt;
                kh.MaTaiKhoanKH = tendn;
                data.KHACHHANGs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.Register();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {

            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];

            if (string.IsNullOrEmpty(tendn) || string.IsNullOrEmpty(matkhau))
            {
                ViewData["Error"] = "Vui lòng nhập đầy đủ thông tin";
            }
            else
            {
                TAIKHOANKH tk = data.TAIKHOANKHs.SingleOrDefault(n => n.MaTaiKhoanKH == tendn && n.MatKhau == matkhau);
                if (tk != null)
                {
                    if (tk.MaVaiTro == 1)
                    {
                        QUANTRI qt = data.QUANTRIs.SingleOrDefault(p => p.MaTaiKhoanKH == tk.MaTaiKhoanKH);
                        Session["QuanTri"] = qt;
                        return RedirectToAction("Statictical", "Product", new { area = "Admin" });
                    }
                    else
                    {
                        KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(p => p.MaTaiKhoanKH == tk.MaTaiKhoanKH);
                        Session["Taikhoan"] = kh;
                        return RedirectToAction("Index", "Shoes");
                    }
                }
                else
                {
                    ViewBag.ThongBao = "Sai tên đăng nhập hoặc mật khẩu";
                }

            }

            return View();
        }
        public ActionResult Logout()
        {
            Session["Taikhoan"] = null;
            return RedirectToAction("Index", "Shoes", new { area = "" });
        }

        public ActionResult ForgetPassword(string email = "")
        {
            if (email != "")
            {
                var account = data.TAIKHOANKHs.FirstOrDefault(p => p.Email == email);

                if (account != null)
                {
                    Session["TaiKhoanQuenMatKhau"] = account;
                    /*Gửi Mail*/
                    string emailShop = "thebees.hutech@gmail.com";
                    string passWordShop = "boksnmojcgaaskeh";
                    string emailKhachHang = account.Email;
                    MailMessage mailMessage = new MailMessage(emailShop, emailKhachHang);
                    Random r = new Random();
                    int maXacNhan = r.Next(10000, 99999);
                    mailMessage.Subject = "[THEBEES_HUTECH_SHOP] Xác nhận tài khoản";
                    mailMessage.Body = "Mã xác nhận tài khoản là: " + maXacNhan;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        System.Net.NetworkCredential nc = new NetworkCredential(emailShop, passWordShop);
                        smtp.Credentials = nc;
                        smtp.EnableSsl = true;
                        smtp.Send(mailMessage);
                    }
                    /**/
                    Session["Code"] = maXacNhan;
                    return RedirectToAction("ConfirmCode", "User");
                }
                else
                {
                    ViewBag.Error = "Email không tồn tại";
                }
            }
            return View();
        }

        public ActionResult ConfirmCode(int? confirmCode)
        {
            TAIKHOANKH account = Session["TaiKhoanQuenMatKhau"] as TAIKHOANKH;
            if (account != null)
            {
                int code = (int)Session["Code"];
                if (confirmCode != null)
                {
                    if (confirmCode == code)
                    {
                        return RedirectToAction("ChangeNewPassWord", "User");
                    }
                    else
                    {
                        ViewBag.MaXacNhan = "Mã xác nhận không hợp lệ";
                    }
                }
            }
            else
            {
                return RedirectToAction("ForgetPassword", "User");
            }
            return View();
        }

        public ActionResult ChangeNewPassWord(string newPassword = "", string newPassword1 = "")
        {
            TAIKHOANKH account = Session["TaiKhoanQuenMatKhau"] as TAIKHOANKH;
            if (account != null)
            {
                if (newPassword != "" && newPassword1 != "")
                {
                    if (newPassword == newPassword1)
                    {
                        TAIKHOANKH tk = data.TAIKHOANKHs.FirstOrDefault(p => p.MaTaiKhoanKH == account.MaTaiKhoanKH);
                        tk.MatKhau = newPassword;
                        UpdateModel(account);
                        data.SubmitChanges();
                        Session["TaiKhoanQuenMatKhau"] = null;
                        Session["Code"] = null;
                        return RedirectToAction("Login", "User");
                    }
                    else
                    {
                        ViewBag.Error = "Mật khẩu không khớp. Vui lòng nhập lại !";
                    }
                }
            }
            else
            {
                return RedirectToAction("ForgetPassword", "User");
            }

            return View();
        }

        public ActionResult Profile()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }
        public ActionResult SuaProfile(int id)
        {

            KHACHHANG sanpham = data.KHACHHANGs.SingleOrDefault(n => n.MaKhachHang == id);
            var E_sp = data.KHACHHANGs.First(m => m.MaKhachHang == id);
            return View(E_sp);
        }
        [HttpPost]
        public ActionResult SuaProfile(int id, FormCollection collection)
        {
            var E_sanpham = data.KHACHHANGs.First(m => m.MaKhachHang == id);
            var E_ten = collection["TenKhachHang"];
            var E_matkhau = collection["MatKhau"];
            var E_email = collection["Email"];
            var E_diachi = collection["DiachiKH"];
            var E_sdt = collection["SDTKH"];


            E_sanpham.MaKhachHang = id;
            if (string.IsNullOrEmpty(E_ten))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_sanpham.TenKhachHang = E_ten.ToString();
              
                E_sanpham.DiachiKH = E_diachi;
                E_sanpham.SDTKH = E_sdt;
                UpdateModel(E_sanpham);
                data.SubmitChanges();
                return RedirectToAction("Profile");
            }
            return this.SuaProfile(id);
        }

        public ActionResult NotAuthorize()
        {
            return View();
        }
    }
}