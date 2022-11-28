using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NOVA.Models
{
    public class Mail
    {
        public string Baslik { get; set; }
        public string Icerik { get; set; }
        public string AdSoyad { get; set; }
        public List<string> MailAdresi { get; set; }
        public string SingleMailAdresi { get; set; }
    }
}