using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class FilterModel
    {
        public List<string> TESLIM_YERI { get; set; }
        public List<string> PLASIYER_ADI { get; set; }
        public List<string> STOK_ADI { get; set; }

        public List<string> CARI_DOVIZ_TIPI { get; set; }
        //[Column(TypeName = "datetime")]
        public DateTime? BASLANGIC_TARIH { get; set; }
        //[Column(TypeName = "datetime")]
        public DateTime? BITIS_TARIH { get; set; }
        public string URETILECEKMI1 { get; set; }
        public bool URETILECEKMI { get; set; }
        public string SSIP_CHECK { get; set; }

    }
}