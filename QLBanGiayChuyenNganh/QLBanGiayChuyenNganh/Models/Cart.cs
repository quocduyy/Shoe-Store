using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace QLBanGiayChuyenNganh.Models
{
    public class Cart
    {
        dbShoesDataContext data = new dbShoesDataContext();
        public int id { get; set; }

        public string ten { get; set; }

        public string hinh { get; set; }
        public string mau  { get; set; }
        public int size { get; set; }

        public double gia { get; set; }

        public int iSoluong { get; set; }

        public Double dThanhtien
        {
            get { return iSoluong * gia; }
        }
        public Cart(int id)
        {
            this.id = id;
            GIAY giay = data.GIAYs.SingleOrDefault(n => n.MaGiay == id);
            SIZE sizeGiay = data.SIZEs.SingleOrDefault(n => n.MaSize == giay.MaSize);
            MAU mauGiay = data.MAUs.SingleOrDefault(n => n.MaMau == giay.MaMau);
            ten = giay.TenGiay;
            hinh = giay.HinhGiay;
            gia = double.Parse(giay.GiaBan.ToString());
            mau = mauGiay.TenMau.ToString();
            size = (int)sizeGiay.SoSize;
            iSoluong = 1;
        }
    }
}