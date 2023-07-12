using NetOpenX50;
using NOVA.Models;
using System.Collections.Generic;
using System.Net;
using System;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ServiceStack;
using System.Linq;

namespace NOVA.Utils
{
    public static class RoleHelper
    {
        public static void CheckRole(Controller Controller) 
        {
            #region Check Role
            List<User> yetki = GetRole(Controller.Request.Cookies["Id"].Value.ToInt());

            bool? yetkiKontrolSatis = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 1).USER_AUTH;
            bool? yetkiKontrolStok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 2).USER_AUTH;
            bool? yetkiKontrolUretim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 3).USER_AUTH;
            bool? yetkiKontrolSatinAlma = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 4).USER_AUTH;
            bool? yetkiKontrolFinans = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 5).USER_AUTH;
            bool? yetkiKontrolMuhasebe = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 6).USER_AUTH;
            bool? yetkiKontrol = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 9).USER_AUTH;
            bool? kullaniciayar = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 10).USER_AUTH;
            bool? kullaniciyetki = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 11).USER_AUTH;
            bool? istatistik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 12).USER_AUTH;
            bool? yetkiKontrolYonetim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            bool? yetkiKontrolSube = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 15).USER_AUTH;
            bool? yetkiKontrolSevkiyat = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 17).USER_AUTH;
            bool? yetkiKontrolDetayliSip = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 16).USER_AUTH;
            bool? yetkiKontrolSiparisRaporu = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 18).USER_AUTH;
            bool? yetkiKontrolIstatistik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 8).USER_AUTH;
            bool? yetkidetaylisatinalma = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 19).USER_AUTH;
            bool? yetkisaticisiparisi = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 20).USER_AUTH;
            bool? isemrikayit = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 27).USER_AUTH;
            bool? yetkianlikuretim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 26).USER_AUTH;
            bool? yetkifiyatlistok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 28).USER_AUTH;
            bool? yetkifiyatsizstok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 29).USER_AUTH;
            bool? ziyaretkaydi = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            bool? musteriraporu = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            bool? musteriraporuozel = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 31).USER_AUTH;
            bool? ziyaretplani = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 30).USER_AUTH;
            bool? yonetimstok = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 33).USER_AUTH;
            bool? fiyatyonetim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 34).USER_AUTH;
            bool? fiyatlistesi = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 35).USER_AUTH;
            bool? kuryetki = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 36).USER_AUTH;
            bool? uygulamaistatistik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 37).USER_AUTH;
            bool? puantaj = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 38).USER_AUTH;
            bool? teklif = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 39).USER_AUTH;
            bool? tekliflerim = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 40).USER_AUTH;
            bool? ik = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 7).USER_AUTH;
            bool? ik1 = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 13).USER_AUTH;
            bool? ik2 = yetki.FirstOrDefault(t => t.USER_ID == Controller.Request.Cookies["Id"].Value && t.MODULE_INCKEY == 14).USER_AUTH;
            #endregion

            #region SetViewBag
            Controller.ViewBag.DisplayTekliflerim = tekliflerim is true ? "unset" : "none";
            Controller.ViewBag.DisplayTeklif = teklif is true ? "unset" : "none";
            Controller.ViewBag.Puantaj = puantaj is true ? "unset" : "none";
            Controller.ViewBag.Istatistik = uygulamaistatistik is true ? "unset" : "none";
            Controller.ViewBag.DisplayKur = kuryetki is true ? "unset" : "none";
            Controller.ViewBag.DisplayFiyatListesi = fiyatlistesi is true ? "unset" : "none";
            Controller.ViewBag.FiyatYonetim = fiyatyonetim is true ? "unset" : "none";
            Controller.ViewBag.Stok = yonetimstok is true ? "unset" : "none";
            Controller.ViewBag.DisplayZiyaretPlani = ziyaretplani is true ? "unset" : "none";
            Controller.ViewBag.DisplayMusteriOzel = musteriraporuozel is true ? "unset" : "none";
            Controller.ViewBag.DisplayMusteriRaporu = ((musteriraporu == true && musteriraporuozel == false) || (Controller.Request.Cookies["Id"].Value == "10001" || Controller.Request.Cookies["Id"].Value == "10002")) ? "unset" : "none";
            Controller.ViewBag.DisplayZiyaretKaydi = ziyaretkaydi is true ? "unset" : "none";
            Controller.ViewBag.DisplayFiyatsizStok = yetkifiyatsizstok is true ? "unset" : "none";
            Controller.ViewBag.DisplayFiyatliStok = yetkifiyatlistok is true ? "unset" : "none";
            Controller.ViewBag.DisplayAnlikUretim = yetkianlikuretim is true ? "unset" : "none";
            Controller.ViewBag.DisplayIsEmriKayit = isemrikayit is true ? "unset" : "none";
            Controller.ViewBag.DisplaySaticiSiparisRaporu = yetkisaticisiparisi is true ? "unset" : "none";
            Controller.ViewBag.DisplaySatinAlmaRaporu = yetkidetaylisatinalma is true ? "unset" : "none";
            Controller.ViewBag.DisplayIstatistik = yetkiKontrolIstatistik is true ? "unset" : "none";
            Controller.ViewBag.DisplaySiparisRaporu = yetkiKontrolSiparisRaporu is true ? "unset" : "none";
            Controller.ViewBag.DisplaySevkiyat = yetkiKontrolSevkiyat is true ? "unset" : "none";
            Controller.ViewBag.DisplaySiparis = yetkiKontrolDetayliSip is true ? "unset" : "none";
            Controller.ViewBag.Sube = yetkiKontrolSube is true ? "unset" : "none";
            Controller.ViewBag.DisplayYonetim = yetkiKontrolYonetim is true ? "unset" : "none";
            Controller.ViewBag.DisplayUretim = yetkiKontrolUretim is true ? "unset" : "none";
            Controller.ViewBag.DisplaySatinAlma = yetkiKontrolSatinAlma is true ? "unset" : "none";
            Controller.ViewBag.DisplayFinans = yetkiKontrolFinans is true ? "unset" : "none";
            Controller.ViewBag.DisplayMuhasebe = yetkiKontrolMuhasebe is true ? "unset" : "none";
            Controller.ViewBag.DisplayStok = yetkiKontrolStok is true ? "unset" : "none";
            Controller.ViewBag.DisplaySatis = yetkiKontrolSatis is true ? "unset" : "none";
            Controller.ViewBag.Display = yetkiKontrol is true ? "unset" : "none";
            Controller.ViewBag.Display1 = kullaniciayar is true ? "unset" : "none";
            Controller.ViewBag.Display2 = kullaniciyetki is true ? "unset" : "none";
            Controller.ViewBag.Display3 = istatistik is true ? "unset" : "none";
            Controller.ViewBag.Display4 = ik is true ? "unset" : "none";
            Controller.ViewBag.Display5 = ik1 is true ? "unset" : "none";
            Controller.ViewBag.Display6 = ik2 is true ? "unset" : "none";
            #endregion
        }

        public static List<User> GetRole(int id)
        {
            var apiUrl = $"http://192.168.2.13:83/api/userwithroles/{id}";

            Uri url = new Uri(apiUrl);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string json = client.DownloadString(url);

            JavaScriptSerializer ser = new JavaScriptSerializer();
            List<User> jsonList = ser.Deserialize<List<User>>(json);
            return jsonList;
        }
    }
}