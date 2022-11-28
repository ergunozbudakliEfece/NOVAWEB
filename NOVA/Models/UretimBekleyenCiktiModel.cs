﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class UretimBekleyenCiktiModel
    {
        public int? INCKEY { get; set; }
        public string MAK_KODU { get; set; }

        public string CIKTI_OLCU_BR1 { get; set; }
        public string CIKTI_OLCU_BR2 { get; set; }
        public string CIKTI_STOK_KODU { get; set; }
        public string CIKTI_STOK_ADI { get; set; }
        public decimal? CIKTI_MIKTAR { get; set; }
        public decimal? CIKTI_MIKTAR2 { get; set; }
        public bool? URETILDIMI { get; set; }
    }
}