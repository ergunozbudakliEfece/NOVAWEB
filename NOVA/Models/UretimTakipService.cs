using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace NOVA.Models
{
    public class UretimTakipService
    {
        
        public List<UretimTakip> TakipList() {
            var apiUrl = "http://192.168.2.13:83/api/uretimtakip/";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<UretimTakip> jsonList = ser.Deserialize<List<UretimTakip>>(json);
            
            return jsonList;
            
        }
    }
    
}