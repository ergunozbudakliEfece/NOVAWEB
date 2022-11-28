using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class Personel
    {
        public int USER_ID { get; set; }
        
        public string ISIM { get; set; }
        public string CINSIYET { get; set; }
        public string SOYISIM { get; set; }
        public string TCKN { get; set; } = null;
        
        public DateTime? DOGUM_TARIHI { get; set; }
        public string DOGUM_YERI_IL { get; set; } = null;
        public string DOGUM_YERI_ILCE { get; set; } = null;
        public string OGRENIM_DURUMU { get; set; } = null;
        public string MEZUN_OKUL { get; set; } = null;
        public string MEZUN_BOLUM { get; set; } = null;
        
        public int? MEZUN_YIL { get; set; }
        public string IKAMETGAH_ADRES { get; set; } = null;
        public string IKAMETGAH_IL { get; set; } = null;
        public string IKAMETGAH_ILCE { get; set; } = null;
        public string MEDENI_HAL { get; set; } = null;
        public string ES_CALISMA_DURUMU { get; set; } = null;
        public string ES_CALISMA_FIRMA { get; set; } = null;
        public string ES_UNVANI { get; set; } = null;
        public Int16? COCUK_SAYI { get; set; }
        public string IKAMET_EV_DURUM { get; set; } = null;
        public string ARAC_DURUM { get; set; } = null;
        public string ARAC_PLAKA { get; set; } = null;
        public string EHLIYET_VAR { get; set; } = null;
        public string EHLIYET_SINIF { get; set; } = null;
        [Column(TypeName = "datetime")]
        public DateTime? MEVCUT_IS_ILK_TARIH { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? IS_CIKIS_TARIH { get; set; }
        public string CALISILAN_BIRIM { get; set; } = null;
        public string GOREV { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ILK_IS_TARIH { get; set; }
        public string KAN_GRUP { get; set; } = null;
        public string SUBE { get; set; } = null;
        public string VARSA_SUREKLI_HAST { get; set; } = null;
        public string VARSA_ENGEL_DURUM { get; set; } = null;
        public string VARSA_SUREKLI_KULL_ILAC { get; set; } = null;
        public string COVID_ASI_DURUM { get; set; } = null;
        public Int16? KAC_DOZ_ASI { get; set; } 
        public string ILETISIM_BILGI_TEL { get; set; } = null;
        public string ILETISIM_BILGI_MAIL { get; set; } = null;
        public string ACIL_DURUM_KISI { get; set; } = null;
        public string ACIL_DURUM_KISI_ILETISIM { get; set; } = null;
        [Column(TypeName = "datetime")]
        public DateTime? KAYIT_TARIH { get; set; }

    }
}