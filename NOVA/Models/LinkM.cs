using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class LinkM
    {
        public int ID { get; set; }
        public string SITUATION { get; set; }
        public DateTime? START_DATE { get; set; }
        public int? DURATION { get; set; }
        public DateTime? END_DATE { get; set; }
    }
}