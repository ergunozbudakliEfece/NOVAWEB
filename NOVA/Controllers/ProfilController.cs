﻿
using DocumentFormat.OpenXml.EMMA;
using iTextSharp.text;
using NOVA.Models;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace NOVA.Controllers
{
    
    public class ProfilController : Controller
    {
        // GET: Profil
        public ActionResult Index()
        {
           
           
            Session["Name"]=Request.Cookies["Name"].Value.ToString();
            ViewBag.RoleName = Request.Cookies["RoleName"].Value.ToString();
            ViewBag.Id=Request.Cookies["Id"].Value.ToString();
            var yetki = GetYetki();
            var yetkiKontrolSatis = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 1).USER_AUTH;

            var yetkiKontrolStok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 2).USER_AUTH;
            var yetkiKontrolUretim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 3).USER_AUTH;
            var yetkiKontrolSatinAlma = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 4).USER_AUTH;
            var yetkiKontrolFinans = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 5).USER_AUTH;
            var yetkiKontrolMuhasebe = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 6).USER_AUTH;
            var yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 9).USER_AUTH;
            var kullaniciayar = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 10).USER_AUTH;
            var kullaniciyetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 11).USER_AUTH;
            var istatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 12).USER_AUTH;
            var yetkiKontrolYonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            var yetkiKontrolSube = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 15).USER_AUTH;
            var yetkiKontrolSevkiyat = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 17).USER_AUTH;
            var yetkiKontrolDetayliSip = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 16).USER_AUTH;
            var yetkiKontrolSiparisRaporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 18).USER_AUTH;
            var yetkiKontrolIstatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            var yetkidetaylisatinalma = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 19).USER_AUTH;
            var yetkisaticisiparisi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 20).USER_AUTH;
            var isemrikayit = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 27).USER_AUTH;
            var yetkianlikuretim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 26).USER_AUTH;
            var yetkifiyatlistok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28).USER_AUTH;
            var yetkifiyatsizstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 29).USER_AUTH;
            var ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            var ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            var yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            var fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            var fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            var kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
            var uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            var puantaj = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38).USER_AUTH;
            var teklif = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 39).USER_AUTH;
            var tekliflerim = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 40).USER_AUTH;
            if (tekliflerim != true)
            {
                ViewBag.DisplayTekliflerim = "none";
            }
            else
            {
                ViewBag.DisplayTekliflerim = "unset";
            }
            if (teklif != true)
            {
                ViewBag.DisplayTeklif = "none";
            }
            else
            {
                ViewBag.DisplayTeklif = "unset";
            }
            if (puantaj != true)
            {
                ViewBag.Puantaj = "none";
            }
            else
            {
                ViewBag.Puantaj = "unset";
            }
            if (uygulamaistatistik != true)
            {
                ViewBag.Istatistik = "none";
            }
            else
            {
                ViewBag.Istatistik = "unset";
            }
            if (kuryetki != true)
            {
                ViewBag.DisplayKur = "none";
            }
            else
            {
                ViewBag.DisplayKur = "unset";
            }
            if (fiyatlistesi != true)
            {
                ViewBag.DisplayFiyatListesi = "none";
            }
            else
            {
                ViewBag.DisplayFiyatListesi = "unset";
            }
            if (fiyatyonetim != true)
            {
                ViewBag.FiyatYonetim = "none";
            }
            else
            {
                ViewBag.FiyatYonetim = "unset";
            }
            if (yonetimstok != true)
            {
                ViewBag.Stok = "none";
            }
            else
            {
                ViewBag.Stok = "unset";
            }
            if (ziyaretplani != true)
            {
                ViewBag.DisplayZiyaretPlani = "none";
            }
            else
            {
                ViewBag.DisplayZiyaretPlani = "unset";
            }
            if (musteriraporuozel != true)
            {
                ViewBag.DisplayMusteriOzel = "none";
            }
            else
            {
                ViewBag.DisplayMusteriOzel = "unset";
            }
            if ((musteriraporu == true && musteriraporuozel == false) || (Request.Cookies["Id"].Value == "10001" || Request.Cookies["Id"].Value == "10002"))
            {
                ViewBag.DisplayMusteriRaporu = "unset";
            }
            else
            {
                ViewBag.DisplayMusteriRaporu = "none";

            }
            if (ziyaretkaydi != true)
            {
                ViewBag.DisplayZiyaretKaydi = "none";
            }
            else
            {
                ViewBag.DisplayZiyaretKaydi = "unset";
            }
            if (yetkifiyatsizstok != true)
            {
                ViewBag.DisplayFiyatsizStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatsizStok = "unset";
            }
            if (yetkifiyatlistok != true)
            {
                ViewBag.DisplayFiyatliStok = "none";
            }
            else
            {
                ViewBag.DisplayFiyatliStok = "unset";
            }
            if (yetkianlikuretim != true)
            {
                ViewBag.DisplayAnlikUretim = "none";
            }
            else
            {
                ViewBag.DisplayAnlikUretim = "unset";
            }
            if (isemrikayit != true)
            {
                ViewBag.DisplayIsEmriKayit = "none";
            }
            else
            {
                ViewBag.DisplayIsEmriKayit = "unset";
            }
            if (yetkisaticisiparisi != true)
            {
                ViewBag.DisplaySaticiSiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySaticiSiparisRaporu = "unset";
            }
            if (yetkidetaylisatinalma != true)
            {
                ViewBag.DisplaySatinAlmaRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlmaRaporu = "unset";
            }
            if (yetkiKontrolIstatistik != true)
            {
                ViewBag.DisplayIstatistik = "none";
            }
            else
            {
                ViewBag.DisplayIstatistik = "unset";
            }
            if (yetkiKontrolSiparisRaporu != true)
            {
                ViewBag.DisplaySiparisRaporu = "none";
            }
            else
            {
                ViewBag.DisplaySiparisRaporu = "unset";
            }
            if (yetkiKontrolSevkiyat != true)
            {
                ViewBag.DisplaySevkiyat = "none";
            }
            else
            {
                ViewBag.DisplaySevkiyat = "unset";
            }
            if (yetkiKontrolDetayliSip != true)
            {
                ViewBag.DisplaySiparis = "none";
            }
            else
            {
                ViewBag.DisplaySiparis = "unset";
            }
            if (yetkiKontrolSube != true)
            {
                ViewBag.Sube = "none";
            }
            else
            {
                ViewBag.Sube = "unset";
            }
            if (yetkiKontrolYonetim != true)
            {
                ViewBag.DisplayYonetim = "none";
            }
            else
            {
                ViewBag.DisplayYonetim = "unset";
            }
            if (yetkiKontrolUretim != true)
            {
                ViewBag.DisplayUretim = "none";
            }
            else
            {
                ViewBag.DisplayUretim = "unset";
            }
            if (yetkiKontrolSatinAlma != true)
            {
                ViewBag.DisplaySatinAlma = "none";
            }
            else
            {
                ViewBag.DisplaySatinAlma = "unset";
            }
            if (yetkiKontrolFinans != true)
            {
                ViewBag.DisplayFinans = "none";
            }
            else
            {
                ViewBag.DisplayFinans = "unset";
            }
            if (yetkiKontrolMuhasebe != true)
            {
                ViewBag.DisplayMuhasebe = "none";
            }
            else
            {
                ViewBag.DisplayMuhasebe = "unset";
            }
            if (yetkiKontrolStok != true)
            {
                ViewBag.DisplayStok = "none";
            }
            else
            {
                ViewBag.DisplayStok = "unset";
            }
            if (yetkiKontrolSatis != true)
            {
                ViewBag.DisplaySatis = "none";
            }
            else
            {
                ViewBag.DisplaySatis = "unset";
            }
            if (yetkiKontrol != true)
            {
                ViewBag.Display = "none";
            }
            else
            {
                ViewBag.Display = "unset";
            }
            if (kullaniciayar != true)
            {
                ViewBag.Display1 = "none";
            }
            else
            {
                ViewBag.Display1 = "unset";
            }
            if (kullaniciyetki != true)
            {
                ViewBag.Display2 = "none";
            }
            else
            {
                ViewBag.Display2 = "unset";
            }
            if (istatistik != true)
            {
                ViewBag.Display3 = "none";
            }
            else
            {
                ViewBag.Display3 = "unset";
            }
            var ik = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 7).USER_AUTH;
            if (ik != true)
            {
                ViewBag.Display4 = "none";
            }
            else
            {
                ViewBag.Display4 = "unset";
            }
            var ik1 = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 13).USER_AUTH;
            if (ik1 != true)
            {
                ViewBag.Display5 = "none";
            }
            else
            {
                ViewBag.Display5 = "unset";
            }
            var ik2 = yetki.FirstOrDefault(t => t.USER_ID == Request.Cookies["Id"].Value && t.MODULE_INCKEY == 14).USER_AUTH;
            if (ik2 != true)
            {
                ViewBag.Display6 = "none";
            }
            else
            {
                ViewBag.Display6 = "unset";
            }
            return View();
        }
        public async Task<ActionResult> SendPasswordMail()
        {
            WebMail.SmtpServer = "192.168.2.13";
            
               var  subject = "NOVA | Şifre Değişikliği";
            
            var apiUrl = "http://192.168.2.13:83/api/user/link/1";

            var httpClient = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            var response = await httpClient.SendAsync(request);
            var img = "<img src = \"data:image/png;base64, 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 \"/>";

            string u = "http://192.168.2.13/sifre/index?l=" + Encrypt(GetLink().ToString()) + "&us=" + Encrypt(Request.Cookies["UserName"].Value);
            string body = "Merhaba " + Request.Cookies["FirstName"].Value + " " + Request.Cookies["LastName"].Value + ",</br></br>" +
                 "<a href='" + u + "'>Buradaki bağlantıya</a> tıklayarak açılan sayfada yeni şifrenizi belirleyebilirsiniz.</br></br>" +
                 "İyi çalışmalar dileriz.</br></br>" +
                img;

            WebMail.Send(Request.Cookies["Mail"].Value, subject, body, "sistem@efecegalvaniz.com", null, null, true, null, null, null, null, null, null);
            return RedirectToAction("Index");
        }
        public int GetLink()
        {


            var apiUrl = "http://192.168.2.13:83/api/user/link";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<LinkM> jsonList = ser.Deserialize<List<LinkM>>(json);

            //END

            return jsonList[jsonList.Count - 1].ID;
        }
        public List<User> GetYetki()
        {
            var apiUrl = "http://192.168.2.13:83/api/userwithroles";

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            return jsonList;
        }
        public List<SignIn> GetSession(int id)
        {


            var apiUrl = "http://192.168.2.13:83/api/log/max:" + id;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<SignIn> jsonList = ser.Deserialize<List<SignIn>>(json);

            //END

            return jsonList;
        }
        public List<User> GetYetki(int id, int inckey)
        {
            var apiUrl = "http://192.168.2.13:83/api/userwithroles/" + id + "/" + inckey;

            //Connect API
            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);
            //END

            //JSON Parse START
            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            return jsonList;
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