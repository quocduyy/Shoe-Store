using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;
using QLBanGiayChuyenNganh.Models;


namespace QLBanGiayChuyenNganh.Controllers
{
    public class ShoesController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: Shoes
        public ActionResult Index(int gioitinh = 0)
        {
            var I_Shoes = from r in data.GIAYs select r;
            return View(I_Shoes);
        }

        public ActionResult Sort(int giaban= 0)
        {
            List<GIAY> giay = data.GIAYs.ToList();
            var orderByResult = from s in giay
                                orderby s.GiaBan //Sorts the studentList collection in ascending order
                                select s;
            return View(orderByResult);
        }

        public ActionResult SortDesc(int giaban = 0)
        {
            List<GIAY> giay = data.GIAYs.ToList();
            var orderByResult = from s in giay
                                orderby s.GiaBan descending //Sorts the studentList collection in ascending order
                                select s;
            return View(orderByResult);
        }

        public ActionResult Product(string SearchProduct = "", int thuonghieu = 0, int kieudang = 0, int loaigiay = 0, int mau = 0, int size = 0, int chatlieu = 0, int gioitinh = 0)
        {
            if (SearchProduct != "")
            {
                var search_Watch = data.GIAYs.Where(p => p.TenGiay.ToUpper().Contains(SearchProduct.ToUpper())).ToList();
                return View(search_Watch);
            }

            if (thuonghieu != 0)
            {
                var filterThuongHieu = data.GIAYs.Where(p => p.MaThuongHieu == thuonghieu).ToList();
                return View(filterThuongHieu);
            }
            if (kieudang != 0)
            {
                var filterLoaiDay = data.GIAYs.Where(p => p.MaKieuDang == kieudang).ToList();
                return View(filterLoaiDay);
            }
            if (chatlieu != 0)
            {
                var filterChatLieu = data.GIAYs.Where(p => p.MaChatLieu == chatlieu).ToList();
                return View(filterChatLieu);
            }
            if (loaigiay != 0)
            {
                var filterHinhDang = data.GIAYs.Where(p => p.MaLoaiGiay == loaigiay).ToList();
                return View(filterHinhDang);
            }
            if (size != 0)
            {
                var filterKichThuoc = data.GIAYs.Where(p => p.MaSize == size).ToList();
                return View(filterKichThuoc);
            }
            if (mau != 0)
            {
                var fillterMucDo = data.GIAYs.Where(p => p.MaMau == mau).ToList();
                return View(fillterMucDo);
            }

            if (gioitinh != 0)
            {
                var fillterGioiTinh = data.GIAYs.Where(p => p.MaGioiTinh == gioitinh).ToList();
                return View(fillterGioiTinh);
            }
            var P_Watch = from r in data.GIAYs select r;
            return View(P_Watch);
        }
        public ActionResult ProductDetail(int? id)
        {
            var PD_Watch = data.GIAYs.Where(m => m.MaGiay == id).First();
            List<MAU> mAUs = data.MAUs.ToList();
            ViewBag.Mau = mAUs.Select(p => new SelectListItem { Text = p.TenMau, Value = p.MaMau.ToString() }).ToList();
            List<SIZE> sIZEs = data.SIZEs.ToList();
            ViewBag.Size = sIZEs.Select(p => new SelectListItem { Text = p.SoSize.ToString(), Value = p.MaSize.ToString() }).ToList();
            return View(PD_Watch);
        }
    }
}