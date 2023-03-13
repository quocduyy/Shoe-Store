using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class ColorController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: Admin/Brand
        public ActionResult List()
        {
            var L_Color = from r in data.MAUs select r;
            return View(L_Color);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection, MAU th)
        {
            var TenThuongHieu = collection["TenMau"];
            th.TenMau = TenThuongHieu.ToString();
            data.MAUs.InsertOnSubmit(th);
            data.SubmitChanges();
            return RedirectToAction("List", "Color");
        }
        public ActionResult Update(int id)
        {
            var MaThuongHieu = data.MAUs.First(m => m.MaMau == id);
            return View(MaThuongHieu);
        }
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            var MaThuongHieu = data.MAUs.First(m => m.MaMau == id);
            var TenThuongHieu = collection["TenMau"];
            MaThuongHieu.TenMau = TenThuongHieu.ToString();
            UpdateModel(MaThuongHieu);
            data.SubmitChanges();
            return RedirectToAction("List", "Color");
        }
        public ActionResult Delete(int id)
        {
            var MaThuongHieu = data.MAUs.First(m => m.MaMau == id);
            return View(MaThuongHieu);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var MaThuongHieu = data.MAUs.First(m => m.MaMau == id);
            data.MAUs.DeleteOnSubmit(MaThuongHieu);
            data.SubmitChanges();
            return RedirectToAction("List", "Material");
        }
    }
}