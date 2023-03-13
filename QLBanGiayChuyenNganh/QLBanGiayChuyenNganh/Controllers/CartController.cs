using QLBanGiayChuyenNganh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PayPal.Api;

namespace QLBanGiayChuyenNganh.Controllers
{
    public class CartController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        public List<Cart> Laygiohang()
        {
            List<Cart> lstGiohang = Session["ViewCart"] as List<Cart>;
            if (lstGiohang == null)
            {
                lstGiohang = new List<Cart>();
                Session["ViewCart"] = lstGiohang;
            }
            return lstGiohang;
        }
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Cart> lstGiohang = Laygiohang();
            Cart sanpham = lstGiohang.Find(n => n.id == id);
            if (sanpham == null)
            {
                sanpham = new Cart(id);
                lstGiohang.Add(sanpham);
                Session["CountProductCart"] = TongSoLuong();
                return Redirect(strURL);
            }
            else
            {
                sanpham.iSoluong++;
                Session["CountProductCart"] = TongSoLuong();
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int tsl = 0;
            List<Cart> lstGiohang = Session["ViewCart"] as List<Cart>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Sum(n => n.iSoluong);
            }
            return tsl;
        }
        private int TongSoLuongSanPham()
        {
            int tsl = 0;
            List<Cart> lstGiohang = Session["ViewCart"] as List<Cart>;
            if (lstGiohang != null)
            {
                tsl = lstGiohang.Count;
            }
            return tsl;
        }
        private double TongTien()
        {
            double tt = 0;
            List<Cart> lstGiohang = Session["ViewCart"] as List<Cart>;
            if (lstGiohang != null)
            {
                tt = lstGiohang.Sum(n => n.dThanhtien);
            }
            return tt;
        }
        public ActionResult ViewCarts()
        {
            List<Cart> lstGiohang = Laygiohang();
            ViewBag.ViewCarts = lstGiohang.Count;
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);

        }
        public ActionResult GioHangPartial()
        {
            TempData["Tongsoluong"] = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            ViewBag.Tongsoluongsanpham = TongSoLuongSanPham();
            return PartialView();
        }
        public ActionResult XoaGiohang(int id)
        {
            List<Cart> lstGiohang = Laygiohang();

            Cart sanpham = lstGiohang.SingleOrDefault(n => n.id == id);
            if (sanpham != null)
            {
                lstGiohang.RemoveAll(n => n.id == id);
                Session["CountProductCart"] = TongSoLuong();
                return RedirectToAction("ViewCarts");
            }
            return RedirectToAction("ViewCarts");
        }
        public ActionResult CapnhatGiohang(int id, FormCollection collection)
        {
            List<Cart> lstGiohang = Laygiohang();
            Cart sanpham = lstGiohang.SingleOrDefault(n => n.id == id);
            if (sanpham != null)
            {

                sanpham.iSoluong = int.Parse(collection["txtSoLg"].ToString());
                Session["CountProductCart"] = TongSoLuong();
            }
            return RedirectToAction("ViewCarts");
        }
        public ActionResult XoaTatCaGioHang()
        {
            List<Cart> lstGiohang = Laygiohang();
            lstGiohang.Clear();
            Session["CountProductCart"] = TongSoLuong();
            return RedirectToAction("ViewCarts");
        }
        [HttpGet]
        public ActionResult Dathang()
        {
            // kiem tra dang nhap
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            if (Session["ViewCart"] == null)
            {
                return RedirectToAction("Index", "Shoes");
            }
            List<Cart> lstGiohang = Laygiohang();
            if (lstGiohang.Count == 0)
            {
                return RedirectToAction("Index", "Shoes");
            }
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.Tongtien = TongTien();
            return View(lstGiohang);
        }
        public ActionResult Dathang(FormCollection collection)
        {
            GIAY sp = new GIAY();
            HOADON ddh = new HOADON();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Cart> lstGiohang = Laygiohang();
            ddh.MaKhachHang = kh.MaKhachHang;
            ddh.NgayLap = DateTime.Now;
            ddh.MaTinhTrang = 1;
            ddh.TongTien = TongTien();
            data.HOADONs.InsertOnSubmit(ddh);
            data.SubmitChanges();

            //Them chi tiet don hang
            foreach (var item in lstGiohang)
            {
                CTHOADON ctdh = new CTHOADON();
                ctdh.MaHoaDon = ddh.MaHoaDon;
                ctdh.MaGiay = item.id;
                ctdh.SoLuong = item.iSoluong;
                ctdh.ThanhTien = (float)item.dThanhtien;

                sp = data.GIAYs.FirstOrDefault(n => n.MaGiay == item.id);
                sp.SLTon -= item.iSoluong;
                data.SubmitChanges();
                data.CTHOADONs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();

            /*Gửi Mail*/
            string emailShop = "thebees.hutech@gmail.com";
            string passWordShop = "boksnmojcgaaskeh";
            var account = data.TAIKHOANKHs.FirstOrDefault(p => p.MaTaiKhoanKH == kh.MaTaiKhoanKH);
            string emailKhachHang = account.Email;
            MailMessage mailMessage = new MailMessage(emailShop, emailKhachHang);

            string ctDonHang = "Tên sản phẩm   Số lượng   Thành tiền\n";
            foreach (var item in lstGiohang)
            {
                ctDonHang += $"{item.ten}\t\t{item.iSoluong}\t\t{item.dThanhtien}\n";
            }
            mailMessage.Subject = "[THEBEES_HUTECH_SHOP] Thông báo đơn hàng";
            mailMessage.Body = $"CẢM ƠN BẠN ĐÃ ĐẶT HÀNG, CHÚNG TÔI SẼ GỬI ĐƠN HÀNG CHO BẠN SỚM NHẤT! \n\n " +
                $"Mã hóa đơn: {ddh.MaHoaDon}\n Ngày đặt: {ddh.NgayLap}\n Số điện thoại: {kh.SDTKH} \n Giao tới: {kh.DiachiKH}\n Tổng tiền: {ddh.TongTien}\n\n" +
                 "-----------------------------------------------\n" +
                "CHI TIẾT ĐƠN HÀNG\n" +
                $"{ctDonHang}" +
                "------------------------------------------------\n" +
                "Cảm ơn bạn đã mua sản phẩm bên mình <3";

            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                System.Net.NetworkCredential nc = new NetworkCredential(emailShop, passWordShop);
                smtp.Credentials = nc;
                smtp.EnableSsl = true;
                smtp.Send(mailMessage);
            }
            /**/
            Session["ViewCart"] = null;
            return RedirectToAction("DatHangThanhCong", "Cart");
        }
        public ActionResult DatHangThanhCong()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("Login", "User");
            }
            return View();
        }


        // Work with Paypal Payment
        private Payment payment;
        // Create a paypment using an APIContext
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var listItems = new ItemList() { items = new List<Item>() };

            List<Cart> listCarts = (List<Cart>)Session["ViewCart"];
            foreach (var cart in listCarts)
            {
                listItems.items.Add(new Item()
                {
                    name = cart.ten,
                    currency = "USD",
                    price = cart.gia.ToString(),
                    quantity = cart.iSoluong.ToString(),
                    sku = "sku"
                });
            }

            var payer = new Payer() { payment_method = "paypal" };

            // Do the configuration RedirectURLs here with redirectURLs object
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl,
                return_url = redirectUrl
            };

            // Create details object
            var details = new Details()
            {
                tax = "1",
                shipping = "2",
                subtotal = listCarts.Sum(x => x.iSoluong * x.gia).ToString()



            };

            // Create amount object
            var amount = new Amount()
            {
                currency = "USD",
                total = (Convert.ToDouble(details.tax) + Convert.ToDouble(details.shipping) + Convert.ToDouble(details.subtotal)).ToString(),// tax + shipping + subtotal
                details = details
            };

            // Create transaction
            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "HAHGAH",
                invoice_number = Convert.ToString((new Random()).Next(100000)),
                amount = amount,
                item_list = listItems
            });

            payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };

            return payment.Create(apiContext);
        }

        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            payment = new Payment() { id = paymentId };
            return payment.Execute(apiContext, paymentExecution);
        }

        public ActionResult PaymentWithPaypal()
        {
            // Gettings context from the paypal bases on clientId and clientSecret for payment
            APIContext apiContext = PaypalConfiguration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    // Creating a payment
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Cart/PaymentWithPaypal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = CreatePayment(apiContext, baseURI + "guid=" + guid);

                    // Get links returned from paypal response to create call funciton
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = string.Empty;

                    while (links.MoveNext())
                    {
                        Links link = links.Current;
                        if (link.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = link.href;
                        }
                    }

                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    // This one will be executed when we have received all the payment params from previous call
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        //Remove shopping cart session
                        Session.Remove("ViewCart");
                        return View("Failure");
                    }
                }
            }
            catch (Exception ex)
            {
                PaypalLogger.Log("Error: " + ex.Message);
                //Remove shopping cart session
                Session.Remove("ViewCart");
                return View("Failure");
            }

            //Remove shopping cart session
            Session.Remove("ViewCart");
           

            return View("DatHangThanhCong");
        }

    }
}