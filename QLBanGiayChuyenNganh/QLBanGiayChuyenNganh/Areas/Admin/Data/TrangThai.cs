using QLBanGiayChuyenNganh.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOANLAPTRINHWEB.Models
{
    public class TrangThai
    {
        dbShoesDataContext data = new dbShoesDataContext();
        public int MaTrangThai { get; set; }
        public string TenTrangThai { get; set; }
    }
}