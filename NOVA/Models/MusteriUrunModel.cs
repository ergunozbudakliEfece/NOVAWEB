using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class MusteriUrunModel
    {
        public int URUN_ID { get; set; }
        public string URUN_GRUBU { get; set; }
       
        public int? MUSTERI_ID { get; set; }
        public string KAYIT_YAPAN_KULLANICI { get; set; }
        public decimal? YILLIK_KULLANIM { get; set; }
        public string OLCU_BIRIMI { get; set; }
        public DateTime? KAYIT_TARIHI { get; set; }
        public string DEGISIKLIK_YAPAN_KULLANICI { get; set; }
        public DateTime? DEGISIKLIK_TARIHI { get; set; }
    }
}