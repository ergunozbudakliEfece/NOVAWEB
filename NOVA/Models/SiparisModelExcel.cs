using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class SiparisModelExcel
    {
       

       
        public string TESLIM_YERI { get; set; }
        public string SIPARIS_NO { get; set; }
        public DateTime? TARIH { get; set; }
        public string CARI_KODU { get; set; }
        public string CARI_ISIM { get; set; }
        public string CARI_DOVIZ_TIPI { get; set; }
        public string STOK_KODU { get; set; }
        public decimal? DOV_NET_FIYAT { get; set; }
        public string STOK_ADI { get; set; }

        public decimal? MIKTAR { get; set; }
        public string OLCU_BR1 { get; set; }
        public decimal? MIKTAR2 { get; set; }
        
        public string OLCU_BR2 { get; set; }

        

        public decimal? BRUT_FIYAT { get; set; }

        public decimal? SATIS_ISKONTOSU { get; set; }

        public decimal? NET_FIYAT { get; set; }
        public decimal? NET_TUTAR_KALEM { get; set; }
        public decimal? KDV_KALEM { get; set; }
        public decimal? KDVLI_TUTAR_KALEM { get; set; }
        public decimal? DOV_FIAT { get; set; }
        public decimal? DOV_TUTAR { get; set; }
        public decimal? KUR { get; set; }
        public string DOV_TIP { get; set; }

        public Int16? ODEMEGUNU { get; set; }

        public string ACIKLAMA { get; set; }

        public string BAGLANTI_SIP_NO { get; set; }

        public decimal? BRUTTUTAR { get; set; }

        public decimal? SAT_ISKT { get; set; }

        public decimal? NETTUTAR { get; set; }

        public decimal? KDV_ORAN { get; set; }
        public decimal? TOPLAM_KDV { get; set; }
        public decimal? TEVKIFAT { get; set; }
        public decimal? GENELTOPLAM { get; set; }
        public decimal? DOV_GENELTOPLAM { get; set; }
        public decimal? DOV_TOPLAM_KDV { get; set; }
        public decimal? DOV_BRUTTUTAR { get; set; }
        public decimal? DOV_TEVKIFAT { get; set; }
        public string PLASIYER_KODU { get; set; }
        public string PLASIYER_ADI { get; set; }
        public string OZEL_KOD2 { get; set; }
        public string EKALAN1 { get; set; }

        public string EKALAN2 { get; set; }

        public decimal? TESLIM_MIKTARI { get; set; }

        public decimal? BAKIYE_MIKTARI { get; set; }

        public decimal? TESLIM_ADET { get; set; }

        public decimal? BAKIYE_ADET { get; set; }

        public string SIPARISI_ACAN { get; set; }

        public Int16? SIRA { get; set; }


        public DateTime? TESLIM_TARIHI { get; set; }

       
        public string TAHSILAT_SEKLI { get; set; }
        public string OZEL_KOD1 { get; set; }
        public string KUR_GIRILEN { get; set; }
        public string MAIL_KUR { get; set; }
        public double? ALIS_FIYATI_DVZ { get; set; }
        public decimal? ALIS_FIYATI_TL { get; set; }


        public DateTime? VADE_TARIHI { get; set; }

        public string PROJE_KODU { get; set; }

       

        public decimal? URETILEN_MIKTAR1 { get; set; }

        public decimal? URETILEN_MIKTAR2 { get; set; }
        public decimal? URETILECEK_MIKTAR1 { get; set; }
        public decimal? URETILECEK_MIKTAR2 { get; set; }

        public decimal? URETIM_ORANI { get; set; }
        public decimal? TESLIMAT_ORANI { get; set; }
        public string SSIP_NO { get; set; }
        public decimal? TESLIM_MIKTARI_SSIP { get; set; }
        public decimal? BAKIYE_MIKTARI_SSIP { get; set; }
        public decimal? TESLIMAT_ORANI_SSIP { get; set; }
    }
}