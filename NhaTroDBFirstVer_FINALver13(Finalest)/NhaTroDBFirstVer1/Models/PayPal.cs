using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NhaTroDBFirstVer1.Models
{
    public class PayPal
    {
        public PayPal(int idHoaDon)
        {
            cmd = "_cart";
            business = "nguyenhuuvinh2001@gmail.com"; //Email của người bán
            upload = "1";
            currency_code = "USD";
            @return = "http://localhost/DatHang/DaThanhToan?idHoaDon=" + idHoaDon;
            cancel_return = "http://localhost/DatHang/ChuaThanhToan?idHoaDon=" + idHoaDon;
            notify_url = "http://localhost/DatHang/ThongBao?idHoaDon=" + idHoaDon;
        }

        public string cmd { get; set; }
        public string business { get; set; }
        public string upload { get; set; }
        public string @return { get; set; }
        public string cancel_return { get; set; }
        public string notify_url { get; set; }
        public string currency_code { get; set; }
        public string shipping_1 { get; set; } //Cước phí 
        public string tax_cart { get; set; } //Thuế
    }
}