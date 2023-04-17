using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Text;
using ServiceStack;
using System.Net;
using NOVA.Models;
using System.IO;
using System.Security.Cryptography;
using System.Text.Json;

namespace NOVA.Controllers
{
    public class SifreController : Controller
    {
        [AllowAnonymous]
        // GET: Sifre
        public ActionResult Index(string l)
        {
            int id = Decrypt(l).ToInt();
            var apiUrl = "http://192.168.2.13:83/api/user/link/" + id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

          
            var jsonList = JsonSerializer.Deserialize<List<LinkM>>(json);
            if (jsonList[0].SITUATION == "T")
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
      
        public class ModelControl
        {
            public int ID { get; set; }
        }
        [AllowAnonymous]
        public async Task<ActionResult> KullaniciUpdate(UserModel1 user)
        {
            string apiUrl;
            var httpClient = new HttpClient();
            HttpRequestMessage request;
            HttpResponseMessage response;
            user.USER_NAME=Decrypt(user.USER_NAME);
            user.LINK_ID = Decrypt(user.LINK_ID);
            apiUrl = "http://192.168.2.13:83/api/user/update/" + user.USER_NAME;
            user.USER_ID = "0";


            request = new HttpRequestMessage(HttpMethod.Put, apiUrl)
            {
                Content = new StringContent(new JavaScriptSerializer().Serialize(user), Encoding.UTF8, "application/json")
            };

            response = await httpClient.SendAsync(request);

            string apiUrl1;
            var httpClient1 = new HttpClient();
            HttpRequestMessage request1;
            HttpResponseMessage response1;
            apiUrl1 = "http://192.168.2.13:83/api/user/link/" + user.LINK_ID;


            request1 = new HttpRequestMessage(HttpMethod.Put, apiUrl1);

            response1 = await httpClient1.SendAsync(request1);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        
        public class UserModel1
        {
            public string USER_ID { get; set; }


            public string USER_NAME { get; set; }

            public string USER_PASSWORD { get; set; }

            public string USER_FIRSTNAME { get; set; }


            public string USER_LASTNAME { get; set; }


            public string USER_ROLE { get; set; }
            public bool ACTIVE { get; set; }
            public string USER_MAIL { get; set; }
            public string LINK_ID { get; set;}
        }
        public class LinkModel
        {
            public int ID { get; set; }

        }
        public static string Encrypt(string clearText)
        {
            string EncryptionKey = "abc123";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
        public static string Decrypt(string cipherText)
        {
            string EncryptionKey = "abc123";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}