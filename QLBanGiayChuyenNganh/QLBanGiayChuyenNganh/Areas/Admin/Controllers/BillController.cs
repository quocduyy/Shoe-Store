using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Areas.Admin.Controllers
{
    [CustomAuthorization(Order = 1)]
    public class BillController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
        public ActionResult List()
        {
            var L_Bill = from r in data.HOADONs select r;
            return View(L_Bill);
        }
        public ActionResult Detail(int? id)
        {
            var hd = data.HOADONs.Where(p => p.MaHoaDon == id).FirstOrDefault();
            var ct_DonHang = data.CTHOADONs.Where(m => m.MaCTHD == id).ToList();
            ViewBag.MaDonHang = hd.MaHoaDon;
            ViewBag.NgayDat = hd.NgayLap;
            ViewBag.TinhTrang = hd.TINHTRANGDONHANG.TenTinhTrang;
            ViewBag.TongTien = hd.TongTien;
            return View(ct_DonHang);
        }

        //EXport
        public ActionResult Reports(String ReportType, int? id)
        {
            LocalReport localreport = new LocalReport();

            localreport.ReportPath = Server.MapPath("~/Reports/CTreport.rdlc");

            ReportDataSource rps = new ReportDataSource();
            rps.Name = "DataSet1";
         
           
            rps.Value = data.CTHOADONs.Where(m => m.MaCTHD == id).ToList();

            localreport.DataSources.Add(rps);
            string reportType = ReportType;
            string mineType;
            string encoding;
            string exten;

            if (reportType == "Excel")
            {

                exten = "xlsx";
            }
            else if (reportType == "Word")
            {
                exten = "docx";
            }
            else if (reportType == "PDF")
            {
                exten = "pdf";
            }
            else
            {
                exten = "jpg";
            }

            string[] stream;
            Warning[] warning;
            byte[] renderByte;
            renderByte = localreport.Render(reportType, "", out mineType, out encoding, out exten, out stream, out warning);

            Response.AddHeader("content-disposition", "attachment;filename= baocao_tdsneaker." + exten);



            return File(renderByte, exten);



        }

        public ActionResult CapNhatTrangThai(int? idDonHang, int? idTrangThai)
        {
            if (idDonHang != null && idTrangThai != null)
            {
                var donHang = data.HOADONs.Where(p => p.MaHoaDon == idDonHang).FirstOrDefault();
                switch (idTrangThai)
                {
                    case 1:
                        donHang.MaTinhTrang = 2; //Xác nhận
                        UpdateModel(donHang);
                        data.SubmitChanges();
                        break;
                    case 2:
                        donHang.MaTinhTrang = 3; //Giao Hàng
                        UpdateModel(donHang);
                        data.SubmitChanges();
                        break;
                    case 3:
                        donHang.MaTinhTrang = 4; //Hoàn thành
                        UpdateModel(donHang);
                        data.SubmitChanges();
                        break;

                }
            }
            return RedirectToAction("List", "Bill");
        }

        public ActionResult HuyDonHang(int? idDonHang)
        {
            if (idDonHang != null)
            {
                var donHang = data.HOADONs.Where(p => p.MaHoaDon == idDonHang).FirstOrDefault();
                donHang.MaTinhTrang = 5;
                UpdateModel(donHang);
                data.SubmitChanges();
            }
            return RedirectToAction("List", "Bill");
        }
    }
}