using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class MaterialController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        // GET: Admin/Brand
        public ActionResult List()
        {
            var L_Meterial = from r in data.CHATLIEUs select r;
            return View(L_Meterial);
        }
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Add(FormCollection collection, CHATLIEU th)
        {
            var TenThuongHieu = collection["TenChatLieu"];
            th.TenChatLieu = TenThuongHieu.ToString();
            data.CHATLIEUs.InsertOnSubmit(th);
            data.SubmitChanges();
            return RedirectToAction("List", "Material");
        }
        public ActionResult Update(int id)
        {
            var MaThuongHieu = data.CHATLIEUs.First(m => m.MaChatLieu == id);
            return View(MaThuongHieu);
        }
        [HttpPost]
        public ActionResult Update(int id, FormCollection collection)
        {
            var MaThuongHieu = data.CHATLIEUs.First(m => m.MaChatLieu == id);
            var TenThuongHieu = collection["TenChatLieu"];
            MaThuongHieu.TenChatLieu = TenThuongHieu.ToString();
            UpdateModel(MaThuongHieu);
            data.SubmitChanges();
            return RedirectToAction("List", "Material");
        }
        public ActionResult Delete(int id)
        {
            var MaThuongHieu = data.CHATLIEUs.First(m => m.MaChatLieu == id);
            return View(MaThuongHieu);
        }
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var MaThuongHieu = data.CHATLIEUs.First(m => m.MaChatLieu == id);
            data.CHATLIEUs.DeleteOnSubmit(MaThuongHieu);
            data.SubmitChanges();
            return RedirectToAction("List", "Material");
        }
    }
}