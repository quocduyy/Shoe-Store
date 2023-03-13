using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class CategoryController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: Admin/Category
        public ActionResult List()
        {
            var L_Category = from r in data.LOAIGIAYs select r;
            return View(L_Category);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection, LOAIGIAY hd)
        {
            var TenHinhDang = collection["TenLoaiGiay"];
            hd.TenLoaiGiay = TenHinhDang.ToString();
            data.LOAIGIAYs.InsertOnSubmit(hd);
            data.SubmitChanges();
            return RedirectToAction("List", "Category");
        }
        public ActionResult Update(int id)
        {
            var MaHinhDang = data.LOAIGIAYs.First(m => m.MaLoaiGiay == id);
            return View(MaHinhDang);
        }
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            var MaHinhDang = data.LOAIGIAYs.First(m => m.MaLoaiGiay == id);
            var TenHinhDang = collection["TenLoaiGiay"];
            MaHinhDang.TenLoaiGiay = TenHinhDang.ToString();
            UpdateModel(MaHinhDang);
            data.SubmitChanges();
            return RedirectToAction("List", "Category");
        }
        public ActionResult Delete(int id)
        {
            var MaHinhDang = data.LOAIGIAYs.First(m => m.MaLoaiGiay == id);
            return View(MaHinhDang);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var MaHinhDang = data.LOAIGIAYs.First(m => m.MaLoaiGiay == id);
            data.LOAIGIAYs.DeleteOnSubmit(MaHinhDang);
            data.SubmitChanges();
            return RedirectToAction("List", "Category");
        }
    }
}