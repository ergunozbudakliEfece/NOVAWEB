using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class SevkiyatBelge
    {
        public int INCKEY { get; set; }
        public int TYPE { get; set; }
        public bool ACTIVE { get; set; }
        public string BELGE_NO { get; set; }
        public string SIPARIS_NO { get; set; }
        public string IRS_NO { get; set; }
        public string CARI_ISIM { get; set; }
        public string SERI_NO { get; set; }
        public string STOK_KODU { get; set; }
        public string STOK_ADI { get; set; }
        public double MIKTAR1 { get; set; }
        public string OLCU_BR1 { get; set; }
        public double MIKTAR2 { get; set; }
        public string OLCU_BR2 { get; set; }
        public string ACIK1 { get; set; }
        public string ACIK2 { get; set; }
        public string ACIK3 { get; set; }
        public string SERI_NO_3 { get; set; }
        public string SERI_NO_4 { get; set; }
        public string ACIKLAMA_4 { get; set; }
        public string ACIKLAMA_5 { get; set; }
        public int INS_USER_ID { get; set; }
        public string INS_USER_NAME { get; set; }
        public DateTime INS_DATE { get; set; }
        public int UPD_USER_ID { get; set; }
        public string UPD_USER_NAME { get; set; }
        public DateTime UPD_DATE { get; set; }
        public string EXP_1 { get; set; }
        public string EXP_2 { get; set; }
        public string EXP_3 { get; set; }
        public int GIRIS_DEPO { get; set; }
        public int CIKIS_DEPO { get; set; }
        public string PLAKA { get; set; }
        public string SOFOR { get; set; }
    }
}