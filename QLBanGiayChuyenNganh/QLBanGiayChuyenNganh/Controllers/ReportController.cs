using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Reporting.WebForms;
using System.IO;
using QLBanGiayChuyenNganh.Models;

namespace QLBanGiayChuyenNganh.Controllers
{
    public class ReportController : Controller
    {
        dbShoesDataContext data = new dbShoesDataContext();
       
        public ActionResult reportdh()
        {
            return View(data.HOADONs.ToList());
        }

        public ActionResult Reports(String ReportType)
        {
            LocalReport localreport = new LocalReport();

            localreport.ReportPath = Server.MapPath("~/Reports/HdReport.rdlc");

            ReportDataSource rps = new ReportDataSource();
            rps.Name = "DataSet1";
            
            rps.Value = data.HOADONs.ToList();
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
            renderByte = localreport.Render(reportType,"", out mineType, out encoding, out exten, out stream, out warning);

            Response.AddHeader("content-disposition", "attachment;filename= baocao_tdsneaker." + exten);



            return File(renderByte, exten);

           

        }
    }
}