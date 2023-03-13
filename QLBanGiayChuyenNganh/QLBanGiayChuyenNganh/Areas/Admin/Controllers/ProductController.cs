using QLBanGiayChuyenNganh.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class ProductController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();

        // GET: Admin/Product
        public ActionResult List()
        {
            var L_Shoes = from r in data.GIAYs select r;
            return View(L_Shoes);
        }

        public ActionResult Add()
        {
            List<THUONGHIEU> thuonghieu = data.THUONGHIEUs.ToList();
            ViewBag.ThuongHieu = thuonghieu.Select(p => new SelectListItem { Text = p.TenThuongHieu, Value = p.MaThuongHieu.ToString() }).ToList();
            List<LOAIGIAY> loaigiay = data.LOAIGIAYs.ToList();
            ViewBag.LoaiGiay = loaigiay.Select(p => new SelectListItem { Text = p.TenLoaiGiay, Value = p.MaLoaiGiay.ToString() }).ToList();
            List<KIEUDANG> kieudang = data.KIEUDANGs.ToList();
            ViewBag.KieuDang = kieudang.Select(p => new SelectListItem { Text = p.TenKieuDang, Value = p.MaKieuDang.ToString() }).ToList();
            List<CHATLIEU> chatlieu = data.CHATLIEUs.ToList();
            ViewBag.ChatLieu = chatlieu.Select(p => new SelectListItem { Text = p.TenChatLieu, Value = p.MaChatLieu.ToString() }).ToList();
            List<MAU> mau = data.MAUs.ToList();
            ViewBag.Mau = mau.Select(p => new SelectListItem { Text = p.TenMau, Value = p.MaMau.ToString() }).ToList();
            List<SIZE> size = data.SIZEs.ToList();
            ViewBag.Size = size.Select(p => new SelectListItem { Text = p.SoSize.ToString(), Value = p.MaSize.ToString() }).ToList();
           
            List<GIOITINH> gioitinh = data.GIOITINHs.ToList();
            ViewBag.GioiTinh = gioitinh.Select(p => new SelectListItem { Text = p.TenGioiTinh, Value = p.MaGioiTinh.ToString() }).ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection, GIAY h, HttpPostedFileBase HinhGiay)
        {
            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(HinhGiay.FileName);
                var path = Path.Combine(Server.MapPath("/Content/images"), fileName);
                HinhGiay.SaveAs(path);


                var TenGiay = collection["TenGiay"];
                var MoTa = collection["MoTa"];
                var GioiTinh = Convert.ToInt32(collection["MaGioiTinh"]);
               
             
                var GiaNhap = Convert.ToDouble(collection["GiaNhap"]);
                var GiaBan = Convert.ToDouble(collection["GiaBan"]);
                var MaThuonghieu = Convert.ToInt32(collection["MaThuongHieu"]);
                var Maloaigiay = Convert.ToInt32(collection["MaLoaiGiay"]);
                var Mamau = Convert.ToInt32(collection["MaMau"]);
                var MaSize = Convert.ToInt32(collection["MaSize"]);
                var MaChatlieu = Convert.ToInt32(collection["MaChatLieu"]);
               
                var SoLuongTon = Convert.ToInt32(collection["SoLuongTon"]);

                h.TenGiay = TenGiay.ToString();
                h.MoTa = MoTa.ToString();
                h.MaGioiTinh = GioiTinh;
                
                h.GiaNhap = GiaNhap;
                h.GiaBan = GiaBan;
                h.HinhGiay = "/Content/images/" + fileName;
                h.MaThuongHieu = MaThuonghieu;
                h.MaLoaiGiay = Maloaigiay;
                h.MaMau = Mamau;
                h.MaSize = MaSize;
                h.MaChatLieu = MaChatlieu;
               
                h.SLTon = SoLuongTon;
                data.GIAYs.InsertOnSubmit(h);
                data.SubmitChanges();
                return RedirectToAction("List", "Product");
            }
            else
            {
                return View(h);

            }
        }

        public ActionResult index()
        {
            
            return View();
        }
        public ActionResult Statictical()
        {
            /*GIay ban chay nhat thang*/
            var result = from ctdh in data.CTHOADONs
                         from sp in data.GIAYs
                         from dh in data.HOADONs
                         where ctdh.MaGiay == sp.MaGiay
                            && dh.NgayLap.Value.Month == DateTime.Now.Month
                            && dh.NgayLap.Value.Year == DateTime.Now.Year
                         group ctdh by ctdh.MaGiay into g
                         select new GiayBanChay
                         {
                             IDGiay = g.FirstOrDefault().MaGiay.Value,
                             SoLuongBan = g.Sum(p => p.SoLuong).Value
                         };


            if (result.Count() > 0)
            {
                var maSP = result.OrderByDescending(p => p.SoLuongBan).Select(p => p.IDGiay).FirstOrDefault();
                var tenSanPham = data.GIAYs.FirstOrDefault(p => p.MaGiay == maSP).TenGiay;
                ViewBag.SanPhamBanChayNhat = tenSanPham;
            }
            else
            {
                ViewBag.SanPhamBanChayNhat = "Không có";
            }
           

            /*Khach hang mua nhieu nhat thang*/
            var result1 = from dh in data.HOADONs
                          from kh in data.KHACHHANGs
                          where dh.MaKhachHang == kh.MaKhachHang
                              && dh.NgayLap.Value.Month == DateTime.Now.Month
                              && dh.NgayLap.Value.Year == DateTime.Now.Year
                          group dh by dh.MaKhachHang into g
                          select new KhachHangMuaNhieuNhat
                          {
                              IDKhachHang = g.FirstOrDefault().MaKhachHang.Value,
                              TongTienMua = g.Sum(p => p.TongTien).Value
                          };

            if (result1.Count() > 0)
            {
                var maKH = result1.OrderByDescending(p => p.TongTienMua).Select(p => p.IDKhachHang).FirstOrDefault();
                var tenKhachHang = data.KHACHHANGs.FirstOrDefault(p => p.MaKhachHang == maKH).TenKhachHang;
                ViewBag.KhachHangMuaNhieuNhat = tenKhachHang;
            }
            else
            {
                ViewBag.KhachHangMuaNhieuNhat = "Không có";
            }
            return View();
        }

        public ActionResult Delete(int id)
        {
            var MaGiay = data.GIAYs.First(m => m.MaGiay == id);
            return View(MaGiay);
        }
        [HttpPost]



        public ActionResult Delete(int id, FormCollection collection)
        {
            var MaGiay = data.GIAYs.First(m => m.MaGiay == id);
            data.GIAYs.DeleteOnSubmit(MaGiay);
            data.SubmitChanges();
            return RedirectToAction("List", "Product");
        }

        public ActionResult Update(int id)
        {
            var MaGiay = data.GIAYs.First(m => m.MaGiay == id);
            List<THUONGHIEU> thuonghieu = data.THUONGHIEUs.ToList();
            ViewBag.ThuongHieu = thuonghieu.Select(p => new SelectListItem { Text = p.TenThuongHieu, Value = p.MaThuongHieu.ToString() }).ToList();
            List<LOAIGIAY> loaigiay = data.LOAIGIAYs.ToList();
            ViewBag.LoaiGiay = loaigiay.Select(p => new SelectListItem { Text = p.TenLoaiGiay, Value = p.MaLoaiGiay.ToString() }).ToList();
            List<KIEUDANG> kieudang = data.KIEUDANGs.ToList();
            ViewBag.kieudang = kieudang.Select(p => new SelectListItem { Text = p.TenKieuDang, Value = p.MaKieuDang.ToString() }).ToList();
            List<CHATLIEU> chatlieu = data.CHATLIEUs.ToList();
            ViewBag.ChatLieu = chatlieu.Select(p => new SelectListItem { Text = p.TenChatLieu, Value = p.MaChatLieu.ToString() }).ToList();
            List<MAU> mau = data.MAUs.ToList();
            ViewBag.Mau = mau.Select(p => new SelectListItem { Text = p.TenMau, Value = p.MaMau.ToString() }).ToList();
            List<SIZE> size = data.SIZEs.ToList();
            ViewBag.Size = size.Select(p => new SelectListItem { Text = p.SoSize.ToString(), Value = p.MaSize.ToString() }).ToList();
            
            List<GIOITINH> gioitinh = data.GIOITINHs.ToList();
            ViewBag.GioiTinh = gioitinh.Select(p => new SelectListItem { Text = p.TenGioiTinh, Value = p.MaGioiTinh.ToString() }).ToList();
            return View(MaGiay);
        }
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            if (ModelState.IsValid)
            {
                var MaGIAY = data.GIAYs.First(m => m.MaGiay == id);
                var TenGiay = collection["TenGiay"];
                var MoTa = collection["MoTa"];
                var GioiTinh = Convert.ToInt32(collection["MaGioiTinh"]);
                var HinhGiay = collection["HinhGiay"];

                var GiaNhap = Convert.ToDouble(collection["GiaNhap"]);
                var GiaBan = Convert.ToDouble(collection["GiaBan"]);
                var MaThuonghieu = Convert.ToInt32(collection["MaThuongHieu"]);
                var Maloaigiay = Convert.ToInt32(collection["MaLoaiGiay"]);
                var Mamau = Convert.ToInt32(collection["MaMau"]);
                var MaSize = Convert.ToInt32(collection["MaSize"]);
                var MaChatlieu = Convert.ToInt32(collection["MaChatLieu"]);

                var SoLuongTon = Convert.ToInt32(collection["SoLuongTon"]);

                MaGIAY.TenGiay = TenGiay.ToString();
                MaGIAY.MoTa = MoTa.ToString();
                MaGIAY.MaGioiTinh = GioiTinh;

                MaGIAY.GiaNhap = GiaNhap;
                MaGIAY.GiaBan = GiaBan;
                MaGIAY.HinhGiay = HinhGiay.ToString();
                MaGIAY.MaThuongHieu = MaThuonghieu;
                MaGIAY.MaLoaiGiay = Maloaigiay;
                MaGIAY.MaMau = Mamau;
                MaGIAY.MaSize = MaSize;
                MaGIAY.MaChatLieu = MaChatlieu;

                MaGIAY.SLTon = SoLuongTon;
                UpdateModel(MaGIAY);
                data.SubmitChanges();
            }
            return RedirectToAction("List", "Product");
        }
        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }
    }
}