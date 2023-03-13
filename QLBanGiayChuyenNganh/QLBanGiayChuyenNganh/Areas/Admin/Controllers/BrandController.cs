using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class BrandController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: Admin/Brand
        public ActionResult List()
        {
            var L_Brand = from r in data.THUONGHIEUs select r;
            return View(L_Brand);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection, THUONGHIEU th)
        {
            var TenThuongHieu = collection["TenThuongHieu"];
            th.TenThuongHieu = TenThuongHieu.ToString();
            data.THUONGHIEUs.InsertOnSubmit(th);
            data.SubmitChanges();
            return RedirectToAction("List", "Brand");
        }
        public ActionResult Update(int id)
        {
            var MaThuongHieu = data.THUONGHIEUs.First(m => m.MaThuongHieu == id);
            return View(MaThuongHieu);
        }
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            var MaThuongHieu = data.THUONGHIEUs.First(m => m.MaThuongHieu == id);
            var TenThuongHieu = collection["TenThuongHieu"];
            MaThuongHieu.TenThuongHieu = TenThuongHieu.ToString();
            UpdateModel(MaThuongHieu);
            data.SubmitChanges();
            return RedirectToAction("List", "Brand");
        }
        public ActionResult Delete(int id)
        {
            var MaThuongHieu = data.THUONGHIEUs.First(m => m.MaThuongHieu == id);
            return View(MaThuongHieu);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var MaThuongHieu = data.THUONGHIEUs.First(m => m.MaThuongHieu == id);
            data.THUONGHIEUs.DeleteOnSubmit(MaThuongHieu);
            data.SubmitChanges();
            return RedirectToAction("List", "Brand");
        }
    }
}