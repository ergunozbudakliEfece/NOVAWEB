using System;

namespace NOVA.Models
{
    public class Etiket
    {
        public string SERI_NO { get; set; }
        public string STOK_ADI { get; set; }
        public double? MIKTAR1 { get; set; }
        public string OLCU_BR1 { get; set; }
        public double? MIKTAR2 { get; set; }
        public string OLCU_BR2 { get; set; }
        public string STOK_KODU { get; set; }
        public double? GENISLIK { get; set; }
        public string GRUP_ISIM { get; set; }
        public double? BOY { get; set; }
        public string KAYITYAPANKUL { get; set; }
        public DateTime? KAYITTARIHI { get; set; }
        public string SIPARIS_CARI { get; set; }
        public string MAK_KODU { get; set; }
        public string KALINLIK { get; set; }
        public double? METRAJ { get; set; }

        public double? KALITE { get; set; }
        public double? KAPLAMA { get; set; }
        public string MENSEI { get; set; }
        public string FIRMA_SERI_NO { get; set; }

        public string BIRIM_MIKTAR { get; set; }

        public string REF_HAT_KODU { get; set; }
        public string REF_STOK_ADI { get; set; }
    }
}