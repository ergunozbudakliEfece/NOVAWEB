using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class UretCikti
    {
        [Key]
        public int INCKEY { get; set; }
        public string MAK_KODU { get; set; }
        public int URETIM_ONAY_ID { get; set; }
        public string CIKTI_SERI_NO { get; set; }

        public decimal CIKTI_MIKTAR { get; set; }

        public decimal CIKTI_MIKTAR2 { get; set; }

        public string URETIM_KAYIT_ID { get; set; }
        public DateTime TARIH { get; set; }
        public bool URETILDIMI { get; set; }
    }
}