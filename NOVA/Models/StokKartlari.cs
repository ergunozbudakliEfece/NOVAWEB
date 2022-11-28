using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class StokKartlari
    {
        public string STOK_KODU { get; set; }
        public string STOK_ADI { get; set; }
       
        public string OLCU_BR1 { get; set; }
        public string OLCU_BR2 { get; set; }
        public string OLCU_BR3 { get; set; }
        public decimal? PAY_1 { get; set; }
        public double? PAYDA_1 { get; set; }
        public decimal? PAY2 { get; set; }
        public decimal? PAYDA2 { get; set; }
        public decimal? EN { get; set; }
        public decimal? BOY { get; set; }
        public decimal? GENISLIK { get; set; }

    }
}