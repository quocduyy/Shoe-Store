using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class ShapeController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: Admin/Shape
        public ActionResult List()
        {
            var L_Shape = from r in data.KIEUDANGs select r;
            return View(L_Shape);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection, KIEUDANG hd)
        {
            var TenHinhDang = collection["TenKieuDang"];
            hd.TenKieuDang = TenHinhDang.ToString();
            data.KIEUDANGs.InsertOnSubmit(hd);
            data.SubmitChanges();
            return RedirectToAction("List", "Shape");
        }
        public ActionResult Update(int id)
        {
            var MaHinhDang = data.KIEUDANGs.First(m => m.MaKieuDang == id);
            return View(MaHinhDang);
        }
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            var MaHinhDang = data.KIEUDANGs.First(m => m.MaKieuDang == id);
            var TenHinhDang = collection["TenKieuDang"];
            MaHinhDang.TenKieuDang = TenHinhDang.ToString();
            UpdateModel(MaHinhDang);
            data.SubmitChanges();
            return RedirectToAction("List", "Shape");
        }
        public ActionResult Delete(int id)
        {
            var MaHinhDang = data.KIEUDANGs.First(m => m.MaKieuDang == id);
            return View(MaHinhDang);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var MaHinhDang = data.KIEUDANGs.First(m => m.MaKieuDang == id);
            data.KIEUDANGs.DeleteOnSubmit(MaHinhDang);
            data.SubmitChanges();
            return RedirectToAction("List", "Shape");
        }
    }
}