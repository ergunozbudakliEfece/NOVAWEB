using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class MusteriRandevu
    {
        public int INCKEY { get; set; }
        public int? MUSTERI_ID { get; set; }
        public DateTime? PLANLANAN_TARIH { get; set; }
        public int? KAYIT_YAPAN_KULLANICI_ID { get; set; }
        public DateTime? KAYIT_TARIHI { get; set; }
        public int? DEGISIKLIK_YAPAN_KULLANICI_ID { get; set; }
        public DateTime? DEGISIKLIK_TARIHI { get; set; }
        public DateTime? GERCEKLESEN_TARIH { get; set; }
        public string RANDEVU_NOTU { get; set; }
    }
}